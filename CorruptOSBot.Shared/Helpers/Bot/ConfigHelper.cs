using System;
using System.Collections.Generic;
using System.Linq;

namespace CorruptOSBot.Shared.Helpers.Bot
{
    public static class ConfigHelper
    {
        private static Dictionary<string, string> _configuration { get; set; }
        public static bool DEBUG { get; set; }

        public static void Init()
        {

            DEBUG = false;


            var config = new DataHelper().GetConfiguration();
            _configuration = new Dictionary<string, string>();
            foreach (var item in config)
            {
                if (!_configuration.ContainsKey(item.PropertyName.ToLower()))
                {
                    _configuration.Add(item.PropertyName.ToLower(), item.PropertyValue);
                }
            }
        }


        public static string GetSettingProperty(string key)
        {
            if (_configuration.ContainsKey(key.ToLower()))
            {
                return _configuration[key.ToLower()];
            }
            else
            {
                // try to load it from the config
                var configValue = new DataHelper().GetConfiguration().FirstOrDefault(x => x.PropertyName.ToLower() == key.ToLower());
                if (configValue != null)
                {
                    _configuration.Add(configValue.PropertyName.ToLower(), configValue.PropertyValue.ToLower());
                    return configValue.PropertyValue;
                }

                // cant find it in DB either
                return string.Empty;
            }
        }



        public static ulong GetGuildId()
        {
            if (!DEBUG)
            {
                return Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
            }
            else
            {
                return 887751832426930177; //Dev Discord Id
            }
        }
    }
}
