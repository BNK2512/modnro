using HarmonyLib;
using Helper;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Patch
{
    [HarmonyPatch(typeof(GameScr), "onChatFromMe")]
    public class ChatPatch
    {
        static AccessTools.FieldRef<GameScr, bool> isAutoPlay2Ref = AccessTools.FieldRefAccess<GameScr, bool>("isAutoPlay2");
        static AccessTools.FieldRef<GameScr, bool> canAutoPlay2Ref = AccessTools.FieldRefAccess<GameScr, bool>("canAutoPlay2");

        static void Prefix(string __text, string __to, GameScr __instance)
        {
            if (!string.IsNullOrEmpty(__text))
            {
                switch (__text)
                {
                    case "gsm":
                        CommonExtendsion.dctt = !CommonExtendsion.dctt;
                        //isAutoPlay2 = dctt;
                        isAutoPlay2Ref(__instance) = CommonExtendsion.dctt;
                        //canAutoPlay2 = dctt;
                        canAutoPlay2Ref(__instance) = CommonExtendsion.dctt;

                        if (CommonExtendsion.dctt && !CommonExtendsion.isAutoDichChuyenRunning)  // Chỉ tạo thread nếu chưa có thread nào chạy
                        {
                            CommonExtendsion.isAutoDichChuyenRunning = true;
                            new Thread(CommonExtendsion.AutoDichChuyen).Start();
                            GameScr.info1.addInfo("BUINHIKHANG gsm: Bật", 0);
                        }
                        else if (!CommonExtendsion.dctt && CommonExtendsion.isAutoDichChuyenRunning)  // Nếu tắt auto thì dừng thread
                        {
                            CommonExtendsion.isAutoDichChuyenRunning = false;
                            GameScr.info1.addInfo("BUINHIKHANG gsm: Tắt", 0);
                        }
                        else
                        {
                            GameScr.info1.addInfo("BUINHIKHANG gsm: Thread đang chạy, không cần khởi động lại.", 0);
                        }
                        break;
                    case "alogin":
                        CommonExtendsion.alogin = !CommonExtendsion.alogin;
                        GameScr.info1.addInfo("BUINHIKHANG auto login: " + (CommonExtendsion.alogin ? "Bật" : "Tắt"), 0);
                        break;
                    case "g":
                        GameScr.info1.addInfo("BUINHIKHANG Đã lưu id người chơi: " + Char.myCharz().charFocus, 0);
                        CommonExtendsion.savecharid(Char.myCharz().charID);
                        break;
                    default:
                        Service.gI().chat(__text);
                        break;
                }
            }
            else __instance.onChatFromMe(__text, __to);
        }
    }
}