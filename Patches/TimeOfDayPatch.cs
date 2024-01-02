using HarmonyLib;
using UnityEngine.InputSystem;

namespace LC_QOLTweaks.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatch
    {
        private static bool _visible = true;

        [HarmonyPatch(methodName: "SetInsideLightingDimness")]
        [HarmonyPostfix]
        private static void SetClockVisible()
        {
            if (Keyboard.current[Config.Binds.ToggleClockVisibility.Value].wasPressedThisFrame) _visible = !_visible;

            switch (TimeOfDay.Instance.insideLighting)
            {
                case true when _visible:
                    HUDManager.Instance.SetClockVisible(visible: true);
                    break;
                case true when !_visible:
                    HUDManager.Instance.SetClockVisible(visible: false);
                    break;
            }
        }
    }
}
