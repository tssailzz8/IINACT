using Advanced_Combat_Tracker;
using CactbotSelf;
using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.Hooking;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIV_ACT_Plugin.Logfile;
using IINACT.Network;
using IINACT.Windows;
using Machina.FFXIV;
using Machina.FFXIV.Headers.Opcodes;
using PostNamazu;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IINACT;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class Plugin : IDalamudPlugin
{
    public string Name => "IINACT";
    public Version Version { get; }

    private const string MainWindowCommandName = "/iinact";
    private const string EndEncCommandName = "/endenc";
    private const string ChatCommandName = "/chat";
    public readonly WindowSystem WindowSystem = new("IINACT");
    
    internal IDalamudPluginInterface PluginInterface { get; }
    internal ICommandManager CommandManager { get; }
    internal IClientState ClientState { get; }
    internal IDataManager DataManager { get; }
    internal IChatGui ChatGui { get; }
    internal IFramework Framework { get; }
    internal ICondition Condition { get; }
    internal IGameInteropProvider GameInteropProvider { get; }
    internal ISigScanner SigScanner { get; }
    internal INotificationManager NotificationManager { get; }
    public static IPluginLog Log { get; private set; } = null!;

    internal Configuration Configuration { get; }
    private TextToSpeechProvider TextToSpeechProvider { get; }
    private MainWindow MainWindow { get; }
    internal FileDialogManager FileDialogManager { get; }
    private ZoneDownHookManager ZoneDownHookManager { get; }
    private IpcProviders IpcProviders { get; }

    private FfxivActPluginWrapper FfxivActPluginWrapper { get; }
    private RainbowMage.OverlayPlugin.PluginMain OverlayPlugin { get; set; }
    private RainbowMage.OverlayPlugin.WebSocket.ServerController? WebSocketServer { get; set; }
    internal string OverlayPluginStatus => OverlayPlugin.Status;
    private PluginLogTraceListener PluginLogTraceListener { get; }
    private HttpClient HttpClient { get; }
    private delegate void OnUpdateInputUI(IntPtr EventArgument, IntPtr parm1);
    private Hook<OnUpdateInputUI> onUpdateInputUIHook;
    private static readonly Queue<string> ChatQueue = new();
    public DateTime NextClick;
    private delegate long ReplayZonePacketDownDelegate(long a, long targetId, long dataPtr);
    public static CactbotSelf.CactbotSelf cactboSelf;
    public static PostNamazu.PostNamazu post;
    public Plugin(IDalamudPluginInterface pluginInterface,
                  ICommandManager commandManager,
                  IClientState clientState,
                  IDataManager dataManager,
                  IChatGui chatGui,
                  IFramework framework,
                  ICondition condition,
                  IPluginLog pluginLog,
                  IGameInteropProvider gameInteropProvider,
                  ISigScanner sigScanner,
                  INotificationManager notificationManager)
    {
        PluginInterface = pluginInterface;
        CommandManager = commandManager;
        DataManager = dataManager;
        ClientState = clientState;
        ChatGui = chatGui;
        Framework = framework;
        Condition = condition;
        GameInteropProvider = gameInteropProvider;
        SigScanner = sigScanner;
        NotificationManager = notificationManager;
        Log = pluginLog;
        DalamudApi.Initialize(this, pluginInterface);
        OpcodeManager.Instance.SetRegion(DataManager.Language.ToString() == "ChineseSimplified"
                                             ? GameRegion.Chinese
                                             : GameRegion.Global);

        var createZoneDownHookManager = Task.Run(() 
            => new ZoneDownHookManager(NotificationManager, GameInteropProvider));
        Version = Assembly.GetExecutingAssembly().GetName().Version!;

        FileDialogManager = new FileDialogManager();

        HttpClient = new HttpClient();
        
        var fetchDeps =
            new FetchDependencies.FetchDependencies(Version, PluginInterface.AssemblyLocation.Directory!.FullName,
                                                    DataManager.Language.ToString() == "ChineseSimplified", HttpClient);
        
        fetchDeps.GetFfxivPlugin();
        
        PluginLogTraceListener = new PluginLogTraceListener();
        Trace.Listeners.Add(PluginLogTraceListener);

        Advanced_Combat_Tracker.ActGlobals.Init();
        Advanced_Combat_Tracker.ActGlobals.oFormActMain = new Advanced_Combat_Tracker.FormActMain(Log);

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);

        this.TextToSpeechProvider = new TextToSpeechProvider();
        Advanced_Combat_Tracker.ActGlobals.oFormActMain.LogFilePath = Configuration.LogFilePath;

        FfxivActPluginWrapper = new FfxivActPluginWrapper(Configuration, DataManager.Language, ChatGui, Framework, Condition);
        OverlayPlugin = InitOverlayPlugin();

        IpcProviders = new IpcProviders(PluginInterface);

        MainWindow = new MainWindow(this);

        WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler(MainWindowCommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Displays the IINACT main window"
        });

        CommandManager.AddHandler(EndEncCommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Ends the current encounter IINACT is parsing"
        });
        DalamudApi.CommandManager.AddHandler(ChatCommandName, new CommandInfo(ChatDo)
        {
            HelpMessage = "chat"
        });
        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        
        if (clientState.IsPvP)
            EnterPvP();
        else
            LeavePvP();
        
        ClientState.EnterPvP += EnterPvP;
        ClientState.LeavePvP += LeavePvP;

        ZoneDownHookManager = createZoneDownHookManager.Result;
        onUpdateInputUIHook = DalamudApi.Hook.HookFromAddress<OnUpdateInputUI>(
           DalamudApi.SigScanner.ScanText("4C 8B DC 53 55 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 48 83 B9 ?? ?? ?? ?? ??"), OnUpdateInputUIDo);
        onUpdateInputUIHook.Enable();


        this.NextClick = DateTime.Now;
    }
    private ILogOutput _logOutput;
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal bool WriteLogLineImpl(int ID, string line)
    {

        var timestamp = DateTime.Now;
        _logOutput?.WriteLine((FFXIV_ACT_Plugin.Logfile.LogMessageType)ID, timestamp, line);
        return true;
    }
    private void setZone()
    {
        var terr = DalamudApi.GameData.GetExcelSheet<Lumina.Excel.Sheets.TerritoryType>();
        var zoneID = DalamudApi.ClientState.TerritoryType;
        var zoneName = terr?.GetRowOrDefault(zoneID)?.PlaceName.Value.Name.ToString();
        ActGlobals.oFormActMain.CurrentZone = zoneName;
        if (_logOutput == null)
        {
            var plugin = ActGlobals.oFormActMain.FfxivPlugin;
            _logOutput = (ILogOutput)plugin._iocContainer.GetService(typeof(ILogOutput));
        }
        string text = FormatChangeZoneMessage(zoneID, zoneName);
        WriteLogLineImpl(1, text);
    }
    public string FormatChangeZoneMessage(uint ZoneId, string ZoneName)
    {
        return ((FormattableString)$"{ZoneId:X2}|{ZoneName}").ToString(CultureInfo.InvariantCulture);
    }
    private void ChatDo(string command, string arguments)
    {
        string[] array = arguments.Split(new char[]
    {
                    ' '
    });
        if (array.Length >= 2)
        {
            NextClick = DateTime.Now.AddSeconds(1 / 6);
            ChatQueue.Enqueue(arguments);
            //ChatHelper.SendMessage(arguments);
        }

    }
    private void OnUpdateInputUIDo(nint EventArgument, nint parm1)
    {
        onUpdateInputUIHook.Original(EventArgument, parm1);
        var now = DateTime.Now;

        if (this.NextClick < now && ChatQueue.Count > 0)
        {

            var com = ChatQueue.Dequeue();
            ChatHelper.SendMessage(com);
            this.NextClick = now.AddSeconds(1 / 4);
        }
    }
    public void Dispose()
    {
        ClientState.EnterPvP -= EnterPvP;
        ClientState.LeavePvP -= LeavePvP;
        IpcProviders.Dispose();
        ZoneDownHookManager.Dispose();
        
        FfxivActPluginWrapper.Dispose();
        OverlayPlugin.DeInitPlugin();
        Trace.Listeners.Remove(PluginLogTraceListener);

        WindowSystem.RemoveAllWindows();

        MainWindow.Dispose();

        CommandManager.RemoveHandler(MainWindowCommandName);
        CommandManager.RemoveHandler(EndEncCommandName);
        CommandManager.RemoveHandler(ChatCommandName);

        Advanced_Combat_Tracker.ActGlobals.Dispose();
    }

    private RainbowMage.OverlayPlugin.PluginMain InitOverlayPlugin()
    {
        var container = new RainbowMage.OverlayPlugin.TinyIoCContainer();
        
        var logger = new RainbowMage.OverlayPlugin.Logger(Log);
        container.Register(logger);
        container.Register<RainbowMage.OverlayPlugin.ILogger>(logger);

        container.Register(HttpClient);
        container.Register(FileDialogManager);
        container.Register(PluginInterface);

        var overlayPlugin = new RainbowMage.OverlayPlugin.PluginMain(
            PluginInterface.AssemblyLocation.Directory!.FullName, logger, container);
        container.Register(overlayPlugin);
        Advanced_Combat_Tracker.ActGlobals.oFormActMain.OverlayPluginContainer = container;
        
        Task.Run(() =>
        {
            overlayPlugin.InitPlugin(PluginInterface.ConfigDirectory.FullName);

            var registry = container.Resolve<RainbowMage.OverlayPlugin.Registry>();
            MainWindow.OverlayPresets = registry.OverlayTemplates;
            WebSocketServer = container.Resolve<RainbowMage.OverlayPlugin.WebSocket.ServerController>();
            MainWindow.Server = WebSocketServer;
            IpcProviders.Server = WebSocketServer;
            IpcProviders.OverlayIpcHandler = container.Resolve<RainbowMage.OverlayPlugin.Handlers.Ipc.IpcHandlerController>();
            MainWindow.OverlayPluginConfig = container.Resolve<RainbowMage.OverlayPlugin.IPluginConfig>();
            try
            {
                post = new PostNamazu.PostNamazu(DalamudApi.CommandManager);
                post.InitPlugin();
            }
            catch (Exception e)
            {

                DalamudApi.LogError(e.ToString());
            }

            cactboSelf = new CactbotSelf.CactbotSelf(Configuration.shunxu, true);
            cactboSelf.InitPlugin();
            DalamudApi.LogInfo("初始化鲶鱼精");

            if (DalamudApi.ClientState.IsLoggedIn)
            {

                setZone();
            }
        });

        return overlayPlugin;
    }

    private void OnCommand(string command, string args)
    {
        if (command == EndEncCommandName)
        {
            Advanced_Combat_Tracker.ActGlobals.oFormActMain.EndCombat(false);
            return;
        }
            
        switch (args) 
        {
            case "start": //deprecated
            case "ws start":
                WebSocketServer?.Start();
                break;
            case "stop": //deprecated
            case "ws stop":
                WebSocketServer?.Stop();
                break;
            case "log start":
                Configuration.WriteLogFile = true;
                Configuration.Save();
                break;
            case "log stop":
                Configuration.WriteLogFile = false;
                Configuration.Save();
                break;
            case "log pvp start":
                Configuration.DisablePvp = false;
                Configuration.Save();
                break;
            case "log pvp stop":
                Configuration.DisablePvp = true;
                Configuration.Save();
                break;
            default:
                MainWindow.IsOpen = true;
                break;
        }
    }

    private void DrawUI()
    {
        WindowSystem.Draw();
        FileDialogManager.Draw();
    }

    public void DrawConfigUI()
    {
        MainWindow.IsOpen = true;
    }

    private void EnterPvP()
    {
        if (Configuration is not { DisablePvp: true, DisableWritingPvpLogFile: false })
            return;

        Configuration.DisableWritingPvpLogFile = true;
    }

    private void LeavePvP()
    {
        Configuration.DisableWritingPvpLogFile = false;
    }
}
