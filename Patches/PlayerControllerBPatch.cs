using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LC_QOLTweaks.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        private const float Offset = 1.5f;
        private static bool _firstPerson = true;
        private static bool _spectatingDebounce;
        private static bool _infSprintState = true;
        private static bool _infSprintDebounce;

        [HarmonyPatch(methodName: "LateUpdate")]
        [HarmonyPostfix]
        private static void ToggleSpectatorCamera(PlayerControllerB __instance)
        {
            if (Keyboard.current[Config.Binds.ToggleSpectateCamera.Value].wasPressedThisFrame && !_spectatingDebounce)
            {
                _firstPerson = !_firstPerson;
                _spectatingDebounce = true;
            }

            if (Keyboard.current[Config.Binds.ToggleSpectateCamera.Value].wasReleasedThisFrame) _spectatingDebounce = false;

            if (__instance.spectatedPlayerScript == null || !_firstPerson) return;

            Transform spectatingPivot = __instance.spectateCameraPivot.transform;
            Transform spectatingVisor = __instance.spectatedPlayerScript.visorCamera.transform;

            spectatingPivot.position = spectatingVisor.position + spectatingVisor.forward.normalized * Offset;
            spectatingPivot.rotation = spectatingVisor.rotation;

            PluginBase.GetManualLogSource().LogMessage(data: $"Changed spectate camera view to {(_firstPerson ? "First Person." : "Default.")}");
        }

        [HarmonyPatch(methodName: "Update")]
        [HarmonyPostfix]
        private static void ToggleInfSprint(ref float ___sprintMeter)
        {
            StartOfRound instance = StartOfRound.Instance;
            if (Keyboard.current[Config.Binds.ToggleInfSprint.Value].wasPressedThisFrame && !_infSprintDebounce && !instance.localPlayerController.inTerminalMenu)
            {
                _infSprintState = !_infSprintState;
                _infSprintDebounce = true;

                HUDManager.Instance.DisplayTip
                (
                    headerText: "Infinite Sprint",
                    bodyText: $"Toggle set to: {(_infSprintState ? "ON" : "OFF")}"
                );
            }

            if (Keyboard.current[Config.Binds.ToggleInfSprint.Value].wasReleasedThisFrame)
                _infSprintDebounce = false;

            if (_infSprintState) ___sprintMeter = 1f;
        }
    }
}                    
