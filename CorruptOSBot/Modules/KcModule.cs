using Discord.Commands;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class KcModule : ModuleBase<SocketCommandContext>
    {
        [Command("kc")]
        [Summary("(player name) - Generates KC's for the specified player.")]
        public Task SayKcAsync(string playerName)
            => ReplyAsync("Placeholder for [kc] content");

        [Command("mykc")]
        [Summary("Generates your own KC.")]
        public Task SayMyKcAsync(string playerName)
        => ReplyAsync("Placeholder for [mykc] content");

    }
}
