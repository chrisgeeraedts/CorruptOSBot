using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class WoMModule : ModuleBase<SocketCommandContext>
    {
        [Command("wom")]
        [Summary("!wom - Generates links to our Wise Old Man clan page.")]
        public async Task SayWoMAsync()
        {
            if (RootAdminManager.GetToggleState("wom", Context.User) && RootAdminManager.HasAnyRole(Context.User))
            {
                var clan = new WiseOldManClient().GetClan();

                var builder = new EmbedBuilder();
                builder.Color = Color.Blue;
                builder.Title = "Wise Old Man";
                builder.Url = "https://wiseoldman.net/groups/128";
                builder.ThumbnailUrl = "https://wiseoldman.net/img/logo.png";
                builder.Description = "Corrupt OS";
                builder.Fields.Add(new EmbedFieldBuilder().WithIsInline(true).WithName("Homeworld").WithValue(clan.homeworld));
                builder.Fields.Add(new EmbedFieldBuilder().WithIsInline(true).WithName("Members").WithValue(clan.memberCount));
                builder.Fields.Add(new EmbedFieldBuilder().WithIsInline(true).WithName("Score").WithValue(clan.score));
                await ReplyAsync(embed: builder.Build());
            }
        }

    }
}


