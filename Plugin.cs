using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using LC_QOLTweaks.Patches;
using LC_QOLTweaks.Config;

namespace LC_QOLTweaks
{
    public abstract class Plugin
    {
        public const string Guid = "com.snazzy.LC_QOLTweaks";
        public const string Name = "LC_QOLTweaks";
        public const string Version = "0.0.1";
    }

    [BepInPlugin(Plugin.Guid, Plugin.Name, Plugin.Version)]
    public class PluginBase : BaseUnityPlugin
    {
        private static PluginBase _instance = null!;
        private readonly Harmony _harmonyHook = new(id: Plugin.Guid);
        private static ManualLogSource ManualLogSource { get; set; }

        private void Awake()
        {
            InitSingleton();
            InitStartup();
            InitHarmonyHooks();
        }

        private void InitSingleton()
        {
            InitLogSource();

            if (_instance == null)
            {
                _instance = this;
                ManualLogSource.LogMessage(data: "Instance set.");
            }
            else
            {
                ManualLogSource.LogWarning(data: "Duplicate instance of PluginBase detected!");
            }
        }

        private void InitHarmonyHooks()
        {
            try
            {
                _harmonyHook.PatchAll(typeof(PluginBase));
                _harmonyHook.PatchAll(typeof(RoundManagerPatch));
                _harmonyHook.PatchAll(typeof(TimeOfDayPatch));
                _harmonyHook.PatchAll(typeof(PlayerControllerBPatch));

                ManualLogSource.LogMessage(data: "Patching with Harmony complete.");
            }
            catch (Exception exception)
            {
                ManualLogSource.LogFatal(data: $"Failed to apply patches with Harmony: {exception}");
            }
        }

        private static void InitLogSource()
        {
            ManualLogSource = BepInEx.Logging.Logger.CreateLogSource(Plugin.Guid);
        }

        private static void InitStartup()
        {
            ManualLogSource.LogMessage(data: "Loaded LC_QOLTweaks.");

            try
            {
                ManualLogSource.LogMessage(data: $"ToggleSpectateCamera keybinding set to: {Binds.ToggleSpectateCamera.Value}");
                ManualLogSource.LogMessage(data: $"ToggleClockVisibility keybinding set to: {Binds.ToggleClockVisibility.Value}");
                ManualLogSource.LogMessage(data: $"ToggleInfSprint keybinding set to: {Binds.ToggleInfSprint.Value}");
            }
            catch (Exception exception)
            {
                ManualLogSource.LogError(data: $"Keybindings not created: {exception}");
            }
        }

        public static ManualLogSource GetManualLogSource() => ManualLogSource;
    }
}