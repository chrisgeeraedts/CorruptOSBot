using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class ScoreModule : ModuleBase<SocketCommandContext>
    {
        [Command("score")]
        [Summary("Generates a leaderboard for the current SOTW event.")]
        public async Task SayScoreAsync()
        {
            if (RootAdminManager.GetToggleState("score") && RootAdminManager.HasAnyRole(Context.User))
            {
                // load current event
                // First, get all comps
                var comps = new WiseOldManClient().GetClanCompetitions();

                // Filter on comps that have started and arent finished yet
                var f1 = comps.Where(x => x.startsAt < DateTime.Now && x.endsAt > DateTime.Now);

                if (f1.Any())
                {
                    // get the last one in that list
                    var f2 = comps.OrderBy(x => x.id).Last();

                    // get details of this comp
                    CompetitionDetail detailedComp = new WiseOldManClient().GetCompetition(f2.id);

                    // create embed with data
                    var embedBuilder = new EmbedBuilder();
                    embedBuilder.Color = Color.Gold;
                    embedBuilder.Title = f2.title;
                    embedBuilder.Url = string.Format("https://wiseoldman.net/competitions/{0}", f2.id);
                    string s = detailedComp.totalGained >= 10000 ? detailedComp.totalGained.ToString("n0") : detailedComp.totalGained.ToString("d");
                    embedBuilder.Description = string.Format("**Total XP: {0}**", s);
                    
                    AddFields(embedBuilder, detailedComp.participants);

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

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }


        [Command("endscore")]
        [Summary("Generates a leaderboard for the last completed event.")]
        public async Task SayEndScoreAsync()
        {
            if (RootAdminManager.GetToggleState("endscore") && RootAdminManager.HasSpecificRole(Context.User, "Staff"))
            {
                // load current event
                // First, get all comps
                var comps = new WiseOldManClient().GetClanCompetitions();

                // get the last one in that list
                var f2 = comps.Where(x => x.endsAt < DateTime.Now).OrderBy(x => x.id).Last();

                // get details of this comp
                CompetitionDetail detailedComp = new WiseOldManClient().GetCompetition(f2.id);

                // create embed with data
                var embedBuilder = new EmbedBuilder();
                embedBuilder.Color = Color.Gold;
                embedBuilder.Title = f2.title;
                embedBuilder.Url = string.Format("https://wiseoldman.net/competitions/{0}", f2.id);
                string s = detailedComp.totalGained >= 10000 ? detailedComp.totalGained.ToString("n0") : detailedComp.totalGained.ToString("d");
                embedBuilder.Description = string.Format("**Total XP: {0}**", s);

                // get top 3 partipants
                AddFields(embedBuilder, detailedComp.participants);

                await ReplyAsync(string.Format("**{0}** has ended", detailedComp.title), embed: embedBuilder.Build());

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Command("endscore")]
        [Summary("(eventid) Generates a leaderboard for the last completed event.")]
        public async Task SayEndScoreAsync(int compId)
        {
            if (RootAdminManager.GetToggleState("endscore") && RootAdminManager.HasSpecificRole(Context.User, "Staff"))
            {
                // get details of this comp
                CompetitionDetail detailedComp = new WiseOldManClient().GetCompetition(compId);

                if (detailedComp != null)
                {
                    // create embed with data
                    var embedBuilder = new EmbedBuilder();
                    embedBuilder.Color = Color.Gold;
                    embedBuilder.Title = detailedComp.title;
                    embedBuilder.Url = string.Format("https://wiseoldman.net/competitions/{0}", compId);
                    string s = detailedComp.totalGained >= 10000 ? detailedComp.totalGained.ToString("n0") : detailedComp.totalGained.ToString("d");
                    embedBuilder.Description = string.Format("**Total XP: {0}**", s);

                    AddFields(embedBuilder, detailedComp.participants);

                    await ReplyAsync(string.Format("**{0}** has ended", detailedComp.title), embed: embedBuilder.Build());
                }
                

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
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


