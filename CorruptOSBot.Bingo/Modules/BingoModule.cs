using CorruptOSBot.Shared;
using CorruptOSBot.Bingo;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using CorruptOSBot.Shared.Helpers.Discord;

namespace CorruptOSBot.Modules
{
    public class BingoModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Admin)]
        [Command("bingo-score")]
        [Summary("!bingo-score - Retrieves your teams current score")]
        public async Task SayBingoScoreAsync()
        {
            if (ToggleStateManager.GetToggleState("bingo-score", Context.User) &&
                RoleHelper.HasRole(Context.User, Context.Guild, 3)) //bot dev
            {
                
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }




        [Helpgroup(HelpGroup.Admin)]
        [Command("bingo-setteams")]
        [Summary("!bingo-setteams - Retrieves your teams current score")]
        public async Task SayBingoSetTeamsAsync()
        {
            if (ToggleStateManager.GetToggleState("bingo-setteams", Context.User) &&
                RoleHelper.HasRole(Context.User, Context.Guild, 3)) //bot dev
            {
                // Retrieves teams from WOM and update DB
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }
    }
}
