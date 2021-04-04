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
            clanId = 128;
            verificationCode = ConfigurationManager.AppSettings["WOMCode"];
        }


        public Competition GetCompetitions(int compId)
        {
            Competition product = null;
            HttpResponseMessage response = client.GetAsync(path + string.Format("/competitions/{0}", compId)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Competition>(result);
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


        public Clan GetClan(int clanId)
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
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Member
    {
        public string username { get; set; }
        public string role { get; set; }
    }

    public class Root
    {
        public string verificationCode { get; set; }
        public List<Member> members { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class NameChangeRoot
    {
        public string oldName { get; set; }
        public string newName { get; set; }
    }



}
