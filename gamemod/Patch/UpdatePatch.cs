using HarmonyLib;
using Helper;
using System;

namespace Patch
{
    [HarmonyPatch(typeof(GameScr), "update")]
    public class UpdatePatch
    {
        static AccessTools.FieldRef<GameScr, bool> isAutoPlay2Ref = AccessTools.FieldRefAccess<GameScr, bool>("isAutoPlay2");
        static void Prefix(GameScr __instance)
        {
            if (!CommonExtendsion.isAutoDCTTStarted && !Char.isLoadingMap)
            {
                CommonExtendsion.isAutoDCTTStarted = true;
                CommonExtendsion.StartAllAuto(__instance, isAutoPlay2Ref);
            }
            else __instance.update();
        }
    }
}
