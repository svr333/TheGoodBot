using System.Collections.Generic;
using Newtonsoft.Json;

namespace TheGoodBot.Entities
{
    public class Guild
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("premium")]
        public bool IsPremium { get; set; }
    }

    public class Player
    {
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("detailed_xp")]
        public List<int> DetailedXp { get; set; }
        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }
        [JsonProperty("guild_id")]
        public string GuildId { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("level")]
        public int Level { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("xp")]
        public int Xp { get; set; }
    }

    public class Role
    {
        [JsonProperty("color")]
        public int Colour { get; set; }
        [JsonProperty("hoist")]
        public bool IsHoisted { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("managed")]
        public bool IsManaged { get; set; }
        [JsonProperty("mentionable")]
        public bool IsMentionable { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("permissions")]
        public int Permissions { get; set; }
        [JsonProperty("position")]
        public int Position { get; set; }
    }

    public class RoleReward
    {
        [JsonProperty("rank")]
        public int Rank { get; set; }
        [JsonProperty("role")]
        public Role Role { get; set; }
    }

    public class Mee6
    {
        [JsonProperty("admin")]
        public bool IsAdmin { get; set; }
        [JsonProperty("banner_url")]
        public object BannerUrl { get; set; }
        [JsonProperty("guild")]
        public Guild Guild { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("player")]
        public Player Player { get; set; }
        [JsonProperty("players")]
        public List<Player> Users { get; set; }
        [JsonProperty("role_rewards")]
        public List<RoleReward> RoleRewards { get; set; }
        [JsonProperty("user_guild_settings")]
        public object UserGuildSettings { get; set; }
    }
}
