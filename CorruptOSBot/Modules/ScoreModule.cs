using CorruptOSBot.Extensions;
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
            // load current event



            await ReplyAsync("Placeholder for [score] content");

        }






        //private string GetContent()
        //{
        //    Clan clan = new WiseOldManClient().GetClan(128);
            
        //    Competition comp =
        //        new WiseOldManClient().GetCompetitions(2085);
        //}

    }
}
