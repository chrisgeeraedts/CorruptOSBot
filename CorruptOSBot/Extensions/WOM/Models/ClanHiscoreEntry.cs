using System;

namespace CorruptOSBot.Extensions
{
    public class ClanHiscoreEntry
    {
        public Player player { get; set; }

        public ClanHiscoreData data { get; set; }
    }

    public class ClanHiscoreData
    {
        public int rank { get; set; }

        // Levels
        public int experience { get; set; }
        public string level { get; set;  }

        // Boss
        public int kills { get; set; }

        // Activity
        public int scores { get; set; }

        // Computed Metric (EHP/EHB)
        public int value { get; set; }
    }
}
