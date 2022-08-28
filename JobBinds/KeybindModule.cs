using Dalamud.Game;
using Dalamud.Hooking;
using Dalamud.Logging;

namespace JobBinds;

public class KeybindModule : IDisposable
{
    private SigScanner SigScanner;
    
    public static Hook<KeybindDelegate> KeybindHook;

    public KeybindModule(SigScanner sigScanner)
    {
        SigScanner = sigScanner;
        var ptr = SigScanner.ScanText("E8 ?? ?? ?? ?? 48 89 83 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 48 8B D0");
        KeybindHook = new Hook<KeybindDelegate>(ptr, (dunno, row, column, key, modifier) =>
        {
            try
            {
                string keyCodes = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                List<string> modifiers = new List<string> { "NONE", "SHIFT", "CTRL", "UNUSED", "ALT" };
                PluginLog.Debug($"keybind changed?: {dunno} | {row} | {column} | {key}->{keyCodes[key - 321]} | {modifier}->{modifiers[modifier]}");
            }
            catch (Exception e)
            {
                PluginLog.Error($"Can't find keyCode {key}", e);
            }

            return KeybindHook.Original(dunno, row, column, key, modifier);
            });
        KeybindHook.Enable();
    }

    public void Dispose()
    {
        KeybindHook.Disable();
        KeybindHook.Dispose();
    }
}