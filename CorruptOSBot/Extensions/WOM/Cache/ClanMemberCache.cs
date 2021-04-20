using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using System;
using System.Collections.Generic;

namespace CorruptOSBot.Extensions.WOM
{
    public static partial class WOMMemoryCache
    {
        public class ClanMemberCache
        {
            public ClanMemberCache()
            {
                _clanMemberDetails = new List<ClanMemberDetail>();
            }
            public DateTime LastUpdated { get; set; }
            private List<ClanMemberDetail> _clanMemberDetails { get; set; }
            public List<ClanMemberDetail> ClanMemberDetails
            {
                get
                {
                    return _clanMemberDetails;
                }
                set
                {
                    _clanMemberDetails = value;
                    LastUpdated = DateTime.Now;
                }
            }
        }
    }
}
