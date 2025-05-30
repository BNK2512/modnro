using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Helpers

{
    public static class Funcs
    {
        public static void Log(string message)
        {
            try
            {
                File.AppendAllText("mod_log.txt", $"[{DateTime.Now}] - {message}\n");
            }
            catch { }
        }

        public static void Debug(string message)
        {
            GameScr.info1.addInfo("[DEBUG] " + message, 0);
        }

        public static void SafeSleep(int milliseconds)
        {
            try
            {
                Thread.Sleep(milliseconds);
            }
            catch (ThreadInterruptedException)
            {
                Log("Sleep bị ngắt!");
            }
        }

        public static bool IsCharInArray(string[] array, string input)
        {
            return array.Contains(input);
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

        public static void AppendNumbers(string filePath, string[] newNumbers)
        {
            string newData = string.Join(",", newNumbers);
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
                File.WriteAllText(filePath, newData);
            else
                File.AppendAllText(filePath, "," + newData);
        }
    }
}
