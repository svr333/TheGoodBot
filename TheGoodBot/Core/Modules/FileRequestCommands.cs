using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using TheGoodBot.Core.Services;

namespace TheGoodBot.Core.Modules
{
    [Group("requestfiles")]
    public class FileRequestCommands : ModuleBase<SocketCommandContext>
    {
        private RequestFileService _requestFileService;

        public FileRequestCommands(RequestFileService requestFileService)
        {
            _requestFileService = requestFileService;
        }

        [Command(""), Alias("all")]
        [Summary("Gives the user all files regarding their server.")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task RequestAllFiles()
            => _requestFileService.RequestFiles("all", Context.Guild.Id, Context.User);

        [Command("settings"), Alias()]
        [Summary("Gives the user all the settings files regarding their server.")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task RequestSettingsFiles()
            => _requestFileService.RequestFiles("GuildAccounts", Context.Guild.Id, Context.User);

        [Command(""), Alias()]
        [Summary("Gives the user all the Language files regarding their server.")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task RequestLanguageFiles()
            => _requestFileService.RequestFiles("Languages", Context.Guild.Id, Context.User);
    }
}
