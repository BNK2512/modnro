
using HarmonyLib;

namespace MeMod
{
    public class ModMain
    {
        private static bool isPatched = false;

        public static void Main()
        {
            if (isPatched) return; // tránh patch lặp
            var harmony = new Harmony("mod.test.chatpatch");
            harmony.PatchAll();
            isPatched = true;
        }
    }
}



