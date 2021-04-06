﻿using System;
using System.Configuration;

namespace CorruptOSBot.Helpers
{
    public static class ConfigHelper
    {
        public static string GetSettingProperty(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception)
            {
                //TODO manage this properly
                return string.Empty;
            }
        }
    }
}
