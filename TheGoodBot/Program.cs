using System.Threading.Tasks;
using TheGoodBot.Core;

namespace TheGoodBot
{
    class Program
    {
        static Task Main(string[] args)
            => new BasicBotClient().InitializeAsync();
    }
}