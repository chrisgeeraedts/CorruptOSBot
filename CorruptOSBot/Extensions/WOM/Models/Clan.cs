using System;
using System.Collections.Generic;

namespace CorruptOSBot.Extensions
{
    public class Clan
    {
        public int id { get; set; }
        public string name { get; set; }
        public string clanChat { get; set; }
        public string description { get; set; }
        public int homeworld { get; set; }
        public int score { get; set; }
        public bool verified { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int memberCount { get; set; }
        public List<ClanMember> memberships { get; set; }
    }
}
