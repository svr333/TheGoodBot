using System;
using System.IO;
using System.Text;

namespace TheGoodBot.Core.Services.Logging
{
    public class FailedCommandLogService
    {
        private string folder = $"Logs";
        private string file = $"{DateTime.UtcNow.Year}-{DateTime.UtcNow.Month}-{DateTime.UtcNow.Day}.txt";
        private string folderPath;
        private string filePath;

        private void SetFilePath(ulong guildID, string subfolder)
        {
            filePath = $"{folder}/{guildID}/{subfolder}/{file}";
            folderPath = $"{folder}/{guildID}/{subfolder}";
        }

        private void SaveLog(StringBuilder content)
        {
            File.WriteAllText(filePath, content.ToString());
        }

        private string GetLog()
        {
            CheckFileExists();
            var text = File.ReadAllText(filePath);
            return text;
        }

        public void UpdateLog(string message, ulong guildID, string logType)
        {
            SetFilePath(guildID, logType);
            var text = GetLog();
            var sb = new StringBuilder(text);
            sb.Append(message);
            SaveLog(sb);
        }

        private void CheckFileExists()
        {
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(folderPath);
                File.Create(filePath);
            }
        }
    }
}