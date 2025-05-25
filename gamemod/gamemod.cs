using System;
using HarmonyLib;
using Helper;

namespace GameMod
{
    class GameMod
    {
        static void Main(string[] args)
        {
            Harmony harmony = new Harmony("BUINHIKHANG_MOD");
            harmony.PatchAll();
        }
    }
}