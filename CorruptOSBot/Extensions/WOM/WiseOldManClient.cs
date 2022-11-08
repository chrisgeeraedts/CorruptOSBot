using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using CorruptOSBot.Helpers;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace CorruptOSBot.Extensions
{
    public class WiseOldManClient : IDisposable
    {
        private int clanId;
        private HttpClient client;
        private string path;
        private string verificationCode;

        public WiseOldManClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", SettingsConstants.WOMApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", SettingsConstants.GMKirbyDiscordTag);
            path = "https://api.wiseoldman.net/v2";
            clanId = Convert.ToInt32(ConfigHelper.GetSettingProperty("WOMClanId"));
            verificationCode = ConfigHelper.GetSettingProperty("WOMCode");
        }

        #region Clan Endpoints

        public Clan GetClan()
        {
            Clan clanDetails = null;
            HttpResponseMessage response = client.GetAsync($"{path}/groups/{clanId}").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                clanDetails = JsonConvert.DeserializeObject<Clan>(result);
            }
            return clanDetails;
        }

        public List<ClanMember> GetClanMembers()
        {
            List<ClanMember> clanMembers = null;
            HttpResponseMessage response = client.GetAsync($"{path}/groups/{clanId}").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var clan = JsonConvert.DeserializeObject<Clan>(result);

                clanMembers = clan.memberships;
            }
            return clanMembers;
        }

        public List<Competition> GetClanCompetitions()
        {
            List<Competition> product = null;
            HttpResponseMessage response = client.GetAsync($"{path}/groups/{clanId}/competitions").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<List<Competition>>(result);
            }
            return product;
        }

        public void RemoveGroupMember(string rsn)
        {
            try
            {
                var content = JsonConvert.SerializeObject(new RemoveMemberRoot()
                {
                    verificationCode = verificationCode,
                    members = new List<string>()
                {
                    rsn
                }
                });

                HttpContent payload = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync($"{path}/groups/{clanId}/remove-members", payload).Result;
                if (response.IsSuccessStatusCode)
                {
                    response.Content.ReadAsStringAsync();
                }
                else
                {
                    var result = response.ReasonPhrase;
                    Program.Log(new LogMessage(LogSeverity.Error, "RemoveGroupMember", String.Format("{0}", result)));
                }
            }
            catch (System.Exception e)
            {
                Program.Log(new LogMessage(LogSeverity.Error, "RemoveGroupMember", "Failed to change WOM - " + e.Message));
            }
        }

        public ClanMember AddGroupMember(string rsn)
        {
            var content = JsonConvert.SerializeObject(new Root()
            {
                verificationCode = verificationCode,
                members = new List<Member>()
                {
                    new Member() {username = rsn, role = "helper"}
                }
            });

            HttpContent payload = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync($"{path}/groups/{clanId}/add-members", payload).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var clanMembers = JsonConvert.DeserializeObject<AddMemberRoot>(result);
                return clanMembers.members.FirstOrDefault(x => x.player.username.ToLower() == rsn.ToLower());
            }
            return null;
        }

        public List<ClanRecentAchievementsRoot> GetClanRecentAchievements()
        {
            List<ClanRecentAchievementsRoot> product = null;
            HttpResponseMessage response = client.GetAsync($"{path}/groups/{clanId}/achievements").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<List<ClanRecentAchievementsRoot>>(result);
            }
            return product;
        }

        public List<ClanHiscoreEntry> GetClanHiscores(string metric, int limit)
        {
            List<ClanHiscoreEntry> clanHiscores = null;
            HttpResponseMessage response = client.GetAsync($"{path}/groups/{clanId}/hiscores?metric={metric}&limit={limit}").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                clanHiscores = JsonConvert.DeserializeObject<List<ClanHiscoreEntry>>(result);
            }
            return clanHiscores;
        }

        #endregion

        #region Competition Endpoints

        public CompetitionDetail GetCompetition(int compId)
        {
            CompetitionDetail product = null;
            HttpResponseMessage response = client.GetAsync($"{path}/competitions/{compId}").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<CompetitionDetail>(result);
            }
            return product;
        }

        public bool UpdateAllParticipants()
        {
            List<Competition> Competetions = GetClanCompetitions();

            var CompList = Competetions.Where(x =>
            x.startsAt < DateTime.Now &&
            x.endsAt > DateTime.Now);

            if (CompList.Any())
            {
                var currentComp = CompList.OrderBy(x => x.id).First();
                var content = JsonConvert.SerializeObject(new Root()
                {
                    verificationCode = verificationCode
                });

                HttpContent payload = new StringContent(content, Encoding.UTF8, "application/json");

                Program.Log(new LogMessage(LogSeverity.Info, "CompId", currentComp.id.ToString()));

                HttpResponseMessage response = client.PostAsync($"{path}/competitions/{currentComp.id}/update-all", payload).Result;

                if (response.IsSuccessStatusCode)
                {
                    Program.Log(new LogMessage(LogSeverity.Info, "UpdateAllParticipants", "Update Call Success"));
                    return true;
                }
                else
                {
                    Program.Log(new LogMessage(LogSeverity.Info, "UpdateAllParticipants", "Update Call Failed."));
                }
            }
            else
            {
                Program.Log(new LogMessage(LogSeverity.Info, "UpdateAllParticipants", "No active event is running at this moment."));
            }
            return false;
        }

        #endregion


        #region Misc Endpoints

        public string PostNameChange(string oldName, string newName)
        {
            var content = JsonConvert.SerializeObject(new NameChangeRoot()
            {
                newName = newName,
                oldName = oldName
            });

            HttpContent payload = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync($"{path}/names", payload).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return string.Empty;
            }
            else
            {
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        internal ClanMemberDetail GetPlayerDetails(string username)
        {
            ClanMemberDetail clanMemberDetail = null;
            HttpResponseMessage response = client.GetAsync($"{path}/players/{username}").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                clanMemberDetail = JsonConvert.DeserializeObject<ClanMemberDetail>(result);
            }
            else
            {
                Console.WriteLine($"Failed to get WOM details for: {username}");
            }
            return clanMemberDetail;
        }

        public List<ClanMember> SearchUsersByName(string clanMemberRsn)
        {
            List<ClanMember> foundClanMembers = null;
            HttpResponseMessage response = client.GetAsync($"{path}/players/search?username={clanMemberRsn}").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                foundClanMembers = JsonConvert.DeserializeObject<List<ClanMember>>(result);
            }
            return foundClanMembers;
        }

        #endregion

        private class AddMemberRoot
        {
            public List<ClanMember> members { get; set; }
        }

        public void Dispose()
        {
            client = null;
        }
    }
}