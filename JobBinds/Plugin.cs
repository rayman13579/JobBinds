using Dalamud.Game;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Common.Configuration;

namespace JobBinds;

public class Plugin : IDalamudPlugin
{
    public string Name => "JobBinds";

    public string OpenCommand => "/jobBinds";

    private DalamudPluginInterface PluginInterface { get; init; }

    private CommandManager CommandManager { get; init; }

    public static SigScanner SigScanner { get; set; }

    private Configuration Configuration { get; init; }

    private PluginUI PluginUI { get; init; }

    private KeybindModule KeybindModule;

    public Plugin(DalamudPluginInterface pluginInterface, CommandManager commandManager, SigScanner sigScanner)
    {
        PluginInterface = pluginInterface;
        CommandManager = commandManager;
        SigScanner = sigScanner;
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);
        PluginUI = new PluginUI(Configuration);

        KeybindModule = new KeybindModule(sigScanner);

        CommandManager.AddHandler(OpenCommand, new CommandInfo(OpenPlugin));

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }

    private void OpenPlugin(string command, string args)
    {
        PluginUI.Visible = true;
    }

    private void DrawUI()
    {
        PluginUI.Draw();
    }

    private void DrawConfigUI()
    {
        PluginUI.ConfigVisible = true;
    }

    public void Dispose()
    {
        PluginUI.Dispose();
        KeybindModule.Dispose();
        CommandManager.RemoveHandler(OpenCommand);
    }
}