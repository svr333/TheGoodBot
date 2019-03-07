using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord.Commands;
using Newtonsoft.Json;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Guilds
{
    public class CooldownService
    {
        private ConcurrentDictionary<string, uint> Cooldowns;

        private CommandService _command;
        private string filePath = "";

        public CooldownService(CommandService command)
        {
            _command = command;
        }

        public void CreateAllPairs(ulong guildID)
        {
            Cooldowns = new ConcurrentDictionary<string, uint>();
            GetCooldownAccount(guildID);
            var commands = _command.Commands.ToList();

            for (int i = 0; i < commands.Count; i++)
            {
                var key = $"{commands[i].Module.Group}-{commands[i].Name}";

                if (Cooldowns.ContainsKey(key)) continue;
                Cooldowns.TryAdd(key, 0);
            }

            SaveAccount(guildID);
        }

        private void GetCooldownAccount(ulong guildID)
        {
            filePath = $"GuildAccounts/{guildID}/Cooldowns.json";
            if (!File.Exists(filePath)) { File.WriteAllText(filePath, ""); }
            var json = File.ReadAllText(filePath);
            Cooldowns = JsonConvert.DeserializeObject<ConcurrentDictionary<string, uint>>(json);
            if (Cooldowns == null) { Cooldowns = new ConcurrentDictionary<string, uint>(); }
        }

        private void SaveAccount(ulong guildID)
        {
            filePath = $"GuildAccounts/{guildID}/Cooldowns.json";
            var rawData = JsonConvert.SerializeObject(Cooldowns, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public uint GetCooldown(string key, ulong guildID)
        {
            GetCooldownAccount(guildID);
            Cooldowns.TryGetValue(key, out uint value);
            return value;
        }
    }
}