using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using Discord.Commands;
using Newtonsoft.Json;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Guilds
{
    public class CooldownService
    {
        private ConcurrentDictionary<string, uint> Cooldowns = new ConcurrentDictionary<string, uint>();

        private CommandService _command;
        private string filePath = "";

        public CooldownService(CommandService command)
        {
            _command = command;
        }

        public void CreateAllPairs(ulong guildID)
        {
            var commands = _command.Commands.ToList();

            for (int i = 0; i < commands.Count; i++)
            {
                var key = $"{commands[i].Module.Group}-{commands[i].Name}";

                if (Cooldowns.ContainsKey(key)) continue;
                if(Cooldowns.TryAdd(key, 50));
            }

            SaveAccount(guildID);
        }

        private void GetAccount(ulong guildID)
        {
            filePath = $"GuildAccounts/{guildID}/Cooldowns.json";
            var json = File.ReadAllText(filePath);
            Cooldowns = JsonConvert.DeserializeObject<ConcurrentDictionary<string, uint>>(json);
        }

        private void SaveAccount(ulong guildID)
        {
            filePath = $"GuildAccounts/{guildID}/Cooldowns.json";
            var rawData = JsonConvert.SerializeObject(Cooldowns, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public uint GetCooldown(string key, ulong guildID)
        {
            GetAccount(guildID);
            Cooldowns.TryGetValue(key, out uint value);
            return value;
        }
    }
}