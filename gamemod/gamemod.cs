using System;
using HarmonyLib;
using Helper;

//namespace GameMod
//{
public class GameMod
{
    public static void Main()
    {
        Harmony harmony = new Harmony("BUINHIKHANG_MOD");
        harmony.PatchAll();
    }
}
//}