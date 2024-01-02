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
        private static bool _debounced;

        [HarmonyPatch(methodName: "LateUpdate")]
        [HarmonyPostfix]
        private static void ToggleSpectatorCamera(PlayerControllerB __instance)
        {
            if (Keyboard.current.vKey.wasPressedThisFrame && !_debounced)
            {
                _firstPerson = !_firstPerson;
                _debounced = true;
            }

            if (Keyboard.current.vKey.wasReleasedThisFrame) _debounced = false;

            if (__instance.spectatedPlayerScript == null || !_firstPerson) return;

            Transform spectatingPivot = __instance.spectateCameraPivot.transform;
            Transform spectatingVisor = __instance.spectatedPlayerScript.visorCamera.transform;

            spectatingPivot.position = spectatingVisor.position + spectatingVisor.forward.normalized * Offset;
            spectatingPivot.rotation = spectatingVisor.rotation;

            PluginBase.GetManualLogSource().LogMessage(data: $"Changed spectate camera view to {(_firstPerson ? "First Person." : "Default.")}");
        }
    }
}