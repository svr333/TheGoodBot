namespace TheGoodBot.Entities
{
    public class GuildUserAccount
    {
        public ulong UserId { get; set; }
        public ulong GuildId { get; set; }
        public string Language { get; set; }
        public uint Colour { get; set; }
        public uint  JoinPosition { get; set; }
    }
}