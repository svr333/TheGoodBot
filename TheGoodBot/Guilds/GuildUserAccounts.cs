using TheGoodBot.Entities;

namespace TheGoodBot.Guilds
{
    public class GuildUserAccounts
    {
        private ulong _guildId;
        private ulong _userId;
        private string filePath;
        private GuildUserAccountService _guildUserAccountService;

        public GuildUserAccounts(GuildUserAccountService service = null)
        {
            _guildUserAccountService = service ?? new GuildUserAccountService();
        }

        public void SaveGuildUserAccount(GuildUserAccountStruct guildUser)
        {
            _guildId = guildUser.GuildId;
            _userId = guildUser.UserId;
            filePath = "GuildUserAccounts/" + _guildId + "/" +_userId + ".json";
            _guildUserAccountService.SaveGuildUserAccount(guildUser, filePath);
        }

        public GuildUserAccountStruct GetGuildUser(ulong guildID, ulong userID)
        {
            var guildUser = _guildUserAccountService.GetOrCreateGuildUserAccount(guildID, userID);
            return guildUser;
        }
    }
}