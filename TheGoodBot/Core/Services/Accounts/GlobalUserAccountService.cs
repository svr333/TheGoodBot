using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Services.Accounts
{
    public class GlobalUserAccountService
    {
        public GlobalUserAccount GetOrCreateGlobalUserAccount(ulong userId)
        {
            CreateUserAccount(userId);
            var globalUser = GetUserAccount(userId);
            return globalUser;
        }

        public void CreateUserAccount(ulong userId)
        {
            string filePath = $"GlobalUserAccounts/{userId}.json";
            string directory = $"GlobalUserAccounts";
            if (File.Exists(filePath)) { return; }

            Directory.CreateDirectory(directory);
            var rawData = JsonConvert.SerializeObject(GenerateGlobalUserAccount(), Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public void SaveUserAccount(GlobalUserAccount user, ulong userId)
        {
            string filePath = $"GlobalUserAccounts/{userId}.json";
            string rawData = JsonConvert.SerializeObject(user, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public GlobalUserAccount GetUserAccount(ulong userId)
        {
            string filePath = $"GlobalUserAccounts/{userId}.json";
            var rawData = File.ReadAllText(filePath);
            var globalUser = JsonConvert.DeserializeObject<GlobalUserAccount>(rawData);
            return globalUser;
        }

        private GlobalUserAccount GenerateGlobalUserAccount() => new GlobalUserAccount()
        {
            AutoPrivateProfile = false,
            Language = "",
            Colour = 5198940
        };
    }
}