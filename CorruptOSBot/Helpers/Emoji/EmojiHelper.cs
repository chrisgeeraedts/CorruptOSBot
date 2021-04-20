using CorruptOSBot.Helpers.Bot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CorruptOSBot.Helpers
{
    public static class EmojiHelper
    {
        private static Dictionary<EmojiEnum, string> _emojiStrings;
        private static Regex rxNonDigits = new Regex(@"[^\d]+");

        public static string GetEmojiString(EmojiEnum emoji)
        {

            fillEmojiIfNeeded();

            if (_emojiStrings.ContainsKey(emoji))
            {
                return _emojiStrings[emoji];
            }
            return string.Empty;
        }

        public static string GetFullEmojiString(EmojiEnum emoji)
        {
            var baseString = GetEmojiString(emoji);
            return string.Format("<{0}>", baseString);
        }

        public static ulong GetEmojiId(EmojiEnum emoji)
        {
            var baseString = GetEmojiString(emoji);
            string cleaned = rxNonDigits.Replace(baseString, "");
            return Convert.ToUInt64(cleaned);
        }

        private static void fillEmojiIfNeeded()
        {
            if (_emojiStrings == null)
            {
                _emojiStrings = new Dictionary<EmojiEnum, string>();
                foreach (EmojiEnum emojiItem in (EmojiEnum[])Enum.GetValues(typeof(EmojiEnum)))
                {
                    var propertyIdentifier = string.Format("emoji_{0}", emojiItem.ToString());
                    var emojiString = ConfigHelper.GetSettingProperty(propertyIdentifier);
                    if (!_emojiStrings.ContainsKey(emojiItem))
                    {
                        _emojiStrings.Add(emojiItem, emojiString);
                    }
                }
            }
        }
    }
}
