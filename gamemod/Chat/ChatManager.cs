namespace Chat
{
    public class ChatManager
    {
        public void ChatMod(string text)
        {
			switch (text)
			{
				case "gsm":
					dctt = !dctt;
					isAutoPlay2 = dctt;
					canAutoPlay2 = dctt;

					if (dctt && !isAutoDichChuyenRunning)  // Chỉ tạo thread nếu chưa có thread nào chạy
					{
						isAutoDichChuyenRunning = true;
						new Thread(AutoDichChuyen).Start();
						GameScr.info1.addInfo("BUINHIKHANG gsm: Bật", 0);
					}
					else if (!dctt && isAutoDichChuyenRunning)  // Nếu tắt auto thì dừng thread
					{
						isAutoDichChuyenRunning = false;
						GameScr.info1.addInfo("BUINHIKHANG gsm: Tắt", 0);
					}
					else
					{
						GameScr.info1.addInfo("BUINHIKHANG gsm: Thread đang chạy, không cần khởi động lại.", 0);
					}
					break;
				case "alogin":
					alogin = !alogin;
					GameScr.info1.addInfo("BUINHIKHANG auto login: " + (alogin ? "Bật" : "Tắt"), 0);
					break;
				case "g":
					GameScr.info1.addInfo("BUINHIKHANG Đã lưu id người chơi: " + Char.myCharz().charFocus, 0);
					savecharid(Char.myCharz().charID);
					break;
				default:
					Service.gI().chat(text);
					break;
			}
        }
    }
}