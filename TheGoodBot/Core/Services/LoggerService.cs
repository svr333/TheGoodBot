using System;

namespace TheGoodBot.Core.Services
{
    public class LoggerService
    {
        public void Log(string msg)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("dd/M HH:mmtt")}] - {msg}");
        }
    }
}