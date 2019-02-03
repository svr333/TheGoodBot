using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using TheGoodBot.Storage.Implementations;

namespace TheGoodBot.Languages
{
    public class LanguageSelector
    {
        private string filePath;
        private LanguageStorage _languageStorage;

        public LanguageSelector(LanguageStorage languageStorage)
        {
            _languageStorage = languageStorage;
        }
        // Get language from GuildAccount, get setting from GuildAccount
        // And if true, get User Language. That Language value is what we use.
        // Make GetFormatted method (Peter's alerts.json)
        public string GetText(string key, string language)
        {
            filePath = "Languages/LanguageFiles/" + language;
            var receivedLanguage = _languageStorage.RestoreObject<Dictionary<string, string>>(filePath, language);
            return receivedLanguage[key];
        }

    }
}