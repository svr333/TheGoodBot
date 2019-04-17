using Discord.WebSocket;
using System.IO;
using System.IO.Compression;

namespace TheGoodBot.Core.Services.Commands
{
    public class RequestFileService
    {

        public string ZipFiles(string fileType, ulong guildId)
        {
            var filePath = GetFiles(fileType, guildId);
            ZipFile.CreateFromDirectory(filePath, $"Temp-{fileType}-{guildId}-files.zip");
            return $"Temp-{fileType}-{guildId}-files.zip";
        }

        public string GetFiles(string fileType, ulong guildId) 
            => $"{fileType}/{guildId}";

        public void RequestFiles(string fileType, ulong guildId, SocketUser user)
        {
            if (fileType == "all")
            {
                RequestFiles("GuildAccounts", guildId, user);
                RequestFiles("Languages", guildId, user);
                return;
            }
            var filePath = ZipFiles(fileType, guildId);
            var dmChannel = user.GetOrCreateDMChannelAsync().Result;
            dmChannel.SendFileAsync(filePath, 
                $"You have requested `{fileType}` files. Please do not continue if you don't know what you're doing. " +
                $"Info can be found here: <insert link here Senne, ty>");

            File.Delete(filePath);
        }
    }
}
