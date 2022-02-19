using System;

namespace CorruptOSBot.Extensions.WOM.ClanMemberDetails
{
    public class ClanMemberDetail
    {
        public int exp { get; set; }
        public int id { get; set; }
        public string username { get; set; }
        public string displayName { get; set; }
        public string type { get; set; }
        public string build { get; set; }
        public string country { get; set; }
        public bool flagged { get; set; }
        public double ehp { get; set; }
        public double ehb { get; set; }
        public double ttm { get; set; }
        public double tt200m { get; set; }
        public DateTime? lastImportedAt { get; set; }
        public DateTime? lastChangedAt { get; set; }
        public DateTime? registeredAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public int? combatLevel { get; set; }
        public LatestSnapshot latestSnapshot { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}