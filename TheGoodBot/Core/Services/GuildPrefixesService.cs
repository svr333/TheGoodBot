using System.Collections.Generic;
using System.Text;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Services
{
    public class GuildPrefixesService
    {
        private GuildAccountService _guildAccountService;

        public GuildPrefixesService(GuildAccountService guildAccountService)
        {
            _guildAccountService = guildAccountService ?? new GuildAccountService();
        }

        public List<string> GetPrefixes(ulong guildID)
        {
            var guildPrefixesList = _guildAccountService.GetOrCreateGuildAccount(guildID).prefixesList;
            return guildPrefixesList;
            /*var sb = new StringBuilder();

            for (int i = 0; i < guildPrefixesList.Count; i++)
            {
                sb.Append("|" + guildPrefixesList[i]);
            }*/
        }
    }
}