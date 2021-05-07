using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class ScoreModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("score")]
        [Summary("!score - Generates a leaderboard for the current SOTW event. (Only allowed in **event-general**)")]
        public async Task SayScoreAsync()
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "event-general", Context.User))
            {
                if (ToggleStateManager.GetToggleState("score", Context.User)
                    && RootAdminManager.HasAnyRole(Context.User))
                {
                    // load current event
                    // First, get all comps
                    var comps = new WiseOldManClient().GetClanCompetitions();

                    // Filter on comps that have started and arent finished yet
                    var f1 = comps.Where(x =>
                    x.startsAt < DateTime.Now &&
                    x.endsAt > DateTime.Now);
                    // 12-04-2021 < 13-04-2021   ==> TRUE
                    // 17-04-2021 > 13-04-2021   ==> TRUE

                    if (f1.Any())
                    {
                        // get the last one in that list
                        var f2 = f1.OrderBy(x => x.id).First();

                        // get details of this comp
                        CompetitionDetail detailedComp = new WiseOldManClient().GetCompetition(f2.id);

                        // create embed with data
                        var embedBuilder = new EmbedBuilder();
                        embedBuilder.Color = Color.Green;
                        embedBuilder.Title = f2.title;
                        embedBuilder.Url = string.Format("https://wiseoldman.net/competitions/{0}", f2.id);
                        string s = detailedComp.totalGained >= 10000 ? detailedComp.totalGained.ToString("n0") : detailedComp.totalGained.ToString("d");
                        embedBuilder.Description = string.Format("**Total XP: {0}**", s);
                        embedBuilder.WithFooter(string.Format("Event runs from {0} till {1}", detailedComp.startsAt?.ToString("r"), detailedComp.endsAt?.ToString("r")));
                        embedBuilder.ImageUrl = "https://cdn.discordapp.com/attachments/790605695150063646/829015595395055616/Line_Ext.png";
                        embedBuilder.ThumbnailUrl = "https://oldschool.runescape.wiki/images/thumb/f/f2/Old_school_bond_detail.png/120px-Old_school_bond_detail.png?15aab";

                        AddFields(embedBuilder, detailedComp.participants);
                        AddImage(embedBuilder, detailedComp.title);

                        await ReplyAsync(string.Format("Current results for **{0}**", detailedComp.title), embed: embedBuilder.Build());
                    }
                    else
                    {
                        var nextComp = comps.OrderBy(x => x.id).Last();
                        var embedBuilder = new EmbedBuilder();
                        if (nextComp != null)
                        {
                            embedBuilder.Color = Color.Gold;
                            embedBuilder.Title = "No active event!";
                            embedBuilder.Url = string.Format("https://wiseoldman.net/competitions/{0}", nextComp.id);
                            embedBuilder.Description = string.Format("No active event is running at this moment. The next event is **{0}** and will start at **{1}**, which is in **{2}** hours!",
                                nextComp.title, nextComp.startsAt, Convert.ToInt32(nextComp.startsAt.Subtract(DateTime.Now).TotalHours));
                            embedBuilder.ImageUrl = Constants.EventImage;


                            await ReplyAsync(embed: embedBuilder.Build());
                        }
                        else
                        {
                            embedBuilder.Color = Color.Gold;
                            embedBuilder.Title = "No active event!";
                            embedBuilder.Description = string.Format("No active event is running at this moment.");
                            embedBuilder.ImageUrl = Constants.EventImage;
                            await ReplyAsync(embed: embedBuilder.Build());
                        }
                    }
                }
            }
            else
            {
                await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!score", "event-general");               
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("endscore")]
        [Summary("!endscore {compId}(optional) - Generates a leaderboard for the last completed event (or for the given CompId event). (Only allowed in **leaderboard**)")]
        public async Task SayEndScoreAsync()
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "leaderboard", Context.User))
            {
                if (ToggleStateManager.GetToggleState("endscore", Context.User) && PermissionManager.HasSpecificRole(Context.User, "Staff"))
                {
                    // load current event
                    // First, get all comps
                    var comps = new WiseOldManClient().GetClanCompetitions();

                    // get the last one in that list
                    var f2 = comps.Where(x => x.endsAt < DateTime.Now).OrderBy(x => x.id).Last();

                    if (f2 != null)
                    {
                        // get details of this comp
                        CompetitionDetail detailedComp = new WiseOldManClient().GetCompetition(f2.id);

                        // create embed with data
                        var embedBuilder = new EmbedBuilder();
                        embedBuilder.Color = Color.Green;
                        embedBuilder.Title = f2.title;
                        embedBuilder.Url = string.Format("https://wiseoldman.net/competitions/{0}", f2.id);
                        string s = detailedComp.totalGained >= 10000 ? detailedComp.totalGained.ToString("n0") : detailedComp.totalGained.ToString("d");
                        embedBuilder.Description = string.Format("**Total XP: {0}**", s);
                        embedBuilder.WithFooter(string.Format("Event ran from {0} till {1}", detailedComp.startsAt?.ToString("r"), detailedComp.endsAt?.ToString("r")));
                        embedBuilder.ImageUrl = "https://cdn.discordapp.com/attachments/790605695150063646/829015595395055616/Line_Ext.png";
                        embedBuilder.ThumbnailUrl = "https://oldschool.runescape.wiki/w/Trailblazer_dragon_trophy#/media/File:Trailblazer_dragon_trophy_detail.png";

                        // get top 3 partipants
                        AddFields(embedBuilder, detailedComp.participants);
                        AddImage(embedBuilder, detailedComp.title);

                        await ReplyAsync(string.Format("**{0}** has ended", detailedComp.title), embed: embedBuilder.Build());

                    }
                }
            }
            else
            {
                await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!endscore", "leaderboard");
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("endscore")]
        [Summary("!endscore {compId}(optional) - Generates a leaderboard for the last completed event (or for the given CompId event). (Only allowed in **leaderboard**)")]
        public async Task SayEndScoreAsync(int compId)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "leaderboard", Context.User))
            {
                if (ToggleStateManager.GetToggleState("endscore", Context.User) && PermissionManager.HasSpecificRole(Context.User, "Staff"))
                {
                    // get details of this comp
                    CompetitionDetail detailedComp = new WiseOldManClient().GetCompetition(compId);

                    if (detailedComp != null)
                    {
                        // create embed with data
                        var embedBuilder = new EmbedBuilder();
                        embedBuilder.Color = Color.Gold;
                        embedBuilder.Title = detailedComp.title;
                        embedBuilder.Url = string.Format("https://wiseoldman.net/competitions/{0}", detailedComp.id);
                        string s = detailedComp.totalGained >= 10000 ? detailedComp.totalGained.ToString("n0") : detailedComp.totalGained.ToString("d");
                        embedBuilder.Description = string.Format("**Total XP: {0}**", s);
                        embedBuilder.WithFooter(string.Format("Event ran from {0} till {1}", detailedComp.startsAt?.ToString("r"), detailedComp.endsAt?.ToString("r")));
                        embedBuilder.ImageUrl = "https://cdn.discordapp.com/attachments/790605695150063646/829015595395055616/Line_Ext.png";
                        embedBuilder.ThumbnailUrl = "https://oldschool.runescape.wiki/w/Trailblazer_dragon_trophy#/media/File:Trailblazer_dragon_trophy_detail.png";

                        // get top 3 partipants
                        AddFields(embedBuilder, detailedComp.participants);
                        AddImage(embedBuilder, detailedComp.title);

                        await ReplyAsync(string.Format("**{0}** has ended", detailedComp.title), embed: embedBuilder.Build());
                    }
                }
            }
            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        private void AddImage(EmbedBuilder embedBuilder, string title)
        {
            //var titleLower = title.ToLower();
            //if (titleLower.Contains("fish"))
            //{
            //  embedBuilder.ImageUrl = "https://oldschool.runescape.wiki/images/thumb/5/56/Fishing_Skill_Boss_and_Easter_Event_%281%29.jpg/600px-Fishing_Skill_Boss_and_Easter_Event_%281%29.jpg?3a727";
            //}
            //else if (titleLower.Contains("thiev"))
            //{
            //    embedBuilder.ImageUrl = "https://oldschool.runescape.wiki/images/thumb/5/5a/Rocky_pet.png/280px-Rocky_pet.png?5bf2f";
            //}
        }

        private void AddFields(EmbedBuilder embedBuilder, List<Participant> participants)
        {
            var orderedParticipants = participants.OrderByDescending(x => x.progress.gained).ToList();

            if (orderedParticipants.Count > 0)
            {
                AddField(embedBuilder, "\U0001f947", participants[0].displayName, participants[0].progress.gained);
            }
            if (orderedParticipants.Count > 1)
            {
                AddField(embedBuilder, "\U0001f948", participants[1].displayName, participants[1].progress.gained);
            }
            if (orderedParticipants.Count > 2)
            {
                AddField(embedBuilder, "\U0001f949", participants[2].displayName, participants[2].progress.gained);
            }
        }

        private void AddField(EmbedBuilder embedBuilder, string icon, string player, int score)
        {
            var sb = new StringBuilder();
            sb.AppendLine(icon);
            sb.AppendLine(string.Format("**{0}**", player));
            string s = score >= 10000 ? score.ToString("n0") : score.ToString("d");
            sb.AppendLine(s);
            embedBuilder.AddField("\u200b", sb.ToString(), true);
        }
    }
}


