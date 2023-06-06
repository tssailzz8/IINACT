using Dalamud.Configuration;
using Dalamud.Plugin;
using Newtonsoft.Json;

namespace IINACT;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public string DefaultLogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "IINACT");
    private string? logFilePath;
    public List<string> shunxu=new();
    public int TTSIndex;
    public bool UseEdeg;

    [JsonIgnore]
    private DalamudPluginInterface? PluginInterface { get; set; }

    public int ParseFilterMode { get; set; }

    public bool DisableDamageShield { get; set; }

    public bool DisableCombinePets { get; set; }

    public bool SimulateIndividualDoTCrits { get; set; }

    public bool ShowRealDoTTicks { get; set; }

    public bool ShowDebug { get; set; }

    public bool EnableOverlay { get; set; } = true;
    public bool EnablePost { get; set; } = true;
    public bool EnableCact { get; set; } = true;

    public string LogFilePath
    {
        get => Directory.Exists(logFilePath) ? logFilePath : DefaultLogFilePath;
        set => logFilePath = value;
    }

    public int Version { get; set; } = 0;

    public void Initialize(DalamudPluginInterface pluginInterface)
    {
        PluginInterface = pluginInterface;
    }

    public void Save()
    {
        PluginInterface?.SavePluginConfig(this);
    }
}
