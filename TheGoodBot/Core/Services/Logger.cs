using System;

namespace TheGoodBot.Core.Services
{
    public class Logger
    {
        public void Log(string msg)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("dd/M HH:mmtt")}] - {msg}");
        }
    }
}