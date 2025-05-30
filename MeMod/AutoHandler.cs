using System;
using System.IO;
using System.Linq;
using System.Threading;
using HarmonyLib;
using Helpers;

namespace MeMod;

public class AutoHandler
{
    public static int FindYardrat()
    {
        for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
        {
            var item = Char.myCharz().arrItemBag[i];
            if (item != null && item.template != null && item.template.name != null && item.template.name.Contains("Yardrat"))
            {
                return i;
            }
        }
        return -1;
    }

    public static void DichChuyen(int CharID)
    {
        Item[] arrItembody = global::Char.myCharz().arrItemBag;
        if (arrItembody[5] == null)
        {
            Service.gI().getItem(4, (sbyte)FindYardrat());
            Service.gI().gotoPlayer(CharID);
            Service.gI().getItem(5, 5);
            return;
        }
        if (arrItembody[5].template.name.Contains("Yardrat"))
        {
            Service.gI().gotoPlayer(CharID);
            return;
        }
        if (!arrItembody[5].template.name.Contains("Yardrat"))
        {
            Service.gI().getItem(4, (sbyte)FindYardrat());
            Service.gI().gotoPlayer(CharID);
            Service.gI().getItem(4, (sbyte)FindYardrat());
        }
    }

    public static void AutoDichChuyen()
    {
        int[] charID = new int[] { 0 };

        if (Char.myCharz().charFocus == null && File.Exists(Gl.path) && new FileInfo(Gl.path).Length > 0)
        {
            charID = Funcs.ReadNumbers(Gl.path);
        }

        int i = 0;
        while (Gl.dctt)
        {

            if (i >= charID.Length)
                break;

            int id = charID[i];
            if (GameScr.findCharInMap(id) == null)
            {
                DichChuyen(id);
            }

            if (!Gl.dctt)
                break;

            Thread.Sleep(5000);
        }

        Gl.isAutoDichChuyenRunning = false;
    }

    public static void savecharid(int id)
    {
        string[] result = new string[] { id.ToString() };
        Funcs.AppendNumbers(Gl.path, result);
    }

    public static void StartAllAuto()
    {
        if (!Gl.isAutoDichChuyenRunning)
        {
            Gl.dctt = true;
            Gl.isAutoDichChuyenRunning = true;
            new Thread(AutoDichChuyen).Start();
        }

        // Thêm auto khác nếu có...
    }

    // ---- Hỗ trợ ----

    public static bool hasBeanInBag()
    {
        foreach (Item item in Char.myCharz().arrItemBag)
        {
            if (item != null && item.template.type == 6) return true;
        }
        return false;
    }

    private Mob findClosestMob()
    {
        Mob closest = null;
        int minDist = int.MaxValue;

        for (int i = 0; i < GameScr.vMob.size(); i++)
        {
            Mob m = (Mob)GameScr.vMob.elementAt(i);
            if (m != null && m.hp > 0 && m.status != 0 && m.status != 1 && !m.isMobMe)
            {
                int dist = Math.abs(m.x - Char.myCharz().cx) + Math.abs(m.y - Char.myCharz().cy);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = m;
                }
            }
        }
        return closest;
    }

    private Skill getBestAvailableSkill()
    {
        Skill best = null;
        Skill[] skills = GameCanvas.isTouch ? GameScr.onScreenSkill : GameScr.keySkill;

        foreach (Skill s in skills)
        {
            if (s == null || s.paintCanNotUseSkill || s.template.isSkillSpec() || Char.myCharz().skillInfoPaint() != null)
                continue;

            long mpRequired = (s.template.manaUseType == 2) ? 1 :
                            (s.template.manaUseType == 1) ? (s.manaUse * Char.myCharz().cMPFull / 100) :
                            s.manaUse;

            if (Char.myCharz().cMP >= mpRequired && s.coolDown == 0)
            {
                if (best == null || s.point > best.point) // Ưu tiên mạnh hơn
                {
                    best = s;
                }
            }
        }
        return best;
    }

    public static void moveTo(int x, int y)
    {
        if (Math.abs(Char.myCharz().cx - x) > 10 || Math.abs(Char.myCharz().cy - y) > 10)
        {
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
        }
    }

}

