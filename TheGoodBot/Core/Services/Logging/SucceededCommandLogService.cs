﻿using System;
using System.IO;
using System.Text;

namespace TheGoodBot.Core.Services.Logging
{
    public class SucceededCommandLogService
    {
        private string folder = $"Logs";
        private string file = $"{DateTime.UtcNow.Year}-{DateTime.UtcNow.Month}-{DateTime.UtcNow.Day}.txt";

        private void SaveLog(StringBuilder content, ulong guildID)
        {
            File.WriteAllText($"{folder}/{guildID}/SucceededCommands/{file}", content.ToString());
        }

        private string GetLog(ulong guildID)
        {
            CheckFileExists(guildID);
            var text = File.ReadAllText($"{folder}/{guildID}/SucceededCommands/{file}");
            return text;
        }

        public void UpdateLog(string message, ulong guildID)
        {
            var text = GetLog(guildID);
            var sb = new StringBuilder(text);
            sb.Append(message);
            SaveLog(sb, guildID);
        }

        private void CheckFileExists(ulong guildID)
        {
            string filePath = $"{folder}/{guildID}/SucceededCommands/{file}";
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory($"{folder}/{guildID}/SucceededCommands");
                File.Create(filePath);
            }
        }
    }
}