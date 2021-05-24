using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class CalendarModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("calendar")]
        [Summary("!calendar - Shows upcoming events")]
        public async Task SayWoMAsync()
        {
            if (ToggleStateManager.GetToggleState("calendar", Context.User) && 
                RoleHelper.HasAnyRole(Context.User))
            {
                await ReplyAsync(embed: CreateCalendarEmbed());
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        private Embed CreateCalendarEmbed()
        {
            var comps = new WiseOldManClient().GetClanCompetitions();

            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddMonths(1);

            var embedBuilder = new EmbedBuilder();
            embedBuilder.Color = Color.Orange;
            embedBuilder.Title = "Event Calendar";
            embedBuilder.WithFooter(string.Format("For more information contact {2} or {3}",
                startDate.ToString("r"),
                endDate.ToString("r"),
                "@JohnFranzese",
                "@Shearted"));
            embedBuilder.ImageUrl = "https://cdn.discordapp.com/attachments/790605695150063646/829015595395055616/Line_Ext.png";
            embedBuilder.ThumbnailUrl = "https://icons.iconarchive.com/icons/martz90/circle/64/calendar-icon.png";

            var calendarItems = comps.Where(x =>
            x.startsAt >= startDate &&
            x.startsAt < endDate).ToList();

            if (calendarItems.Any())
            {
                foreach (var item in calendarItems)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(String.Format("💠 **{0}**", item.title));
                    sb.AppendLine(String.Format("*Start date:* {0}", item.startsAt));
                    sb.AppendLine(String.Format("*End date:* {0}", item.endsAt));
                    embedBuilder.AddField("\u200b", sb.ToString(), false);
                }
            }
            else
            {
                embedBuilder.AddField("\u200b", "No events for the next month!", false);
            }

            return embedBuilder.Build();
        }
    }
}


