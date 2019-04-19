using System.Collections.Generic;

namespace TheGoodBot.Entities.Achievements
{
    public class TieredAchievements
    {
        public uint Tier { get; set; }
        public string Name { get; set; }
        public string PublicDescription { get; set; }
        public uint ReceivalPoints { get; set; }

        public uint GetReceivalPoints(uint Tier)
            => Tier * 10;
    }
}
