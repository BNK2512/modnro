using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Helper
{
    public static class CommonExtendsion
    {
        public static bool dctt = false;
        public static bool isAutoDichChuyenRunning = false;
        public static bool alogin = true;
        public static bool isAutoDCTTStarted = false;
        const string path = "data.txt";
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
            AppendNumbers(path, result);
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

        public static void AutoDichChuyen()
        {
            int[] charID = new int[] { 0 };
            if (!File.Exists(path)) return;
            if (Char.myCharz().charFocus == null)
            {

                if (new FileInfo(path).Length > 0)
                {
                    charID = ReadNumbers(path);
                }
            }

            // Auto dịch chuyển
            int i = 0;
            while (dctt)
            {
                if (i >= charID.Length)
                    break;

                int id = charID[i];
                if (GameScr.findCharInMap(id) == null)
                {
                    DichChuyen(id);
                }


                if (!dctt)
                    break;

                Thread.Sleep(5000);
            }

            isAutoDichChuyenRunning = false;
        }

        public static void StartAllAuto(GameScr __instance, AccessTools.FieldRef<GameScr, bool> isAutoPlay2Ref)
        {
            if (!isAutoDichChuyenRunning)
            {
                dctt = true;
                //isAutoPlay2 = true;
                isAutoPlay2Ref(__instance) = true;
                isAutoDichChuyenRunning = true;
                new Thread(AutoDichChuyen).Start();
            }

            // Thêm auto khác nếu có...
        }
    }
}
