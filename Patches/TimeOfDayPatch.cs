using HarmonyLib;

namespace LC_QOLTweaks.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatch
    {
        [HarmonyPatch(methodName: "SetInsideLightingDimness")]
        [HarmonyPostfix]
        private static void SetClockVisible()
        {
            if (TimeOfDay.Instance.insideLighting)
            {
                HUDManager.Instance.SetClockVisible(visible: true);
            }
        }
    }
}
