using System.Collections.Generic;
using System.Linq;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Guilds
{
    public class GuildAccounts
    {
        //(auto-)creation of GuildAccounts
        private GuildAccountStruct _guildAccount;
        private string saveFile;
        private GuildAccountService _guildAccountService;

        public GuildAccounts(GuildAccountService service = null)
        {
            _guildAccountService = service ?? new GuildAccountService();
        }

        public void SaveAccount(GuildAccountStruct guildAccount, ulong guildID)
        {
            _guildAccount = guildAccount;
            _guildAccountService.SaveGuildAccount(_guildAccount, guildID);
        }

        public GuildAccountStruct GetGuildAccount(ulong guildID)
        {
            var account = _guildAccountService.GetOrCreateGuildAccount(guildID);
            return account;
        }
    }
}