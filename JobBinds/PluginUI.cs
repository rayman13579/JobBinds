using System.Numerics;
using Dalamud.Game.ClientState.Party;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Common.Configuration;
using ImGuiNET;

namespace JobBinds;

public class PluginUI : IDisposable
{
    private Configuration configuration;

    private bool visible = false;

    public bool Visible
    {
        get => visible;
        set => visible = value;
    }

    private bool configVisible = false;

    public bool ConfigVisible
    {
        get => configVisible;
        set => configVisible = value;
    }

    public PluginUI(Configuration configuration)
    {
        this.configuration = configuration;
    }

    public void Dispose()
    {
    }

    public void Draw()
    {
        DrawMainWindow();
        DrawConfigWindow();
    }

    public void DrawMainWindow()
    {
        if (!Visible)
        {
            return;
        }

        ImGui.SetNextWindowSize(new Vector2(300, 300), ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowSizeConstraints(new Vector2(300, 300), new Vector2(float.MaxValue, float.MaxValue));
        if (ImGui.Begin("JobBinds", ref visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
        {
            ImGui.Text($"Toggle {(configuration.Toggle ? "on" : "off")}");

            if (ImGui.Button("Open Settings"))
            {
                ConfigVisible = true;
            }
        }
        ImGui.End();
    }

    public void DrawConfigWindow()
    {
        if (!ConfigVisible)
        {
            return;
        }

        ImGui.SetNextWindowSize(new Vector2(300, 100), ImGuiCond.Always);
        if (ImGui.Begin("JobBinds Settings", ref configVisible, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
        {
            var configValue = configuration.Toggle;
            if (ImGui.Checkbox("Toggle On", ref configValue))
            {
                configuration.Toggle = configValue;
                configuration.Save();
            }

            if (ImGui.Button("Open Plugin"))
            {
                Visible = true;
            }
        }
        ImGui.End();
    }
}