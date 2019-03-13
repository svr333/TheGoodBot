using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using Discord.Commands;
using System.Linq;

namespace TheGoodBot.Core.Services.Accounts.GuildAccounts
{
    public class InvokeService
    {
        private ConcurrentDictionary<string, uint> Invokes;
        private CommandService _command;

        private string filePath;

        public InvokeService(CommandService command)
        {
            _command = command;
        }

        public void CreateAllPairs(ulong guildID)
        {
            Invokes = new ConcurrentDictionary<string, uint>();
            GetCooldownAccount(guildID);
            var commands = _command.Commands.ToList();

            for (int i = 0; i < commands.Count; i++)
            {
                var key = $"{commands[i].Module.Group}-{commands[i].Name}";

                if (Invokes.ContainsKey(key)) continue;
                Invokes.TryAdd(key, 0);
            }
            SaveAccount(guildID);
        }

        private void GetCooldownAccount(ulong guildID)
        {
            filePath = $"GuildAccounts/{guildID}/Invokes.json";
            if (!File.Exists(filePath)) { File.WriteAllText(filePath, ""); }
            var json = File.ReadAllText(filePath);
            Invokes = JsonConvert.DeserializeObject<ConcurrentDictionary<string, uint>>(json);
            if (Invokes == null) { Invokes = new ConcurrentDictionary<string, uint>(); }
        }

        private void SaveAccount(ulong guildID)
        {
            filePath = $"GuildAccounts/{guildID}/Invokes.json";
            var rawData = JsonConvert.SerializeObject(Invokes, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public uint GetCooldown(string key, ulong guildID)
        {
            GetCooldownAccount(guildID);
            Invokes.TryGetValue(key, out uint value);
            return value;
        }
    }
}