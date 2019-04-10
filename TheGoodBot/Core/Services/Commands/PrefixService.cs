using Discord.Commands;
using System.Collections.Generic;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Core.Services.Languages;
using TheGoodBot.Guilds;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Services.Commands
{
    public class PrefixService
    {
        private Settings _settingsAccount;
        private CustomEmbedService _embedService;
        private GuildAccountService _guildAccountService;

        public PrefixService(GuildAccountService guildAccountService, CustomEmbedService embedService)
        {
            _guildAccountService = guildAccountService;
            _embedService = embedService;
        }

        public string GetPrefixesAsString(ulong guildId)
            => GetCurrentPrefixList(guildId).ReturnListAsString();

        private List<string> GetCurrentPrefixList(ulong guildId)
        {
            _settingsAccount = _guildAccountService.GetSettingsAccount(guildId);
            return _settingsAccount.PrefixList;
        }

        public async void AddPrefixAsync(SocketCommandContext context, string newPrefix)
        {
            GetCurrentPrefixList(context.Guild.Id);

            if (newPrefix is "") { await _embedService.CreateAndPostEmbeds(context, "ParamPrefixRequired"); return; }
            else if (_settingsAccount.PrefixList.Contains(newPrefix)) { await _embedService.CreateAndPostEmbeds(context, "prefixAlreadyExists"); }
            _settingsAccount.PrefixList.Add(newPrefix);
            SaveAccount(_settingsAccount, context.Guild.Id);
            await _embedService.CreateAndPostEmbeds(context, "addprefix");
        }
        public async void RemovePrefixAsync(SocketCommandContext context, string prefix)
        {
            if (prefix is "") { await _embedService.CreateAndPostEmbeds(context, "ParamPrefixRequired"); return; }
            GetCurrentPrefixList(context.Guild.Id);
            if (!_settingsAccount.PrefixList.Contains(prefix)) { await _embedService.CreateAndPostEmbeds(context, "prefixDoesntExist"); }
            _settingsAccount.PrefixList.Remove(prefix);
            SaveAccount(_settingsAccount, context.Guild.Id);
            await _embedService.CreateAndPostEmbeds(context, "removeprefix");
        }

        private void SaveAccount(Settings settings, ulong guildId)
        {
            _guildAccountService.SaveSettingsGuildAccount(settings, guildId);
        }
    }
}
