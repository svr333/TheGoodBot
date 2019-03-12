using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TheGoodBot.Core.Services.Logging;

namespace TheGoodBot.Core.Services
{
    public class LoggerService
    {
        private FailedCommandLogService _commandFailed;
        private CommandSucceededService _commandSucceeded;

        public LoggerService(FailedCommandLogService commandFailed)
        {
            _commandFailed = commandFailed;
        }

        public void LogFailedCommand(string message, ulong guildID, string logType)
        {
            try
            {
                _commandFailed.UpdateLog(message, guildID, logType);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public void LogSucceededCommand(string msg)
        {

        }

        public void LogGuildAccountChanges()
        {

        }

        public void LogGuildUserAccountChanges()
        {

        }
    }
}