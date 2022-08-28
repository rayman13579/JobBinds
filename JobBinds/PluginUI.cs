using System.Numerics;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;
using Framework = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework;

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

    public unsafe void DrawMainWindow()
    {
        if (!Visible)
        {
            return;
        }

        ImGui.SetNextWindowSize(new Vector2(300, 300), ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowSizeConstraints(new Vector2(300, 300), new Vector2(float.MaxValue, float.MaxValue));
        if (ImGui.Begin("JobBinds", ref visible))
        {
            ImGui.Text($"Toggle {(configuration.Toggle ? "on" : "off")}");

            if (ImGui.Button("Open Settings"))
            {
                ConfigVisible = true;
            }

            try
            {
                if (ImGui.Button("Set Keybind"))
                {
                    KeybindModule.KeybindHook.Original(1546165053088, 0, 1, 332, 1);
                }
                
                AgentInterface* configKeyAgent = Framework.Instance()->GetUiModule()->GetAgentModule()->GetAgentByInternalId(AgentId.Configkey);

                if (ImGui.Button("Toggle Keybinds"))
                {
                    configKeyAgent->Show();
                }
            }
            catch (Exception e)
            {
                PluginLog.Error("rip", e);
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