using System;
using System.IO;
using System.Text;

namespace TheGoodBot.Core.Services.Logging
{
    public class FileLoggerService
    {
        private const string folder = "Logs";
        private string file = $"{DateTime.UtcNow.Year}-{DateTime.UtcNow.Month}-{DateTime.UtcNow.Day}.txt";

        private void SaveLog(StringBuilder sb)
        {
            File.WriteAllText(String.Concat(folder, "/", file), sb.ToString());
        }

        private string GetLog()
        {
            CheckFileExists();
            var text = File.ReadAllText(String.Concat(folder, "/", file));
            return text;
        }

        public void UpdateLog(string message)
        {
            var text = GetLog();
            var sb = new StringBuilder(text);
            sb.Append(message);
            SaveLog(sb);
        }

        private void CheckFileExists()
        {
            string filePath = String.Concat(folder, "/", file);
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(folder);
                File.Create(filePath);
            }
        }
    }
}