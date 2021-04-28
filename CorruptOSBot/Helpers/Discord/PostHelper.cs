using CorruptOSBot.Shared.Helpers.Bot;
using System;

namespace CorruptOSBot.Helpers.Discord
{
    public static class PostHelper
    {
        public static ulong GetPostId(string postName)
        {
            try
            {
                var postString = ConfigHelper.GetSettingProperty(postName);
                return Convert.ToUInt64(postString);
            }
            catch (Exception)
            {
                return ulong.MinValue;
            }
        }
    }
}
