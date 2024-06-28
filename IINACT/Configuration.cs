using Dalamud.Configuration;
using Dalamud.Plugin;
using Newtonsoft.Json;

namespace IINACT;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public string DefaultLogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "IINACT");
    private string? logFilePath;

    [JsonIgnore]
    private DalamudPluginInterface? PluginInterface { get; set; }

    public int ParseFilterMode { get; set; }

    public bool DisableDamageShield { get; set; }

    public bool DisableCombinePets { get; set; }

    public bool DisablePvp { get; set; }
    
    public bool SimulateIndividualDoTCrits { get; set; }

    public bool ShowRealDoTTicks { get; set; }

    public bool ShowDebug { get; set; }

    public string LogFilePath
    {
        get => Directory.Exists(logFilePath) ? logFilePath : DefaultLogFilePath;
        set => logFilePath = value;
    }

    public bool WriteLogFile
    {
        get => Advanced_Combat_Tracker.ActGlobals.oFormActMain.WriteLogFile;
        set => Advanced_Combat_Tracker.ActGlobals.oFormActMain.WriteLogFile = value;
    }

    public bool DisableWritingPvpLogFile
    {
        get => Advanced_Combat_Tracker.ActGlobals.oFormActMain.DisableWritingPvpLogFile;
        set => Advanced_Combat_Tracker.ActGlobals.oFormActMain.DisableWritingPvpLogFile = value;
    }

    public int Version { get; set; } = 1;
    
    public string? SelectedOverlay { get; set; }

    public void Initialize(DalamudPluginInterface pluginInterface)
    {
        PluginInterface = pluginInterface;
    }

    public void Save()
    {
        PluginInterface?.SavePluginConfig(this);
    }
}
