using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers.PVM;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Text = string.Format("{0} Yeah | {1} Nah - {2}", "👍", "👎", DateTime.Now.ToString("r"))
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
                Text = string.Format("Do not reply via text - {0}", DateTime.Now.ToString("r"))
            };
            builder.Description = message;
            return builder.Build();
        }


        public static Embed CreateWOMEmbed()
        {
            var client = new WiseOldManClient();
            var clanId = ConfigHelper.GetSettingProperty("WOMClanId");

            var clan = client.GetClan();

            if (clan != null)
            {
                var builder = new EmbedBuilder();
                builder.Color = Color.Blue;
                builder.Title = "Affliction | Wise Old Man";
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

                if (recentAchievements != null)
                {
                    sb2.AppendLine("\u200b");
                    foreach (var recentAchievement in recentAchievements.Take(3))
                    {
                        sb2.AppendLine(string.Format("**{0}** - {1}", recentAchievement.player.displayName, recentAchievement.name));
                    }
                    sb2.AppendLine("\u200b");

                    builder.Fields.Add(new EmbedFieldBuilder().WithIsInline(true).WithName("Recent achievements").WithValue(sb2).WithIsInline(false));

                    builder.Fields.Add(new EmbedFieldBuilder().WithIsInline(true).WithName("Homeworld").WithValue(clan.homeworld).WithIsInline(true));
                    builder.Fields.Add(new EmbedFieldBuilder().WithIsInline(true).WithName("Members").WithValue(clan.memberCount).WithIsInline(true));

                    builder.WithFooter(string.Format("Last updated: {0}", DateTime.Now.ToString("r")));
                    return builder.Build();
                }                
            }
            return null;
        }

        public static async Task<Embed> CreateFullLeaderboardEmbed(int triggerTimeInMS)
        {
            // connect the kcs per boss
            var result = await BossKCHelper.GetTopBossKC(WOMMemoryCache.OneDayMS);

            var builder = new EmbedBuilder();
            builder.Color = Color.DarkGreen;

            BuildSet(result.Take(15), builder);
            BuildSet(result.Skip(15).Take(15), builder);
            BuildSet(result.Skip(30).Take(15), builder);

            builder.Title = "Top boss KC";
            builder.WithFooter(string.Format("Last updated: {0} | next update at: {1}", DateTime.Now, DateTime.Now.Add(TimeSpan.FromMilliseconds(triggerTimeInMS))));

            return builder.Build();
        }


        public static void BuildSet(IEnumerable<KcTopList> result, EmbedBuilder builder)
        {
            // First column
            var sb = new StringBuilder();
            foreach (var item in result)
            {
                if (item.KcPlayers.Count() > 0)
                {
                    sb.AppendLine(string.Format("{0} {1} {2} **({3})**", EmojiHelper.GetFullEmojiString(item.Boss.ToString()), "\U0001f947", item.KcPlayers.Skip(0).First().Player, item.KcPlayers.Skip(0).First().Kc));
                }
                else
                {
                    sb.AppendLine(string.Format("{0} {1} {2}", EmojiHelper.GetFullEmojiString(item.Boss.ToString()), "\U0001f947", "---"));
                }
            }
            builder.AddField("\u200b", sb.ToString(), true);

            // Second column
            var sb2 = new StringBuilder();
            foreach (var item in result)
            {
                if (item.KcPlayers.Count() > 1)
                {
                    sb2.AppendLine(string.Format("{0} {1} ({2})", "\U0001f948", item.KcPlayers.Skip(1).First().Player, item.KcPlayers.Skip(1).First().Kc));
                }
                else
                {
                    sb2.AppendLine(string.Format("{0} {1}", "\U0001f948", "---"));
                }
            }
            builder.AddField("\u200b", sb2.ToString(), true);

            // Second column
            var sb3 = new StringBuilder();
            foreach (var item in result)
            {
                if (item.KcPlayers.Count() > 2)
                {
                    sb3.AppendLine(string.Format("{0} {1} ({2})", "\U0001f949", item.KcPlayers.Skip(2).First().Player, item.KcPlayers.Skip(2).First().Kc));
                }
                else
                {
                    sb3.AppendLine(string.Format("{0} {1}", "\U0001f949", "---"));
                }

            }
            builder.AddField("\u200b", sb3.ToString(), true);
        }
    }
}
