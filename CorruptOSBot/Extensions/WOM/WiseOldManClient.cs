using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using CorruptOSBot.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Extensions
{
    public class WiseOldManClient
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


        public void AddCompParticipant(Competition competition, Participation participation)
        {
            throw new NotImplementedException();
            //Competition product = null;

            //var content = JsonConvert.SerializeObject(participation);

            //HttpContent c = new StringContent(content, Encoding.UTF8, "application/json");

            //HttpResponseMessage response = client.PostAsync(path + string.Format("/competitions/{0}/add-participants", competition.id), c).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var result = response.Content.ReadAsStringAsync().Result;
            //}
            //else
            //{
            //    var result = response.ReasonPhrase;
            //}
        }


        public void AddTeamCompParticipant(Competition competition, TeamCompParticipation.Root participation)
        {
            var content = JsonConvert.SerializeObject(participation);

            HttpContent c = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(path + string.Format("/competitions/{0}/add-teams", competition.id), c).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                var result = response.ReasonPhrase;
            }
        }


        public void AddGroupMember(string rsn)
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
            }
            else
            {
                var result = response.ReasonPhrase;
            }
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


        public List<Achievement> GetAchievements(int id)
        {
            List<Achievement> achievements = null;
            HttpResponseMessage response = client.GetAsync(path + string.Format("/players/{0}/achievements", clanId)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                achievements = JsonConvert.DeserializeObject<List<Achievement>>(result);
            }
            return achievements;
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Progress
    {
        public int start { get; set; }
        public int end { get; set; }
        public int gained { get; set; }
    }

    public class History
    {
        public DateTime? date { get; set; }
        public int value { get; set; }
    }

    public class Participant
    {
        public int exp { get; set; }
        public int id { get; set; }
        public string username { get; set; }
        public string displayName { get; set; }
        public string type { get; set; }
        public string build { get; set; }
        public object country { get; set; }
        public bool flagged { get; set; }
        public double ehp { get; set; }
        public double ehb { get; set; }
        public double ttm { get; set; }
        public double tt200m { get; set; }
        public DateTime? lastImportedAt { get; set; }
        public DateTime? lastChangedAt { get; set; }
        public DateTime? registeredAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public Progress progress { get; set; }
        public List<History> history { get; set; }
    }

    public class CompetitionDetail
    {
        public int id { get; set; }
        public string title { get; set; }
        public string metric { get; set; }
        public string type { get; set; }
        public int score { get; set; }
        public DateTime? startsAt { get; set; }
        public DateTime? endsAt { get; set; }
        public object groupId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public object group { get; set; }
        public string duration { get; set; }
        public int totalGained { get; set; }
        public List<Participant> participants { get; set; }
    }
}
