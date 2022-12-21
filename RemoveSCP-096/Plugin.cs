using HarmonyLib;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace RemoveSCP_096
{
    internal sealed class Plugin
    {
        [PluginPriority(LoadPriority.Medium)]
        [PluginEntryPoint("Remove SCP-096", "1.0.0", "Remove SCP-096 from spawn queue", "Xname")]
        private void Load()
        {
            _harmony.PatchAll();
        }

        [PluginUnload]
        private void Unload()
        {
            _harmony.UnpatchAll();
        }

        private static readonly Harmony _harmony = new("com.rscp096.patch");
    }
}
