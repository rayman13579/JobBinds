using Dalamud.Configuration;
using Dalamud.Plugin;

namespace JobBinds;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool Toggle { get; set; } = false;

    [NonSerialized] private DalamudPluginInterface? pluginInterface;

    public void Initialize(DalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;
    }

    public void Save()
    {
        pluginInterface!.SavePluginConfig(this);
    }
}