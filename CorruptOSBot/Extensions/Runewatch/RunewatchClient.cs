using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Extensions
{
    public class RunewatchClient
    {
        HttpClient client;
        List<RunewatchEntry> _runewatchEntries;

        DateTime lastUpdated;
        public RunewatchClient()
        {
            client = new HttpClient();
            _runewatchEntries = GetRunewatchEntries();
        }
        public List<RunewatchEntry> GetRunewatchEntries()
        {
            var diffInHours = DateTime.Now.Subtract(lastUpdated).TotalHours;
            if (diffInHours > 24)
            {
                List<RunewatchEntry> runewatchEntry = null;
                var json = new WebClient().DownloadString("https://raw.githubusercontent.com/while-loop/runelite-plugins/runewatch/data/mixedlist.json");

                runewatchEntry = JsonConvert.DeserializeObject<List<RunewatchEntry>>(json);
                lastUpdated = DateTime.Now;

                return runewatchEntry;
            }
            return _runewatchEntries;
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class RunewatchEntry
    {
        public string accused_rsn { get; set; }
        public string published_date { get; set; }
        public string reason { get; set; }
        public string evidence_rating { get; set; }
        public string short_code { get; set; }
        public string source { get; set; }
    }
}
