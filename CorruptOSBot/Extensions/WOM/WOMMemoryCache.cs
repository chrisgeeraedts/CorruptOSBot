using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Extensions.WOM
{
    public static partial class WOMMemoryCache
    {

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
            if (details == null || CanUpdate(refreshIfAfterThisTimeInMs, details.LastUpdated))
            {
                await ForceUpdateClanMember(clanMemberId);
            }
        }

        public static async Task UpdateClanMember(int refreshIfAfterThisTimeInMs, string clanMemberRsn)
        {
            var details = ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == clanMemberRsn.ToLower());
            if (details == null || CanUpdate(refreshIfAfterThisTimeInMs, details.LastUpdated))
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
            if (updatedClanMembers != null)
            {
                var tempList = new List<ClanMemberDetail>();
                int index = 1;
                int max = updatedClanMembers.Count;
                foreach (var updatedClanMember in updatedClanMembers)
                {
                    var data = WomClient.GetPlayerDetails(updatedClanMember.id);
                    if (data != null)
                    {
                        await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Updating ({1}/{2}) {0}", data.displayName, index, max)));
                        tempList.Add(data);
                    }
                    index++;
                }
                ClanMemberDetails.ClanMemberDetails = tempList;
                ClanMemberDetails.LastUpdated = DateTime.Now;
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Completed reloading memory Clanmembers cache")));
            }
            else
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Failure updating")));
            }

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
            var womClient = new WiseOldManClient();
            await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Reloading memory Clanmember({0}) cache", clanMemberRsn)));

            var clanMember = ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName == clanMemberRsn);
            try
            {
                if (clanMember == null)
                {
                    // we dont have him yet (why?!?) Find player by username
                    var baseClanmember = womClient.SearchUsersByName(clanMemberRsn).Where(x => x.displayName.ToLower() == clanMemberRsn.ToLower()).ToList();
                    if (baseClanmember.Count == 1)
                    {
                        await TryAndUpdateClanMember(womClient, baseClanmember[0].id);
                    }
                }
                else
                {
                    await TryAndUpdateClanMember(womClient, clanMember.id);
                }
                //welp - quess not
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Failed reloading memory Clanmember({0}) cache due to : {1}", clanMemberRsn, e.Message)));
            }
        }

        private static async Task TryAndUpdateClanMember(WiseOldManClient womClient, int id)
        {
            var playerdetails = womClient.GetPlayerDetails(id);
            var detailToRemove = ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.id == id);
            ClanMemberDetails.ClanMemberDetails.Remove(detailToRemove);
            ClanMemberDetails.ClanMemberDetails.Add(playerdetails);
            await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Completed reloading memory Clanmember({0}) cache", id)));
        }
    }
}
