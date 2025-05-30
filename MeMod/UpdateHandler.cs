using HarmonyLib;
using Helpers;
using MeMod;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[HarmonyPatch(typeof(GameScr), "update")]
public class Patch_GameScr_update
{
    public static void Prefix()
    {
        if (Gl.isAutoDCTTStarted && !Char.isLoadingMap)
        {
            AutoHandler.StartAllAuto();
        }
        if (Gl.dctt && Char.myCharz().meDead)
        {
            Service.gI().returnTownFromDead();
        }
        // if (Gl.ak && GameCanvas.gameTick % 50 == 0)
		// {
        //     GameScr.autoPlayMod();
		// }
    }
}

[HarmonyPatch(typeof(ServerListScreen), "update")]
public class Patch_ServerListScreen_update
{
    static int time_ = 0;
    public static void Prefix()
    {
        if (Gl.logintime_ > 5)
        {
            GameCanvas.startOK("login quá 5 lần vẫn không đăng nhập được", 8884, null);
            return;
        }
        else
        if (Gl.alogin)
        {
            time_++;
            bool a = time_ == 200;
            if (a)
            {
                try
                {
                    GameCanvas.serverScreen.perform(3, null);
                    time_ = 0;
                }
                catch
                {
                    Gl.logintime_++;
                }
            }
        }
    }
}

[HarmonyPatch(typeof(GameCanvas), "loadBG")]
public class Patch_GameCanvas_update
{
    public static void Prefix()
    {
        GameCanvas.lowGraphic = Gl.bg;
    }
}
