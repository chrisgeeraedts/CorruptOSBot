using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
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
            if (ToggleStateManager.GetToggleState("wom", Context.User) && RootAdminManager.HasAnyRole(Context.User))
            {
                await ReplyAsync(embed: EmbedHelper.CreateWOMEmbed());
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }

    }
}


