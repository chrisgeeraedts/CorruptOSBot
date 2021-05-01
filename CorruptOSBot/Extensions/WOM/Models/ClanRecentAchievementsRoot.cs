using System;

namespace CorruptOSBot.Extensions
{
    public class ClanRecentAchievementsRoot
    {
        public int threshold { get; set; }
        public int playerId { get; set; }
        public string name { get; set; }
        public string measure { get; set; }
        public string metric { get; set; }
        public DateTime? createdAt { get; set; }
        public Player player { get; set; }
    }
}
