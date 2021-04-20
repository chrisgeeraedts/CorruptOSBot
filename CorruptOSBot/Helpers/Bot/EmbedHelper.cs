using Discord;
using System;
using System.Collections.Generic;

namespace CorruptOSBot.Helpers.Bot
{
    public static class EmbedHelper
    {
        public static Embed CreateDefaultFieldsEmbed(string title, string description, Dictionary<string, string> fields)
        {
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.Description = description;
            builder.Title = title;

            if (fields != null)
            {
                foreach (var item in fields)
                {
                    builder.AddField(item.Key, item.Value);
                }
            }

            return builder.Build();
        }
        public static Embed CreateDefaultFieldsEmbed(string title, Dictionary<string, string> fields)
        {
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.Title = title;

            if (fields != null)
            {
                foreach (var item in fields)
                {
                    builder.AddField(item.Key, item.Value);
                }
            }

            return builder.Build();
        }

        public static Embed CreateDefaultEmbed(string title, string message, string imageUrl = null, string thumbnailUrl = null)
        {
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.Title = title;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                builder.ImageUrl = imageUrl;
            }

            if (!string.IsNullOrEmpty(thumbnailUrl))
            {
                builder.ThumbnailUrl = thumbnailUrl;
            }
            builder.Description = message;
            return builder.Build();
        }

        public static Embed CreateDefaultPollEmbed(string title, string message)
        {
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.ThumbnailUrl = Constants.PollIcon;
            builder.Title = title;
            builder.Footer = new EmbedFooterBuilder()
            {
                Text = string.Format("{0} Yeah | {1} Nah - {2}", "👍", "👎", DateTime.Now.ToString("MM/dd/yyyy"))
            };
            builder.Description = message;
            return builder.Build();
        }

        public static Embed CreateDefaultSuggestionEmbed(string title, string message, string imageUri)
        {
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.ThumbnailUrl = imageUri;
            builder.Title = title;
            builder.Footer = new EmbedFooterBuilder()
            {
                Text = string.Format("Do not reply via text - {0}", DateTime.Now.ToString("MM/dd/yyyy"))
            };
            builder.Description = message;
            return builder.Build();
        }
    }
}
