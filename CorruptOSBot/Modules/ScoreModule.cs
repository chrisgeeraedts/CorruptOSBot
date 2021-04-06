using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using Discord.Commands;
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



                await ReplyAsync("Placeholder for [score] content");
            }
        }
    }
}
