using System.Diagnostics;
using System.Reflection;
using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.Hooking;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using FFXIV_ACT_Plugin.Logfile;
using IINACT.Windows;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Advanced_Combat_Tracker;

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
    
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
    internal Configuration Configuration { get; }
    private TextToSpeechProvider TextToSpeechProvider { get; }
    private MainWindow MainWindow { get; }
    internal FileDialogManager FileDialogManager { get; }
    private GameServerTime GameServerTime { get; }
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


    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        DalamudApi.Initialize(this, pluginInterface);
        PluginLogTraceListener = new PluginLogTraceListener();
        Trace.Listeners.Add(PluginLogTraceListener);
        
        FileDialogManager = new FileDialogManager();

        Machina.FFXIV.Dalamud.DalamudClient.GameNetwork = DalamudApi.GameNetwork;

        HttpClient = new HttpClient();
        var fetchDeps = new FetchDependencies.FetchDependencies(
            DalamudApi.PluginInterface.AssemblyLocation.Directory!.FullName, HttpClient, DalamudApi.ClientState.ClientLanguage);
        
        fetchDeps.GetFfxivPlugin();
        
        PluginLogTraceListener = new PluginLogTraceListener();
        Trace.Listeners.Add(PluginLogTraceListener);

        Advanced_Combat_Tracker.ActGlobals.oFormActMain = new Advanced_Combat_Tracker.FormActMain(DalamudApi.PluginLog);

        Configuration = DalamudApi.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(DalamudApi.PluginInterface);

        this.TextToSpeechProvider = new TextToSpeechProvider(Configuration);
        Advanced_Combat_Tracker.ActGlobals.oFormActMain.LogFilePath = Configuration.LogFilePath;

        FfxivActPluginWrapper = new FfxivActPluginWrapper(Configuration, DalamudApi.GameData.Language, DalamudApi.Chat, DalamudApi.Framework, DalamudApi.Condition);
        OverlayPlugin = InitOverlayPlugin();

        IpcProviders = new IpcProviders(DalamudApi.PluginInterface);
        ConfigWindow = new ConfigWindow(this);
        MainWindow = new MainWindow(this);
        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);
        DalamudApi.Framework.Update += Updata;
        DalamudApi.ClientState.Login += OnOnLogin;
       onUpdateInputUIHook = DalamudApi.Hook.HookFromAddress<OnUpdateInputUI>(
                    DalamudApi.SigScanner.ScanText("4C 8B DC 56 41 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 48 83 B9 ?? ?? ?? ?? ?? 4C 8B FA"), OnUpdateInputUIDo);
        onUpdateInputUIHook.Enable();


        this.NextClick = DateTime.Now;
        DalamudApi.CommandManager.AddHandler(MainWindowCommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Displays the IINACT main window"
        });
        //DalamudApi.Commands.ProcessCommand(MainWindowCommandName);
        DalamudApi.CommandManager.AddHandler(EndEncCommandName, new CommandInfo(EndEncounter)
        {
            HelpMessage = "Ends the current encounter IINACT is parsing"
        });
        DalamudApi.CommandManager.AddHandler(ChatCommandName, new CommandInfo(ChatDo)
        {
            HelpMessage = "chat"
        });
        DalamudApi.PluginInterface.UiBuilder.Draw += DrawUI;
        DalamudApi.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        FormActMain.delta0 = DalamudApi.SigScanner.GetStaticAddressFromSig("89 3D ?? ?? ?? ?? 84 DB");
        FormActMain.delta4 = DalamudApi.SigScanner.GetStaticAddressFromSig("44 8B 1D ?? ?? ?? ?? 43 8D 0C 10");
        FormActMain.deltaC = DalamudApi.SigScanner.GetStaticAddressFromSig("42 03 8C 93 ?? ?? ?? ??");
    }

    private void OnOnLogin()
    {
       setZone();
    }

    private FFXIV_ACT_Plugin.FFXIV_ACT_Plugin GetPluginData()
    {
        return ActGlobals.oFormActMain.FfxivPlugin;
    }
    private int partyLength = 0;



    private void OnUpdateInputUIDo(IntPtr EventArgument, IntPtr parm1)
    {
        onUpdateInputUIHook.Original(EventArgument, parm1);
        var now = DateTime.Now;

        if (this.NextClick < now && ChatQueue.Count > 0)
        {

            var com = ChatQueue.Dequeue();
            ChatHelper.SendMessage(com);
            this.NextClick = now.AddSeconds(1/4);
        }
    }

    private void Updata(IFramework framework)
    {

    }
    private ConfigWindow ConfigWindow { get; }
    public void Dispose()
    {
        DalamudApi.ClientState.EnterPvP -= EnterPvP;
        DalamudApi.ClientState.LeavePvP -= LeavePvP;
        IpcProviders.Dispose();
        GameServerTime.Dispose();
        
        FfxivActPluginWrapper.Dispose();
        OverlayPlugin.DeInitPlugin();
        Trace.Listeners.Remove(PluginLogTraceListener);
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();
        DalamudApi.Framework.Update -= Updata;
        DalamudApi.ClientState.Login -= OnOnLogin;
        DalamudApi.CommandManager.RemoveHandler(MainWindowCommandName);
        DalamudApi.CommandManager.RemoveHandler(EndEncCommandName);
        DalamudApi.CommandManager.RemoveHandler(ChatCommandName);
        onUpdateInputUIHook.Disable();
        cactboSelf.DeInitPlugin();
        //post.DeInitPlugin();
    }
    public  static CactbotSelf.CactbotSelf cactboSelf;
    public static PostNamazu.PostNamazu post;
    private RainbowMage.OverlayPlugin.PluginMain InitOverlayPlugin()
    {
        var container = new RainbowMage.OverlayPlugin.TinyIoCContainer();
        
        var logger = new RainbowMage.OverlayPlugin.Logger(DalamudApi.PluginLog);
        container.Register(logger);
        container.Register<RainbowMage.OverlayPlugin.ILogger>(logger);

        container.Register(HttpClient);
        container.Register(FileDialogManager);
        container.Register(DalamudApi.PluginInterface);
        container.Register(DalamudApi.Framework);

        var overlayPlugin = new RainbowMage.OverlayPlugin.PluginMain(
            DalamudApi.PluginInterface.AssemblyLocation.Directory!.FullName, logger, container);
        container.Register(overlayPlugin);
        Advanced_Combat_Tracker.ActGlobals.oFormActMain.OverlayPluginContainer = container;
        
        Task.Run(() =>
        {
            overlayPlugin.InitPlugin(DalamudApi.PluginInterface.ConfigDirectory.FullName);

            var registry = container.Resolve<RainbowMage.OverlayPlugin.Registry>();
            MainWindow.OverlayPresets = registry.OverlayTemplates;
            WebSocketServer = container.Resolve<RainbowMage.OverlayPlugin.WebSocket.ServerController>();
            MainWindow.Server = WebSocketServer;
            IpcProviders.Server = WebSocketServer;
            IpcProviders.OverlayIpcHandler = container.Resolve<RainbowMage.OverlayPlugin.Handlers.Ipc.IpcHandlerController>();
            ConfigWindow.OverlayPluginConfig = container.Resolve<RainbowMage.OverlayPlugin.IPluginConfig>();
            post = new PostNamazu.PostNamazu(DalamudApi.CommandManager);
            post.InitPlugin();
            cactboSelf = new CactbotSelf.CactbotSelf(Configuration.shunxu, true);
            cactboSelf.InitPlugin();
            DalamudApi.LogInfo("初始化鲶鱼精");
            setZone();

        });

        return overlayPlugin;
    }
    private void setZone()
    {
        var terr = DalamudApi.GameData.GetExcelSheet<Lumina.Excel.Sheets.TerritoryType>();
        var zoneID = DalamudApi.ClientState.TerritoryType;
        var zoneName = terr?.GetRowOrDefault(zoneID)?.PlaceName.Value.Name.ToDalamudString().ToString();
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
    private ILogOutput _logOutput;
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal bool WriteLogLineImpl(int ID, string line)
    {

        var timestamp = DateTime.Now;
        _logOutput?.WriteLine((FFXIV_ACT_Plugin.Logfile.LogMessageType)ID, timestamp, line);
        return true;
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

    private static void EndEncounter(string command, string args)
    {
        Advanced_Combat_Tracker.ActGlobals.oFormActMain.EndCombat(false);
    }
    private void ChatDo(string command, string arguments)
    {
        string[] array = arguments.Split(new char[]
    {
                    ' '
    });
        if (array.Length >=2)
        {
            NextClick = DateTime.Now.AddSeconds(1/6);
            ChatQueue.Enqueue(arguments);
            ChatHelper.SendMessage(arguments);
        }

    }
    private void DrawUI()
    {
        WindowSystem.Draw();
        FileDialogManager.Draw();
    }

    public void DrawConfigUI()
    {
        ConfigWindow.IsOpen = true;

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
