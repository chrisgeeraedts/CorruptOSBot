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
            if (!KnownChannels.ContainsKey(name.ToLower()))
            {
                try
                {
                    string channelSettingProp = string.Format("channel_{0}", name.ToLower());

                    var potentialResult = ConfigHelper.GetSettingProperty(channelSettingProp);
                    var result = Convert.ToUInt64(potentialResult);
                    KnownChannels.Add(name.ToLower(), result);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return KnownChannels[name.ToLower()];
        }
    }
}
