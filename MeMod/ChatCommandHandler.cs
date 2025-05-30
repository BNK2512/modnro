using HarmonyLib;
using Helpers;
using MeMod;
using System;
using System.Collections.Generic;
using System.Threading;

[HarmonyPatch(typeof(GameScr), "onChatFromMe")]
public class Patch_onChatFromMe
{
    // Sử dụng Dictionary để map lệnh chat với hàm xử lý tương ứng cho gọn.
    private static readonly Dictionary<string, Action> chatMap = new Dictionary<string, Action>
    {
        ["gsm"] = () =>
        {
            Gl.dctt = !Gl.dctt;
            Gl.ak = Gl.dctt;
            Gl.canak = Gl.dctt;
            //Funcs.Log();
            if (Gl.dctt && !Gl.isAutoDichChuyenRunning)
            {
                Gl.isAutoDichChuyenRunning = true;
                new Thread(AutoHandler.AutoDichChuyen).Start();
                GameScr.info1.addInfo("Giảm sức mạnh: Bật", 0);
            }
            else if (!Gl.dctt && Gl.isAutoDichChuyenRunning)
            {
                Gl.isAutoDichChuyenRunning = false;
                GameScr.info1.addInfo("Giảm sức mạnh: Tắt", 0);
            }
            else
            {
                GameScr.info1.addInfo("Giảm sức mạnh: Thread đang chạy, không cần khởi động lại.", 0);
            }
        },
        ["alogin"] = () =>
        {
            Gl.alogin = !Gl.alogin;
            GameScr.info1.addInfo("auto đăng nhập: " + (Gl.alogin ? "Bật" : "Tắt"), 0);
        },
        ["ak"] = () =>
        {
            Gl.ak = !Gl.ak;
            GameScr.info1.addInfo("auto đánh: " + (Gl.ak ? "Bật" : "Tắt"), 0);
        },
        ["g"] = () =>
        {
            GameScr.info1.addInfo("Đã lưu id người chơi: " + Char.myCharz().charFocus?.cName ?? "Unknown", 0);
            AutoHandler.savecharid(Char.myCharz().charFocus.charID);
        },
        ["bg"] = () =>
        {
            Gl.bg = !Gl.bg;
            GameScr.info1.addInfo("BackGround " + (Gl.bg ? "Tắt" : "Bật"), 0);
            GameCanvas.lowGraphic = Gl.bg;
        },
    };

    public static bool Prefix(string text, string to)
    {
        // Kiểm tra xem text có nằm trong danh sách lệnh cho phép không
        if (Funcs.IsCharInArray(Gl.allowedChat, text) && chatMap.TryGetValue(text, out var action))
        {
            action.Invoke();
            // Trả về false để chặn hàm gốc không chạy nữa
            return false;
        }
        // Nếu không phải lệnh đặc biệt thì cho hàm gốc chạy bình thường
        return true;
    }
}
