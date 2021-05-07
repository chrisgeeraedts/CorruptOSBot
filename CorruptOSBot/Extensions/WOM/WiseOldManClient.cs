using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
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
        int clanId;
        HttpClient client;
        string path;
        string verificationCode;
        public WiseOldManClient()
        {
            client = new HttpClient();
            path = "https://api.wiseoldman.net";
            clanId = Convert.ToInt32(ConfigHelper.GetSettingProperty("WOMClanId"));
            verificationCode = ConfigHelper.GetSettingProperty("WOMCode");
        }

        public List<Competition> GetClanCompetitions()
        {
            List<Competition> product = null;
            HttpResponseMessage response = client.GetAsync(path + string.Format("/groups/{0}/competitions", clanId)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<List<Competition>>(result);
            }
            return product;
        }

        public CompetitionDetail GetCompetition(int compId)
        {
            CompetitionDetail product = null;
            HttpResponseMessage response = client.GetAsync(path + string.Format("/competitions/{0}", compId)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<CompetitionDetail>(result);
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

                HttpContent c = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(path + string.Format("/groups/{0}/remove-members", clanId), c).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
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
                    new Member() {username = rsn, role = "Member"}
                }
            });

            HttpContent c = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(path + string.Format("/groups/{0}/add-members", clanId), c).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var clanMembers = JsonConvert.DeserializeObject<AddMemberRoot>(result);
                return clanMembers.members.FirstOrDefault(x => x.username.ToLower() == rsn.ToLower());
            }
            return null;
        }

        public void PostNameChange(string oldName, string newName)
        {
            var content = JsonConvert.SerializeObject(new NameChangeRoot()
            {
                newName = newName,
                oldName = oldName
            });

            HttpContent c = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(path + string.Format("/names", clanId), c).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                var result = response.ReasonPhrase;
            }
        }

        public Clan GetClan()
        {
            Clan product = null;
            HttpResponseMessage response = client.GetAsync(path + string.Format("/groups/{0}", clanId)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Clan>(result);
            }
            return product;
        }

        public List<ClanRecentAchievementsRoot> GetClanRecentAchievements()
        {
            List<ClanRecentAchievementsRoot> product = null;
            HttpResponseMessage response = client.GetAsync(path + string.Format("/groups/{0}/achievements", clanId)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<List<ClanRecentAchievementsRoot>>(result);
            }
            return product;
        }

        public List<ClanMember> GetClanMembers()
        {
            List<ClanMember> clanMembers = null;
            HttpResponseMessage response = client.GetAsync(path + string.Format("/groups/{0}/members", clanId)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                clanMembers = JsonConvert.DeserializeObject<List<ClanMember>>(result);
            }
            return clanMembers;
        }

        internal ClanMemberDetail GetPlayerDetails(int id)
        {
            ClanMemberDetail clanMemberDetail = null;
            HttpResponseMessage response = client.GetAsync(path + string.Format("/players/{0}", id)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                clanMemberDetail = JsonConvert.DeserializeObject<ClanMemberDetail>(result);
            }
            return clanMemberDetail;
        }

        public List<ClanMember> SearchUsersByName(string clanMemberRsn)
        {
            List<ClanMember> foundClanMembers = null;
            HttpResponseMessage response = client.GetAsync(path + string.Format("/players/search?username={0}", clanMemberRsn)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                foundClanMembers = JsonConvert.DeserializeObject<List<ClanMember>>(result);
            }
            return foundClanMembers;
        }

        //public List<Achievement> GetAchievements(int id)
        //{
        //    List<Achievement> achievements = null;
        //    HttpResponseMessage response = client.GetAsync(path + string.Format("/players/{0}/achievements", clanId)).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = response.Content.ReadAsStringAsync().Result;
        //        achievements = JsonConvert.DeserializeObject<List<Achievement>>(result);
        //    }
        //    return achievements;
        //}

        //public void AddCompParticipant(Competition competition, Participation participation)
        //{
        //    throw new NotImplementedException();
        //    //Competition product = null;

        //    //var content = JsonConvert.SerializeObject(participation);

        //    //HttpContent c = new StringContent(content, Encoding.UTF8, "application/json");

        //    //HttpResponseMessage response = client.PostAsync(path + string.Format("/competitions/{0}/add-participants", competition.id), c).Result;
        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    var result = response.Content.ReadAsStringAsync().Result;
        //    //}
        //    //else
        //    //{
        //    //    var result = response.ReasonPhrase;
        //    //}
        //}

        //public void AddTeamCompParticipant(Competition competition, TeamCompParticipation.Root participation)
        //{
        //    var content = JsonConvert.SerializeObject(participation);

        //    HttpContent c = new StringContent(content, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = client.PostAsync(path + string.Format("/competitions/{0}/add-teams", competition.id), c).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = response.Content.ReadAsStringAsync().Result;
        //    }
        //    else
        //    {
        //        var result = response.ReasonPhrase;
        //    }
        //}

        public void Dispose()
        {
            client = null;
        }


        public class AddMemberRoot
        {
            public List<ClanMember> members { get; set; }
        }
    }
}
