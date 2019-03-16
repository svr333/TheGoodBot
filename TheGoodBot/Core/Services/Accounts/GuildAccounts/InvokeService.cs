using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using Discord.Commands;
using System.Linq;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Services.Accounts.GuildAccounts
{
    public class InvokeService
    {
        private ConcurrentDictionary<string, int> Invokes;
        private CommandService _command;

        private string filePath;

        public InvokeService(CommandService command)
        {
            _command = command;
        }

        public void CreateAllPairs(ulong guildID)
        {
            Invokes = new ConcurrentDictionary<string, int>();
            GetInvocationAccount(guildID);
            var commands = _command.Commands.ToList();

            for (int i = 0; i < commands.Count; i++)
            {
                var key = $"{commands[i].Module.Group}-{commands[i].Name}";

                if (Invokes.ContainsKey(key)) continue;
                Invokes.TryAdd(key, 0);
            }
            SaveAccount(guildID);
        }

        private void GetInvocationAccount(ulong guildID)
        {
            filePath = $"GuildAccounts/{guildID}/Invokes.json";
            if (!File.Exists(filePath)) { File.WriteAllText(filePath, ""); }
            var json = File.ReadAllText(filePath);
            Invokes = JsonConvert.DeserializeObject<ConcurrentDictionary<string, int>>(json);
            if (Invokes == null) { Invokes = new ConcurrentDictionary<string, int>(); }
        }

        private void SaveAccount(ulong guildID)
        {
            filePath = $"GuildAccounts/{guildID}/Invokes.json";
            var rawData = JsonConvert.SerializeObject(Invokes, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public int GetInvokeTime(string commandName, ulong guildID)
        {
            GetInvocationAccount(guildID);
            Invokes.TryGetValue(commandName, out int value);
            return value;
        }
    }
}