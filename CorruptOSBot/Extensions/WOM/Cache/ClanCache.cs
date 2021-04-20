using System;

namespace CorruptOSBot.Extensions.WOM
{
    public static partial class WOMMemoryCache
    {
        public class ClanCache
        {
            public DateTime LastUpdated { get; set; }

            public Clan _clan { get; set; }
            public Clan Clan
            {
                get
                {
                    return _clan;
                }
                set
                {
                    _clan = value;
                    LastUpdated = DateTime.Now;
                }
            }
        }
    }
}
