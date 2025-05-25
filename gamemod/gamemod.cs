using System;
using Chat;
using Auto;
using Update;
using Helper;

namespace GameMod
{
    class GameMod
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GameMod started!");

            ChatManager chat = new ChatManager();
            chat.SendWelcomeMessage();

            AutoController auto = new AutoController();
            auto.ExecuteAutoTask();

            UpdateManager update = new UpdateManager();
            update.CheckUpdates();

            Logger.Log("GameMod initialized successfully.");

            Console.ReadLine();
        }
    }
}