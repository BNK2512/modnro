using HarmonyLib;

[HarmonyPatch(typeof(GameScr), "onChatFromMe")]
public class Patch_onChatFromMe
{
    public static void Prefix(string text, string to)
    {
        // Gọi trước khi vào hàm gốc
        GameScr.info1.addInfo($"[Prefix] Bạn gõ: {text} - Đến: {to}", 0);
    }

    public static void Postfix(string text, string to)
    {
        // Gọi sau khi hàm gốc chạy xong
        GameScr.info1.addInfo($"[Postfix] Đã gửi xong: {text}", 0);
    }
}
