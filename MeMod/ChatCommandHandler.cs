using HarmonyLib;
using Helpers;
using MeMod;
using System.Threading;

[HarmonyPatch(typeof(GameScr), "onChatFromMe")]
public class Patch_onChatFromMe
{
    public static bool Prefix(string text, string to)
    {
        if (Funcs.IsCharInArray(Gl.allowedChat, text))
        {
            switch (text)
            {
                case "gsm":
                    Gl.dctt = !Gl.dctt;
                    Gl.ak = Gl.dctt;
                    Gl.canak = Gl.dctt;

                    if (Gl.dctt && !Gl.isAutoDichChuyenRunning)  // Chỉ tạo thread nếu chưa có thread nào chạy
                    {
                        Gl.isAutoDichChuyenRunning = true;
                        new Thread(AutoHandler.AutoDichChuyen).Start();
                        GameScr.info1.addInfo("Giảm sức mạnh: Bật", 0);
                    }
                    else if (!Gl.dctt && Gl.isAutoDichChuyenRunning)  // Nếu tắt auto thì dừng thread
                    {
                        Gl.isAutoDichChuyenRunning = false;
                        GameScr.info1.addInfo("Giảm sức mạnh: Tắt", 0);
                    }
                    else
                    {
                        GameScr.info1.addInfo("Giảm sức mạnh: Thread đang chạy, không cần khởi động lại.", 0);
                    }
                    break;
                case "alogin":
                    Gl.alogin = !Gl.alogin;
                    GameScr.info1.addInfo("auto login: " + (Gl.alogin ? "Bật" : "Tắt"), 0);
                    break;
                case "ak":
                    Gl.ak = !Gl.ak;
                    GameScr.info1.addInfo("auto đánh: " + (Gl.ak ? "Bật" : "Tắt"), 0);
                    break;
                case "g":
                    GameScr.info1.addInfo("Đã lưu id người chơi: " + Char.myCharz().charFocus, 0);
                    AutoHandler.savecharid(Char.myCharz().charID);
                    break;
                default:

                    break;
            }
            return false;
        }
        return true;
    }

    // public static void Postfix(string text, string to)
    // {
    //     // Gọi sau khi hàm gốc chạy xong
    //     GameScr.info1.addInfo($"[Postfix] Đã gửi xong: {text}", 0);
    // }
}
