using System;
using System.IO;
using System.Net;
using Discord.Commands;
using Newtonsoft.Json;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Services.Commands
{
    public class LevelSystemService
    {
        private GuildUserAccountService _guildUser;
        private GuildAccountService _guild;

        private SocketCommandContext _context;
        private string _filePath;
        private float _amountOfPages;

        public LevelSystemService(GuildUserAccountService guildUser, GuildAccountService guild)
        {
            _guildUser = guildUser;
            _guild = guild;
        }

        public void ConvertMee6Levels(SocketCommandContext context)
        {
            context.Channel.SendMessageAsync("This can take a while. Please be patient.");
            DateTime start = DateTime.UtcNow;
            GetAmountOfPages(context.Guild.MemberCount);
            _context = context;
            PullAllMee6Data();
            SetAllData();
            context.Channel.SendMessageAsync(
                $"It took me {(DateTime.UtcNow - start).TotalSeconds} to do this for {context.Guild.MemberCount} users.");
        }

        private void SetAllData()
        {
            // TODO: Run a method that adds up all the users xp and adds it to guildStats
            DateTime now = DateTime.UtcNow;
            for (int i = 0; i < _amountOfPages + 1; i++)
            {
                SetFilePath(_context.Guild.Id, i);
                var json = File.ReadAllText(_filePath);
                var mee6 = JsonConvert.DeserializeObject<Mee6>(json);

                for (int j = 0; j < mee6.Users.Count; j++)
                {
                    ulong userId = Convert.ToUInt64(mee6.Users[j].Id);
                    var guildUser = _guildUser.GetOrCreateGuildUserAccount(_context.Guild.Id, userId);
                    guildUser.Xp = Convert.ToUInt32(mee6.Users[j].Xp);
                    _guildUser.SaveGuildUserAccount(guildUser, _context.Guild.Id, userId);
                }

                Console.WriteLine((DateTime.UtcNow - now).TotalSeconds);
            }
            _context.Channel.SendMessageAsync($"All data has been set.\nThis took me {(DateTime.UtcNow - now).TotalSeconds} seconds.");
        }

        private void PullAllMee6Data()
        {
            DateTime start = DateTime.UtcNow;
            for (int i = 0; i < _amountOfPages + 1; i++)
            {
                SetFilePath(_context.Guild.Id, i);
                WebClient web = new WebClient();
                try
                {
                    var json = web.DownloadString($"https://mee6.xyz/api/plugins/levels/leaderboard/{_context.Guild.Id}?page={i}&limit=999");
                    File.WriteAllText(_filePath, json);
                }
                catch (Exception e)
                {
                    // TODO: Catch this in CommandFailedService
                    _context.Channel.SendMessageAsync("Your server doesn't have Mee6 setup!");
                }
            }
            _context.Channel.SendMessageAsync($"All data has been pulled, now applying all data.\nThis took me {(DateTime.UtcNow-start).TotalSeconds} seconds.");
        }

        private void SetFilePath(ulong guildId, int i)
        {
            Directory.CreateDirectory($"GuildAccounts/{guildId}/Mee6");
            _filePath = $"GuildAccounts/{guildId}/Mee6/page{i}.json";
        }

        private void GetAmountOfPages(int memberCount)
        => _amountOfPages = memberCount / 999F;
    }
}
