using System;
using System.IO;
using System.Text;

namespace TheGoodBot.Core.Services.Logging
{
    public class LoggerService
    {
        public void LogFailedCommand(string message, ulong guildID)
        {
            UpdateLog(message, guildID, "FailedCommands");
        }

        public void LogSucceededCommand(string message, ulong guildID)
        {
            UpdateLog(message, guildID, "SucceededCommands");
        }

        public void LogGuildAccountChanges()
        {

        }

        public void LogGuildUserAccountChanges()
        {

        }

        private string file = $"{DateTime.UtcNow.Year}-{DateTime.UtcNow.Month}-{DateTime.UtcNow.Day}.txt";
        private string folderPath;
        private string filePath;

        private void SetFilePath(ulong guildID, string subfolder)
        {
            filePath = $"GuildAccounts/{guildID}/Logs/{subfolder}/{file}";
            folderPath = $"GuildAccounts/{guildID}/Logs/{subfolder}";
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

        public void UpdateLog(string message, ulong guildId, string logType)
        {
            SetFilePath(guildId, logType);
            var sb = new StringBuilder(GetLog());
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
