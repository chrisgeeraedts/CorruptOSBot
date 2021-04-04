using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Helpers
{
    public static class EmbedHelper
    {
        public static Embed CreateDefaultEmbed(string title, string url, Dictionary<string, string> fields = null)
        {
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.Title = title;
            builder.Url = url;
            builder.Footer = new EmbedFooterBuilder() {Text = "footer", IconUrl = "" };

            if (fields != null )
            {
                foreach (var item in fields)
                {
                    builder.AddField(item.Key, item.Value);
                }
            }

            return builder.Build();
        }

        public static Embed CreateDefaultEmbed(string title, string message)
        {
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.Title = title;
            builder.Description = message;
            return builder.Build();
        }
    }
}
