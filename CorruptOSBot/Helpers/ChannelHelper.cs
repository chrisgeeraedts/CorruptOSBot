﻿using System;
using System.Collections.Generic;

namespace CorruptOSBot.Helpers
{
    public static class ChannelHelper
    {
        private static Dictionary<string, ulong> KnownChannels = new Dictionary<string, ulong>();
        public static ulong GetChannelId(string name)
        {
            if (!KnownChannels.ContainsKey(name))
            {
                try
                {
                    string channelSettingProp = string.Format("channel_{0}", name);

                    var potentialResult = ConfigHelper.GetSettingProperty(channelSettingProp);
                    var result = Convert.ToUInt64(potentialResult);
                    KnownChannels.Add(name, result);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return KnownChannels[name];
        }
    }
}
