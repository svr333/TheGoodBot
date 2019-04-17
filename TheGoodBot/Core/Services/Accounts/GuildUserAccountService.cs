using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Services.Accounts
{
    public class GuildUserAccountService
    {
        public GuildUserAccount GetOrCreateGuildUserAccount(ulong guildId, ulong userId)
        {
            CreateGuildUserAccount(guildId, userId);
            var guild = GetAccount(guildId, userId);
            return guild;
        }

        public void SaveGuildUserAccount(GuildUserAccount guildUser, ulong guildId, ulong userId)
        {
            var rawData = JsonConvert.SerializeObject(guildUser, Formatting.Indented);
            File.WriteAllText($"GuildUserAccounts/{guildId}/{userId}.json", rawData);
        }

        private void CreateGuildUserAccount(ulong guildId, ulong userId)
        {
            var filePath = $"GuildUserAccounts/{guildId}/{userId}.json";
            var directory = $"GuildUserAccounts/{guildId}";
            if (!FileExists(filePath, directory))
            {
                var rawData = JsonConvert.SerializeObject(GenerateBlankGuildUserConfig(guildId, userId), Formatting.Indented);
                File.WriteAllText(filePath, rawData);
            }
        }

        private static bool FileExists(string filePath, string directory)
        {
            if (File.Exists(filePath)) { return true; }

            Directory.CreateDirectory(directory);
            return false;
        }

        private GuildUserAccount GetAccount(ulong guildId, ulong userId)
        {
            var filePath = $"GuildUserAccounts/{guildId}/{userId}.json";
            var json = File.ReadAllText(filePath);
            var guildUser = JsonConvert.DeserializeObject<GuildUserAccount>(json);
            return guildUser;
        }

        private uint GetJoinPosition(ulong guildId)
            => (uint) Directory.GetFiles($"GuildUserAccounts/{guildId}").Length;

        private GuildUserAccount GenerateBlankGuildUserConfig(ulong guildId, ulong userId) => new GuildUserAccount()
        {
            UserId = userId,
            GuildId = guildId,
            Language = "English",
            Colour = 5198940,
            JoinPosition = GetJoinPosition(guildId),
            Xp = 0,
            Level = 0
        };
    }
}
