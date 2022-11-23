using System;
using System.Collections.Generic;
using System.Linq;

namespace CorruptOSBot.Shared.Helpers.Bot
{
    public static class ConfigHelper
    {
        private static Dictionary<string, string> Configuration { get; set; }
        public static bool IsDebugMode { get; set; }

        public static void Init()
        {
            IsDebugMode = true;

            var config = new DataHelper().GetConfiguration();
            Configuration = new Dictionary<string, string>();

            foreach (var item in config)
            {
                if (!Configuration.ContainsKey(item.PropertyName.ToLower()))
                {
                    Configuration.Add(item.PropertyName.ToLower(), item.PropertyValue);
                }
            }
        }

        public static string GetSettingProperty(string key)
        {
            if (Configuration.ContainsKey(key.ToLower()))
            {
                return Configuration[key.ToLower()];
            }
            else
            {
                // try to load it from the config
                var configValue = new DataHelper().GetConfiguration().FirstOrDefault(x => x.PropertyName.ToLower() == key.ToLower());
                if (configValue != null)
                {
                    Configuration.Add(configValue.PropertyName.ToLower(), configValue.PropertyValue.ToLower());
                    return configValue.PropertyValue;
                }

                // cant find it in DB either
                return string.Empty;
            }
        }

        public static ulong GetGuildId()
        {
            if (!IsDebugMode)
            {
                return Convert.ToUInt64(GetSettingProperty("GuildId"));
            }
            else
            {
                return 827967957094105108; //Dev Discord Id
            }
        }
    }
}