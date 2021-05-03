using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using System;
using System.Collections.Generic;

namespace CorruptOSBot.Helpers.Discord
{
    public static class ChannelHelper
    {
        private static Dictionary<string, ulong> KnownChannels = new Dictionary<string, ulong>();
        public static ulong GetChannelId(string name)
        {
            if (KnownChannels.Count == 0)
            {
                var channels = new DataHelper().GetChannels();
                foreach (var channel in channels)
                {
                    KnownChannels.Add(channel.Name.ToLower(), Convert.ToUInt64(channel.DiscordChannelId));
                }
            }
            if (KnownChannels.ContainsKey(name.ToLower()))
            {
                return KnownChannels[name.ToLower()];
            }
            return 0;
        }
    }
}
