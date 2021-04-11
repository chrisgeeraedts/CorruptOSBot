using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Extensions.WOM
{



    public static class WOMMemoryCache
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

        public class ClanMemberCache
        {
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

        public static ClanCache Clan = new ClanCache();
        public static ClanMemberCache ClanMemberDetails = new ClanMemberCache();

        public static int OneMinuteMS { get => 1000 * 60; }
        public static int OneHourMS { get => 1000 * 60 * 60; }
        public static int OneDayMS { get => 1000 * 60 * 60 * 24; }

        private static bool CanUpdate(int refreshIfAfterThisTimeInMs, DateTime datetime)
        {
            var diffInMS = DateTime.Now.Subtract(datetime).TotalMilliseconds;
            return diffInMS > refreshIfAfterThisTimeInMs;
        }



        public static async Task UpdateClan(int refreshIfAfterThisTimeInMs)
        {
            if (CanUpdate(refreshIfAfterThisTimeInMs, Clan.LastUpdated))
            {
                await ForceUpdateClan();
            }
        }

        public static async Task UpdateClanMembers(int refreshIfAfterThisTimeInMs)
        {
            if (CanUpdate(refreshIfAfterThisTimeInMs, ClanMemberDetails.LastUpdated))
            {
                await ForceUpdateClanMembers();
            }
        }

        public static async Task UpdateClanMember(int refreshIfAfterThisTimeInMs, int clanMemberId)
        {
            var details = ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.id == clanMemberId);
            if (CanUpdate(refreshIfAfterThisTimeInMs, details.LastUpdated))
            {
                await ForceUpdateClanMember(clanMemberId);
            }
        }

        public static async Task UpdateClanMember(int refreshIfAfterThisTimeInMs, string clanMemberRsn)
        {
            var details = ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName == clanMemberRsn);
            if (CanUpdate(refreshIfAfterThisTimeInMs, details.LastUpdated))
            {
                await ForceUpdateClanMember(clanMemberRsn);
            }
        }

        public static async Task ForceUpdateClan()
        {
            var WomClient = new WiseOldManClient();
            await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Reloading memory Clan cache")));
            var updatedClan = WomClient.GetClan();
            Clan.Clan = updatedClan;
            await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Completed reloading memory Clan cache")));
        }

        public static async Task ForceUpdateClanMembers()
        {
            var WomClient = new WiseOldManClient();
            await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Reloading memory Clanmembers cache")));

            var updatedClanMembers = WomClient.GetClanMembers();
            var tempList = new List<ClanMemberDetail>();
            int index = 1;
            int max = updatedClanMembers.Count;
            foreach (var updatedClanMember in updatedClanMembers)
            {
                var data = WomClient.GetPlayerDetails(updatedClanMember.id);
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Updating ({1}/{2}) {0}", data.displayName, index, max)));
                index++;
                tempList.Add(data);
            }
            ClanMemberDetails.ClanMemberDetails = tempList;

            await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Completed reloading memory Clanmembers cache")));
        }

        public static async Task ForceUpdateClanMember(int clanMemberId)
        {
            var WomClient = new WiseOldManClient();
            await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Reloading memory Clanmember({0}) cache", clanMemberId)));

            var playerdetails = WomClient.GetPlayerDetails(clanMemberId);
            var detailToRemove = ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.id == clanMemberId);
            ClanMemberDetails.ClanMemberDetails.Remove(detailToRemove);
            ClanMemberDetails.ClanMemberDetails.Add(playerdetails);

            await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Completed reloading memory Clanmember({0}) cache", clanMemberId)));
        }

        public static async Task ForceUpdateClanMember(string clanMemberRsn)
        {
            var WomClient = new WiseOldManClient();
            await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Reloading memory Clanmember({0}) cache", clanMemberRsn)));

            var id = ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName == clanMemberRsn)?.id;
            if (id.HasValue)
            {
                var playerdetails = WomClient.GetPlayerDetails(id.Value);
                var detailToRemove = ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName == clanMemberRsn);
                ClanMemberDetails.ClanMemberDetails.Remove(detailToRemove);
                ClanMemberDetails.ClanMemberDetails.Add(playerdetails);
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Completed reloading memory Clanmember({0}) cache", clanMemberRsn)));
            }            
            else
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Failed reloading memory Clanmember({0}) cache", clanMemberRsn)));
            }            
        }
    }
}
