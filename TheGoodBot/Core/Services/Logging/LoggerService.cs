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
        private string _folderPath;
        private string _filePath;

        private void SetFilePath(ulong guildId, string subfolder)
        {
            _filePath = $"GuildAccounts/{guildId}/Logs/{subfolder}/{file}";
            _folderPath = $"GuildAccounts/{guildId}/Logs/{subfolder}";
        }

        private void SaveLog(StringBuilder content)
        {
            File.WriteAllText(_filePath, content.ToString());
        }

        private string GetLog()
        {
            CheckFileExists();
            var text = File.ReadAllText(_filePath);
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
            if (File.Exists(_filePath)) { return; }

            Directory.CreateDirectory(_folderPath);
            File.Create(_filePath);
        }
    }
}
