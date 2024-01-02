using System;
using HarmonyLib;

namespace LC_QOLTweaks.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        [HarmonyPatch(methodName: "SpawnScrapInLevel")]
        [HarmonyPostfix]
        private static void GetScrapValuesInLevel()
        {
            HUDManager hudInstance = HUDManager.Instance;
            SelectableLevel currentLevel = RoundManager.Instance.currentLevel;

            try
            {
                hudInstance.DisplayTip
                (
                    headerText: "Current scrap in level", 
                    bodyText: $"Minimum of {currentLevel.minScrap} scrap items with a value of {currentLevel.minTotalScrapValue}. " +
                    $"Maximum of {currentLevel.maxScrap} scrap items with a value of {currentLevel.maxTotalScrapValue}"
                );
            }
            catch (Exception exception)
            {
                PluginBase.GetManualLogSource().LogFatal(exception);
            }
        }
    }
}
