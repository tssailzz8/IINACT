using System.Diagnostics;

namespace IINACT;

public class PluginLogTraceListener : TraceListener
{
    public override void Write(string? message) { }

    public override void WriteLine(string? message) { }

    public override void WriteLine(string? message, string? category)
    {
        if (message is null) return;
        
        if (category?.Equals("ffxiv_act_plugin", StringComparison.OrdinalIgnoreCase) ?? false)
           DalamudApi.PluginLog.Information($"[FFXIV_ACT_PLUGIN] {message}");

        if (category?.Equals("machina", StringComparison.OrdinalIgnoreCase) ?? false)
            DalamudApi.PluginLog.Information($"[MACHINA] {message}");
        
        if (category?.Equals("debug-machina", StringComparison.OrdinalIgnoreCase) ?? false)
            DalamudApi.PluginLog.Debug($"[MACHINA] {message}");
    }
}
