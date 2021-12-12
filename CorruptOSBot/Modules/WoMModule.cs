using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class WoMModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("wom")]
        [Summary("!wom - Generates links to our Wise Old Man clan page.")]
        public async Task SayWoMAsync()
        {
            if (ToggleStateManager.GetToggleState("wom", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                await ReplyAsync(embed: EmbedHelper.CreateWOMEmbed());
            }

            await Context.Message.DeleteAsync();
        }
    }
}