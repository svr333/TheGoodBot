using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using Discord.Commands;
using Newtonsoft.Json;

namespace TheGoodBot.Core.Services.Accounts.GuildAccounts
{
    public class CooldownService
    {
        private ConcurrentDictionary<string, uint> _cooldowns;

        private readonly CommandService _command;
        private string filePath = "";

        public CooldownService(CommandService command)
        {
            _command = command;
        }

        public void CreateAllPairs(ulong guildId)
        {
            _cooldowns = new ConcurrentDictionary<string, uint>();
            GetCooldownAccount(guildId);
            var commands = _command.Commands.ToList();

            for (int i = 0; i < commands.Count; i++)
            {
                var key = $"{commands[i].Module.Group}-{commands[i].Name}";

                if (_cooldowns.ContainsKey(key)) continue;
                _cooldowns.TryAdd(key, 0);
            }

            SaveAccount(guildId);
        }

        private void GetCooldownAccount(ulong guildId)
        {
            filePath = $"GuildAccounts/{guildId}/Cooldowns.json";
            if (!File.Exists(filePath)) { File.WriteAllText(filePath, ""); }
            var json = File.ReadAllText(filePath);
            _cooldowns = JsonConvert.DeserializeObject<ConcurrentDictionary<string, uint>>(json);
            if (_cooldowns == null) { _cooldowns = new ConcurrentDictionary<string, uint>(); }
        }

        private void SaveAccount(ulong guildId)
        {
            filePath = $"GuildAccounts/{guildId}/Cooldowns.json";
            var rawData = JsonConvert.SerializeObject(_cooldowns, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public uint GetMaxCooldown(string key, ulong guildId)
        {
            GetCooldownAccount(guildId);
            _cooldowns.TryGetValue(key, out uint value);
            return value;
        }
    }
}
