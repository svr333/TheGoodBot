﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Core.Services.Languages;
using TheGoodBot.Entities.GuildAccounts;

namespace TheGoodBot.Core.Services.Accounts.GuildAccounts
{
    public class GuildAccountService
    {
        private List<ulong> _guildIDs;
        private string filePath = "AllGuildID's.json";
        private CreateLanguageFilesService _languageFileService;
        private CreateGuildAccountFilesService _guildFiles;
        private CooldownService _cooldown;
        private InvokeService _invoke;

        public GuildAccountService(CreateGuildAccountFilesService guildFiles, CooldownService cooldown,
            InvokeService invoke, CreateLanguageFilesService languageFilesService)
        {
            if (!File.Exists(filePath)) { CreateFile(); }
            string json = File.ReadAllText(filePath);
            _guildIDs = JsonConvert.DeserializeObject<List<ulong>>(json);

            _languageFileService = languageFilesService;
            _guildFiles = guildFiles;
            _cooldown = cooldown;
            _invoke = invoke;
        }

        /// <summary>Creates all guild cooldowns of the guilds previously saved in the file. </summary>
        public void CreateAllGuildCooldownsAndInvocations()
        {
            _guildIDs = GetAllGuildIDs();
            for (int i = 0; i < _guildIDs.Count; i++)
            {
                _cooldown.CreateAllPairs(_guildIDs[i]);
                _invoke.CreateAllPairs(_guildIDs[i]);
            }
        }

        /// <summary>Creates an empty file at that file location. </summary>
        private void CreateFile()
            => File.WriteAllText(filePath, "");

        /// <summary>Creates all guild accounts - if not made already - saved in the file. </summary>
        public void CreateAllGuildAccounts()
        {
            _languageFileService.CreateAllGuildLanguages(_guildIDs);
            _guildIDs = GetAllGuildIDs();
            for (int i = 0; i < _guildIDs.Count; i++)
            {
                _guildFiles.CreateGuildAccountFiles(_guildIDs[i]);
            }
        }

        /// <summary>Returns a list of all the already saved guilds. </summary>
        /// <returns></returns>
        private List<ulong> GetAllGuildIDs()
        {
            var json = File.ReadAllText(filePath);
            var guildList = JsonConvert.DeserializeObject<List<ulong>>(json);
            return guildList;
        }

        /// <summary>Add guild to the list of guilds. </summary>
        /// <param name="ID"></param>
        public void AddGuild(ulong ID)
        {
            _guildIDs.Add(ID);
            SaveAccounts(_guildIDs);
        }

        /// <summary>Saves all guild's id's in a file that can be used anywhere. </summary>
        /// <param name="guildIDs"></param>
        private void SaveAccounts(List<ulong> guildIDs)
        {
            var json = JsonConvert.SerializeObject(guildIDs, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        /// <summary>Save a copy of the Settings entity at the guild location. </summary>
        /// <param name="guildAccount"></param>
        /// <param name="guildID"></param>
        public void SaveSettingsGuildAccount(Settings guildAccount, ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}/Settings.json"; 
            string rawData = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        /// <summary>Save a copy of the Stats entity at the guild location. </summary>
        /// <param name="guildAccount"></param>
        /// <param name="guildID"></param>
        public void SaveStatsGuildAccount(Stats guildAccount, ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}/Stats.json";
            string rawData = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        /// <summary>Get a copy of the already saved Settings entity. </summary>
        /// <param name="guildID"></param>
        /// <returns></returns>
        public Settings GetSettingsAccount(ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}/Settings.json";
            if (!File.Exists(filePath)) { _guildFiles.CreateGuildAccountFiles(guildID);}
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Settings>(rawData);
        }

        /// <summary>Get a copy of the already saved Stats entity. </summary>
        /// <param name="guildID"></param>
        /// <returns></returns>
        public Stats GetStatsAccount(ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}/Stats.json";
            if (!File.Exists(filePath)) { _guildFiles.CreateGuildAccountFiles(guildID); }
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Stats>(rawData);
        }

        /// <summary>Get the cooldown of that command(-name) in that specific guild. If 0 -> return global cooldown. </summary>
        /// <param name="key"></param>
        /// <param name="guildID"></param>
        /// <returns></returns>
        public uint GetMaxCooldown(string key, ulong guildID)
        {
            var Sguild = GetSettingsAccount(guildID);
            var cooldown = _cooldown.GetMaxCooldown(key, guildID);

            if (!(Sguild.GlobalCooldown == 0))
            {
                if (Sguild.GlobalCooldown >= cooldown) { cooldown = Sguild.GlobalCooldown; }
            }         
            return cooldown;
        }

        public int GetInvocation(string key, ulong guildID)
        {
            int invokeTime = 0;
            var sGuildAccount = GetSettingsAccount(guildID);
            var commandInvokeTime = _invoke.GetInvokeTime(key, guildID);

            if (commandInvokeTime == 0 || commandInvokeTime == 50)
            {
                invokeTime = sGuildAccount.GlobalInvocationTime;
            }
            else { invokeTime = commandInvokeTime; }

            return invokeTime;
        }
    }
}
