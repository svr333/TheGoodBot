namespace TheGoodBot.Entities.Achievements
{
    public class Achievement
    {
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public string PublicDescription { get; set; }
        public string PrivateDescription { get; set; }
        public uint ReceivalPoints { get; set; }
    }
}
