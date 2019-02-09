using System;
using System.Collections.Generic;
using System.IO;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Guilds;
using TheGoodBot.Languages;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Entities
{
    public class LanguageService
    {
        private List<string> _languageList = new List<string>();
        private CreateLanguageFilesService _changeCustomEmbedService;
        private GlobalUserAccountService _globalUserAccountService;
        private GuildAccountService _guildAccountService;
        private GuildUserAccountService _guildUserAccountService;

        public LanguageService(CreateLanguageFilesService changeCustomEmbedService = null, GuildAccountService GuildAccount = null, GlobalUserAccountService GlobalUser = null, GuildUserAccountService GuildUser = null)
        {
            _changeCustomEmbedService = changeCustomEmbedService;
            _guildAccountService = GuildAccount;
            _globalUserAccountService = GlobalUser;
            _guildUserAccountService = GuildUser;
        }

        public void CreateLanguageFiles()
        {
            //if (_languageList == null || !_languageList.Any()) { GenerateNewLanguageList(); }
            GenerateNewLanguageList();

            foreach (var language in _languageList)
            {
                string filePath = "Languages/" + language;
                Directory.CreateDirectory(filePath);
                _changeCustomEmbedService.CreateAllCommandFiles(language);
            }
        }

        public string GetLanguage(ulong guildID, ulong userID)
        {
            var guildAccount = _guildAccountService.GetOrCreateGuildAccount(guildID);
            var globalUser = _globalUserAccountService.GetOrCreateGlobalUserAccount(userID);
            var guildUser = _guildUserAccountService.GetOrCreateGuildUserAccount(guildID, userID);


            if (guildAccount.AllowMembersOwnLanguageSetting == true)
            {
                if (globalUser.Language == String.Empty) { return guildUser.Language; }
                else { return globalUser.Language; }
            }
            return guildAccount.Language;
        }

        public void GenerateNewLanguageList()
        {
            _languageList.Clear();
            _languageList.Add("English");
            _languageList.Add("Dutch");
            _languageList.Add("French");
            _languageList.Add("Spanish");
        }
    }
}