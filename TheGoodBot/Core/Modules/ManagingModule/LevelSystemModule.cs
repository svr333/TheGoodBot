using System.Threading.Tasks;
using Discord.Commands;
using TheGoodBot.Core.Services.Commands;

namespace TheGoodBot.Core.Modules.ManagingModule
{
    public class LevelSystem : ModuleBase<SocketCommandContext>
    {
        private LevelSystemService _levelSystemService;

        public LevelSystem(LevelSystemService levelSystemService)
        {
            _levelSystemService = levelSystemService;
        }

        [Command("mee6", RunMode = RunMode.Async), Alias()]
        public async Task ImportSettingsFromMee6()
            => _levelSystemService.ConvertMee6Levels(Context);
    }
}
