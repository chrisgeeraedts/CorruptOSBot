using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;

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
                var json = new WebClient().DownloadString(ConfigurationManager.AppSettings["runewatchUri"]);

                List<RunewatchEntry>  runewatchEntry = JsonConvert.DeserializeObject<List<RunewatchEntry>>(json);
                lastUpdated = DateTime.Now;

                return runewatchEntry;
            }
            return _runewatchEntries;
        }
    }
}
