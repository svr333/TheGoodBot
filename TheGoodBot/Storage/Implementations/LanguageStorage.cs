using Newtonsoft.Json;
using System.IO;
using static System.IO.Directory;

namespace TheGoodBot.Storage.Implementations
{
    public class LanguageStorage : IDataStorage
    {
        public T RestoreObject<T>(string key, string language)
        {
            var json = File.ReadAllText($"{key}.json");
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void StoreObject(object obj, string key)
        {
            var directory = "Languages/LanguageFiles";
            var filePath = directory + "/" + $"{key}.json";
            CreateDirectory(directory);
            var json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(filePath, json);
        }
    }
}