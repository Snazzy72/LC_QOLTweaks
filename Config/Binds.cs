using System.IO;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine.InputSystem;

namespace LC_QOLTweaks.Config
{
    internal static class Binds
    {
        public static ConfigFile ConfigFile = new(Path.Combine(Paths.ConfigPath, "QOLTweaks.cfg"), saveOnInit: true);

        public static ConfigEntry<Key> ToggleSpectateCamera = ConfigFile.Bind
        (new ConfigDefinition(section: "Keybindings", key: "ToggleSpectateCamera"), defaultValue: Key.V,
            configDescription: new ConfigDescription("Toggle from the default camera view to first person camera view"));

        public static ConfigEntry<Key> ToggleClockVisibility = ConfigFile.Bind
        (new ConfigDefinition(section: "Keybindings", key: "ToggleClockVisibility"), defaultValue: Key.X,
            configDescription: new ConfigDescription("Toggle the clock visibility"));
    }
}
