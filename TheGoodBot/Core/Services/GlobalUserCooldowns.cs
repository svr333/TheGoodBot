using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace TheGoodBot.Core.Services
{
    public class GlobalUserCooldowns
    {
        private ConcurrentDictionary<string, DateTime> _userCooldowns = new ConcurrentDictionary<string, DateTime>();

        public void GetCooldownDictionary()
        {
            if (!File.Exists($"CurrentCooldowns.json")) { File.WriteAllText($"CurrentCooldowns.json", "");}
            var json = File.ReadAllText($"CurrentCooldowns.json");
            _userCooldowns = JsonConvert.DeserializeObject<ConcurrentDictionary<string, DateTime>>(json);
            if (_userCooldowns == null) { _userCooldowns = new ConcurrentDictionary<string, DateTime>(); }
        }

        public void SaveDictionary(ConcurrentDictionary<string, DateTime> dic)
            => File.WriteAllText($"CurrentCooldowns.json", JsonConvert.SerializeObject(dic));

        public bool GetUserCooldown(string key, out DateTime endsAt)
        {
            GetCooldownDictionary();
            var succeeded = _userCooldowns.TryGetValue(key, out endsAt);
            SaveDictionary(_userCooldowns);
            return succeeded;
        }

        public void ChangeUserCooldown(string key, DateTime time, DateTime endsAt)
        {
            GetCooldownDictionary();
            _userCooldowns.TryUpdate(key, time, endsAt);
            SaveDictionary(_userCooldowns);
        }

        public void AddUserCooldown(string key, DateTime time)
        {
            GetCooldownDictionary();
            _userCooldowns.TryAdd(key, time);
            SaveDictionary(_userCooldowns);
        }
    }
}
