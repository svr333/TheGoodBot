using System;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Guilds;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Languages
{
    public class LanguageService
    {       
        private CreateLanguageFilesService _createLanguageFilesService;
        private GlobalUserAccountService _globalUserAccountService;
        private GuildAccountService _guildAccountService;
        private GuildUserAccountService _guildUserAccountService;

        public LanguageService(CreateLanguageFilesService createLanguageFilesService = null, GuildAccountService GuildAccount = null, GlobalUserAccountService GlobalUser = null, GuildUserAccountService GuildUser = null)
        {
            _createLanguageFilesService = createLanguageFilesService;
            _guildAccountService = GuildAccount;
            _globalUserAccountService = GlobalUser;
            _guildUserAccountService = GuildUser;
        }

        public string GetLanguage(ulong guildID, ulong userID)
        {
            var guildAccount = _guildAccountService.GetSettingsAccount(guildID);
            var globalUser = _globalUserAccountService.GetOrCreateGlobalUserAccount(userID);
            var guildUser = _guildUserAccountService.GetOrCreateGuildUserAccount(guildID, userID);

            var language = "";

            if (guildAccount.AllowMembersOwnLanguageSetting == true)
            {
                if (globalUser.Language == String.Empty) { language = guildUser.Language; }
                else { language = globalUser.Language; }
            }
            else language = guildAccount.Language;

            if (language == null || language == String.Empty) { language = "English"; }

            return language;
        }

        
    }
}