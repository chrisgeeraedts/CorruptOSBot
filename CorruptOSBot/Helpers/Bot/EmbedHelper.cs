﻿using CorruptOSBot.Extensions;
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
            var builder = new EmbedBuilder
            {
                Color = Color.Blue,
                Description = description,
                Title = title
            };

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

            var builder = new EmbedBuilder
            {
                Color = Color.Blue,
                Title = title
            };

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
                        builder = new EmbedBuilder
                        {
                            Color = Color.Blue
                        };
                    }
                }
                // Add the remainder
                result.Add(builder.Build());
            }

            return result;
        }

        public static Embed CreateDefaultEmbed(string title, string message, string imageUrl = null, string thumbnailUrl = null)
        {
            var builder = new EmbedBuilder
            {
                Color = Color.Blue,
                Title = title,
                ImageUrl = imageUrl ?? null,
                ThumbnailUrl = thumbnailUrl ?? null,
                Description = message
            };

            return builder.Build();
        }

        public static Embed CreateDefaultPollEmbed(string title, string message)
        {
            var builder = new EmbedBuilder
            {
                Color = Color.Blue,
                ThumbnailUrl = Constants.PollIcon,
                Title = title,
                Description = message,

                Footer = new EmbedFooterBuilder()
                {
                    Text = string.Format("{0} Yeah | {1} Nah - {2}", "👍", "👎", DateTime.Now.ToString("r"))
                }
            };
            return builder.Build();
        }

        public static Embed CreateDefaultSuggestionEmbed(string title, string message, string imageUri)
        {
            var builder = new EmbedBuilder
            {
                Color = Color.Blue,
                ThumbnailUrl = imageUri,
                Title = title,
                Description = message,
                Footer = new EmbedFooterBuilder()
                {
                    Text = string.Format("Do not reply via text - {0}", DateTime.Now.ToString("r"))
                }
            };

            return builder.Build();
        }

        public static Embed CreateWOMEmbed()
        {
            var client = new WiseOldManClient();
            var clanId = ConfigHelper.GetSettingProperty("WOMClanId");

            var clan = client.GetClan();

            if (clan != null)
            {
                var builder = new EmbedBuilder
                {
                    Color = Color.Blue,
                    Title = "Affliction | Wise Old Man",
                    Url = string.Format("https://wiseoldman.net/groups/{0}", clanId),
                    ThumbnailUrl = "https://wiseoldman.net/img/logo.png",
                };

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

        public static async Task<List<Embed>> CreateFullLeaderboardEmbed(int triggerTimeInMS)
        {
            var result = new List<Embed>();
            var bosses = await BossKCHelper.GetTopBossKC();

            var i = 0;
            var increment = 6;

            while(i < bosses.Count)
            {
                result.Add(BuildSet(bosses.Skip(i).Take(increment).ToList()));
                i += increment;
            }

            return result;
        }

        public static Embed CreateWOMEmbedSotw()
        {
            var client = new WiseOldManClient();
            var clanId = ConfigHelper.GetSettingProperty("WOMClanId");

            var clan = client.GetClan();

            if (clan != null)
            {
                var comps = new WiseOldManClient().GetClanCompetitions();

                // Filter on comps that have started and arent finished yet
                var CompList = comps.Where(x =>
                x.startsAt < DateTime.Now &&
                x.endsAt > DateTime.Now);
                // 12-04-2021 < 13-04-2021   ==> TRUE
                // 17-04-2021 > 13-04-2021   ==> TRUE

                if (CompList.Any())
                {
                    // get the last one in that list
                    var CurrentComp = CompList.OrderBy(x => x.id).First();

                    // get details of this comp
                    CompetitionDetail detailedComp = new WiseOldManClient().GetCompetition(CurrentComp.id);

                    var totalGainedString = detailedComp.participations.Sum(item => item.progress.gained).ToString("n0");

                    // create embed with data
                    var embedBuilder = new EmbedBuilder();
                    embedBuilder.Color = Color.Green;
                    embedBuilder.Title = CurrentComp.title;
                    embedBuilder.Url = string.Format("https://wiseoldman.net/competitions/{0}", CurrentComp.id);
                    embedBuilder.Description = detailedComp.title.Contains("SOTW") ? $"**Total XP: {totalGainedString}**" : $"**Total KC: {totalGainedString}**";
                    embedBuilder.WithFooter(string.Format("Event runs from {0} till {1}", detailedComp.startsAt?.ToString("r"), detailedComp.endsAt?.ToString("r")));
                    embedBuilder.ImageUrl = "https://cdn.discordapp.com/attachments/790605695150063646/829015595395055616/Line_Ext.png";
                    embedBuilder.ThumbnailUrl = GetImage(CurrentComp.metric);

                    AddFields(embedBuilder, detailedComp.participations);

                    return embedBuilder.Build();
                }
                return null;
            }
            return null;
        }

        private static Embed BuildSet(IEnumerable<KcTopList> bosses, int? triggerTimeInMS = null)
        {
            var builder = new EmbedBuilder
            {
                Color = Color.Red,
                Title = "Top boss KC",
                Description = "-------------------------------------------------------------------------------------" //To force max width of the embed
            };

            foreach (var boss in bosses)
            {
                builder.AddField(" \u200b ", $"​{EmojiHelper.GetFullEmojiString(boss.Boss.ToString())}");
                builder.AddField($" \U0001f947 {boss.KcPlayers.Skip(0).First().Player}", $"{boss.KcPlayers.Skip(0).First().Kc}", true);
                builder.AddField($" \U0001f948 {boss.KcPlayers.Skip(1).First().Player}", $"{boss.KcPlayers.Skip(1).First().Kc}", true);
                builder.AddField($" \U0001f949 {boss.KcPlayers.Skip(2).First().Player}", $"{boss.KcPlayers.Skip(2).First().Kc}", true);
            }

            if (triggerTimeInMS != null)
            {
                builder.WithFooter($"Last updated: {DateTime.Now} | next update at: {DateTime.Now.Add(TimeSpan.FromMilliseconds(triggerTimeInMS.Value))}");
            }

            return builder.Build();
        }

        private static string GetImage(string metric)
        {
            using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
            {
                var skill = corruptosEntities.Skills.FirstOrDefault(x => x.Name.ToLower() == metric.ToLower());
                if (skill != null && !string.IsNullOrEmpty(skill.Image))
                {
                    return skill.Image;
                }
            }
            return "https://www.pngkey.com/png/full/406-4068714_free-icon-score-high-score-icon-png.png";
        }

        private static void AddFields(EmbedBuilder embedBuilder, List<Participant> participants)
        {
            var orderedParticipants = participants.OrderByDescending(x => x.progress.gained).ToList();

            if (orderedParticipants.Count > 0)
            {
                AddField(embedBuilder, "\U0001f947", participants[0].player.displayName, participants[0].progress.gained);
            }
            if (orderedParticipants.Count > 1)
            {
                AddField(embedBuilder, "\U0001f948", participants[1].player.displayName, participants[1].progress.gained);
            }
            if (orderedParticipants.Count > 2)
            {
                AddField(embedBuilder, "\U0001f949", participants[2].player.displayName, participants[2].progress.gained);
            }
            if (orderedParticipants.Count > 3)
            {
                AddField(embedBuilder, participants);
            }
        }

        private static void AddField(EmbedBuilder embedBuilder, string icon, string player, double score)
        {
            var sb = new StringBuilder();
            sb.AppendLine(icon);
            sb.AppendLine(string.Format("**{0}**", player));
            string s = score >= 10000 ? Convert.ToInt32(score).ToString("n0") : Convert.ToInt32(score).ToString("d");
            sb.AppendLine(s);
            embedBuilder.AddField("\u200b", sb.ToString(), true);
        }

        private static void AddField(EmbedBuilder embedBuilder, List<Participant> participants)
        {
            var sb = new StringBuilder();

            sb.AppendLine("```");

            foreach (var participant in participants.Where(item => item.progress.gained > 0).OrderByDescending(item => item.progress.gained).Skip(3).Take(7))
            {
                sb.AppendLine($"{participants.FindIndex(item => item == participant) + 1,2}\t{participant.player.displayName,15}\t{participant.progress.gained}");
            }

            sb.AppendLine("```");

            embedBuilder.AddField("\u200b", sb.ToString(), true);
        }
    }
}