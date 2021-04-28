using CorruptOSBot.Shared.Helpers.Bot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace CorruptOSBot.Extensions
{
    public class RunewatchClient
    {
        List<RunewatchEntry> _runewatchEntries;

        DateTime lastUpdated;
        public RunewatchClient()
        {
            _runewatchEntries = GetRunewatchEntries();
        }
        public List<RunewatchEntry> GetRunewatchEntries()
        {
            var diffInHours = DateTime.Now.Subtract(lastUpdated).TotalHours;
            if (diffInHours > 24)
            {
                var json = new WebClient().DownloadString(ConfigHelper.GetSettingProperty("runewatchUri"));

                List<RunewatchEntry>  runewatchEntry = JsonConvert.DeserializeObject<List<RunewatchEntry>>(json);
                lastUpdated = DateTime.Now;

                return runewatchEntry;
            }
            return _runewatchEntries;
        }
    }
}
