using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
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
                List<RunewatchEntry> runewatchEntry = _runewatchEntries;
                try
                {
                    var json = new WebClient().DownloadString(ConfigHelper.GetSettingProperty("runewatchUri"));

                    runewatchEntry = JsonConvert.DeserializeObject<List<RunewatchEntry>>(json);
                    lastUpdated = DateTime.Now;
                }
                catch (Exception e)
                {
                    Program.Log(new LogMessage(LogSeverity.Error, "PVMRoleService", "Failed to remove role - " + e.Message));
                }
                return runewatchEntry;
            }
            return _runewatchEntries;
        }
    }
}
