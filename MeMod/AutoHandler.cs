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
        Funcs.Log("dich chuyehnnnn");
        Item[] arrItembody = Char.myCharz().arrItemBag;
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
            charID = ReadNumbers(Gl.path);
        }


        // Auto dịch chuyển
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

    public static int[] ReadNumbers(string filePath)
    {
        if (!File.Exists(filePath)) return new int[0];

        string content = File.ReadAllText(filePath);
        return content
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
    }

    public static void savecharid(int id)
    {
        string[] result = new string[] { id.ToString() };
        AppendNumbers(Gl.path, result);
    }

    public static void AppendNumbers(string filePath, string[] newNumbers)
    {
        string newData = string.Join(",", newNumbers);

        if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
        {
            File.WriteAllText(filePath, newData);
        }
        else
        {
            File.AppendAllText(filePath, "," + newData);
        }
    }
    public static void StartAllAuto(GameScr __instance, AccessTools.FieldRef<GameScr, bool> isAutoPlay2Ref)
    {
        if (!Gl.isAutoDichChuyenRunning)
        {
            Gl.dctt = true;
            //isAutoPlay2 = true;
            isAutoPlay2Ref(__instance) = true;
            Gl.isAutoDichChuyenRunning = true;
            new Thread(AutoDichChuyen).Start();
        }

        // Thêm auto khác nếu có...
    }
}

