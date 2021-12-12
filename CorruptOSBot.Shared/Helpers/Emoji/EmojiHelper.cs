using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CorruptOSBot.Helpers
{
    public static class EmojiHelper
    {
        private static Dictionary<string, string> _emojiStrings;
        private static Regex rxNonDigits = new Regex(@"[^\d]+");

        public static string GetEmojiString(string emoji)
        {
            fillEmojiIfNeeded();

            if (_emojiStrings.ContainsKey(emoji))
            {
                return _emojiStrings[emoji];
            }
            return string.Empty;
        }

        public static string GetFullEmojiString(string emoji)
        {
            var baseString = GetEmojiString(emoji);
            return string.Format("{0}", baseString);
        }

        public static ulong GetEmojiId(string emoji)
        {
            var baseString = GetEmojiString(emoji);
            string cleaned = rxNonDigits.Replace(baseString, "");
            return Convert.ToUInt64(cleaned);
        }

        private static void fillEmojiIfNeeded()
        {
            if (_emojiStrings == null)
            {
                var bosses = new Shared.DataHelper().GetBosses();
                _emojiStrings = new Dictionary<string, string>();
                foreach (var boss in bosses)
                {
                    if (!_emojiStrings.ContainsKey(boss.BossCommand))
                    {
                        _emojiStrings.Add(boss.BossCommand, boss.EmojiName);
                    }
                }
            }
        }
    }
}