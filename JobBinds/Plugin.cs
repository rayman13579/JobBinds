﻿using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Common.Configuration;

namespace JobBinds;

public class Plugin : IDalamudPlugin
{
    public string Name => "JobBinds";

    public string OpenCommand => "/jobBinds";

    private DalamudPluginInterface PluginInterface { get; init; }

    private CommandManager CommandManager { get; init; }

    private Configuration Configuration { get; init; }

    private PluginUI PluginUI { get; init; }

    public Plugin(DalamudPluginInterface pluginInterface, CommandManager commandManager)
    {
        PluginInterface = pluginInterface;
        CommandManager = commandManager;
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);
        PluginUI = new PluginUI(Configuration);

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
    }
}