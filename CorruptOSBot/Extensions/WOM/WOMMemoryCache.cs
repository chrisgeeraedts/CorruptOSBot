using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CorruptOSBot.Extensions.WOM
{
    public static partial class WOMMemoryCache
    {
        public static ClanCache Clan = new ClanCache();
        public static ClanMemberCache ClanMemberDetails = new ClanMemberCache();

        private static bool Reloading = false;

        public static int OneSecondMS { get => 1000; }
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
            try
            {
                var WomClient = new WiseOldManClient();
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Reloading memory Clan cache")));
                var updatedClan = WomClient.GetClan();
                Clan.Clan = updatedClan;
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Completed reloading memory Clan cache")));
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Failure updating clan: {0}", e)));
            }
        }

        public static async Task ForceUpdateClanMembers()
        {
            if (!Reloading)
            {
                Reloading = true;

                var WomClient = new WiseOldManClient();
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Reloading memory Clanmembers cache")));

                try
                {
                    var updatedClanMembers = WomClient.GetClanMembers();
                    if (updatedClanMembers != null)
                    {
                        var tempList = new List<ClanMemberDetail>();
                        int index = 1;
                        int max = updatedClanMembers.Count;

                        foreach (var updatedClanMember in updatedClanMembers)
                        {
                            var indexOfClanMember = updatedClanMembers.IndexOf(updatedClanMember);

                            if (indexOfClanMember % 100 == 0)
                            {
                                Thread.Sleep(1000 * 60); // Waits for 1 Minute
                            }

                            var data = WomClient.GetPlayerDetails(updatedClanMember.player.username);
                            if (data != null)
                            {
                                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Updating ({1}/{2}) {0}", data.displayName, index, max)));
                                tempList.Add(data);
                            }
                            index++;
                        }
                        ClanMemberDetails.ClanMemberDetails = tempList;
                        ClanMemberDetails.LastUpdated = DateTime.Now;
                        await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", $"Completed reloading memory Clanmembers cache. Updated {index} members at {DateTime.Now}."));
                    }
                    else
                    {
                        await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Failure updating")));
                    }
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Failure updating: {0}", e)));
                }
                finally
                {
                    Reloading = false;
                }
            }
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
                    var playerDetails = womClient.GetPlayerDetails(clanMemberRsn);

                    if (playerDetails != null)
                    {
                        await TryAndUpdateClanMember(womClient, playerDetails.displayName);
                    }
                }
                else
                {
                    await TryAndUpdateClanMember(womClient, clanMember.username);
                }
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Failed reloading memory Clanmember({0}) cache due to : {1}", clanMemberRsn, e.Message)));
            }
        }

        private static async Task TryAndUpdateClanMember(WiseOldManClient womClient, string username)
        {
            var playerdetails = womClient.GetPlayerDetails(username);
            var detailToRemove = ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.username == username);
            ClanMemberDetails.ClanMemberDetails.Remove(detailToRemove);
            ClanMemberDetails.ClanMemberDetails.Add(playerdetails);
            await Program.Log(new LogMessage(LogSeverity.Info, "WOMMemoryCache", string.Format("Completed reloading memory Clanmember({0}) cache", username)));
        }
    }
}