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
        public bool verified { get; set; }
        public bool patron { get; set; }
        public string profileImage { get;set; }
        public string bannerImage { get; set; }
        public int score { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int memberCount { get; set; }
        public List<ClanMember> memberships { get; set; }
        public SocialLinks socialLinks { get; set; }
    }

    public class SocialLinks
    {
        public string website { get; set; }
        public string discord { get; set; }
        public string twitter { get; set; }
        public string youtube { get; set; }
        public string twitch { get; set; }
    }
}
