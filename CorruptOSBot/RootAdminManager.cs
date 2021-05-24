using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Services;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorruptOSBot.Shared
{
    public static class RootAdminManager
    {
        private static string BotVersion;
        public static void Init()
        {            
            BotVersion = ConfigHelper.GetSettingProperty("bot-version");
        }
        public static string GetBotVersion()
        {
            return BotVersion;
        }
    }
}
