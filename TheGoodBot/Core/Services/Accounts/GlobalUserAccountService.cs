using System;
using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Services.Accounts
{
    public class GlobalUserAccountService
    {
        public GlobalUserAccountStruct GetOrCreateGlobalUserAccount(ulong userId)
        {
            CreateUserAccount(userId);
            var globalUser = GetUserAccount(userId);
            return globalUser;
        }

        public void CreateUserAccount(ulong userId)
        {
            string filePath = "GlobalUserAccounts/" + userId + ".json";
            string directory = "GlobalUserAccounts";
            if (File.Exists(filePath)) { return; }

            Directory.CreateDirectory(directory);
            var rawData = JsonConvert.SerializeObject(GenerateGlobalUserAccount(), Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public void SaveUserAccount(GlobalUserAccountStruct user, ulong userId)
        {
            string filePath = "GlobalUserAccounts/" + userId + ".json";
            string rawData = JsonConvert.SerializeObject(user, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public GlobalUserAccountStruct GetUserAccount(ulong userId)
        {
            string filePath = "GlobalUserAccounts/" + userId + ".json";
            var rawData = File.ReadAllText(filePath);
            var globalUser = JsonConvert.DeserializeObject<GlobalUserAccountStruct>(rawData);
            return globalUser;
        }

        private GlobalUserAccountStruct GenerateGlobalUserAccount() => new GlobalUserAccountStruct()
        {
            AutoPrivateProfile = false,
            Language = "",
        };
    }
}