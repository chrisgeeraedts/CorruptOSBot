using CorruptOSBot.Extensions;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static List<Embed> CreateDefaultFieldsEmbed(string title, Dictionary<string, string> fields)
        {
            var result = new List<Embed>();
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.Title = title;

            if (fields != null)
            {
                int index = 0;
                foreach (var item in fields)
                {
                    if (index < 24)
                    {
                        builder.AddField(item.Key, item.Value);
                        index++;
                    }
                    else
                    {
                        builder.AddField(item.Key, item.Value);
                        result.Add(builder.Build());
                        index = 0;

                        // create new builder
                        builder = new EmbedBuilder();
                        builder.Color = Color.Blue;
                    }
                }
                // Add the remainder
                result.Add(builder.Build());
            }

            return result;
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
                Text = string.Format("{0} Yeah | {1} Nah - {2}", "👍", "👎", DateTime.Now.ToString("dd/MM/yyyy"))
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
                Text = string.Format("Do not reply via text - {0}", DateTime.Now.ToString("dd/MM/yyyy"))
            };
            builder.Description = message;
            return builder.Build();
        }


        public static Embed CreateWOMEmbed()
        {
            var client = new WiseOldManClient();
            var clanId = ConfigHelper.GetSettingProperty("WOMClanId");

            var clan = client.GetClan();

            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.Title = "Corrupt OS | Wise Old Man";
            builder.Url = string.Format("https://wiseoldman.net/groups/{0}", clanId);
            builder.ThumbnailUrl = "https://wiseoldman.net/img/logo.png";

            var sb = new StringBuilder();
            sb.AppendLine("\u200b");
            sb.AppendLine(string.Format("- [Clan Page](https://wiseoldman.net/groups/{0})", clanId));
            sb.AppendLine(string.Format("- [Competitions](https://wiseoldman.net/groups/{0}/competitions)", clanId));
            sb.AppendLine(string.Format("- [Hiscores](https://wiseoldman.net/groups/{0}/hiscores)", clanId));
            sb.AppendLine(string.Format("- [Achievements](https://wiseoldman.net/groups/{0}/achievements)", clanId));
            builder.Fields.Add(new EmbedFieldBuilder().WithIsInline(true).WithName("Links").WithValue(sb.ToString()).WithIsInline(false));

            var sb2 = new StringBuilder();
            var recentAchievements = client.GetClanRecentAchievements();
            sb2.AppendLine("\u200b");
            foreach (var recentAchievement in recentAchievements.Take(3))
            {
                sb2.AppendLine(string.Format("**{0}** - {1}", recentAchievement.player.displayName, recentAchievement.name));
            }
            sb2.AppendLine("\u200b");

            builder.Fields.Add(new EmbedFieldBuilder().WithIsInline(true).WithName("Recent achievements").WithValue(sb2).WithIsInline(false));

            builder.Fields.Add(new EmbedFieldBuilder().WithIsInline(true).WithName("Homeworld").WithValue(clan.homeworld).WithIsInline(true));
            builder.Fields.Add(new EmbedFieldBuilder().WithIsInline(true).WithName("Members").WithValue(clan.memberCount).WithIsInline(true));

            builder.WithFooter(string.Format("Last updated: {0}", DateTime.Now.ToString("dd/MM/yyyy")));

            return builder.Build();
        }
    }
}
