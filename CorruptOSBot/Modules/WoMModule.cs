using Discord.Commands;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class WoMModule : ModuleBase<SocketCommandContext>
    {
        [Command("wom")]
        [Summary("Generates links to our Wise Old Man clan page.")]
        public Task SayWoMAsync()
            => ReplyAsync("Placeholder for [wom] content");

    }
}
