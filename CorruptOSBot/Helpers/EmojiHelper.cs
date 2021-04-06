using System;
using System.Collections.Generic;
using System.Configuration;

namespace CorruptOSBot.Helpers
{
    public static class EmojiHelper
    {
        private static Dictionary<EmojiEnum, string> _emojiStrings;

        public static string GetEmojiString(EmojiEnum emoji)
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

            if (_emojiStrings.ContainsKey(emoji))
            {
                return _emojiStrings[emoji];
            }
            return string.Empty;
        }
    }


}
