using Discord.Commands;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class InviteModule : ModuleBase<SocketCommandContext>
    {
        [Command("invite")]
        [Summary("Generates an invite link.")]
        public Task SayInviteAsync()
            => ReplyAsync("Placeholder for [invite] content");

        [Command("inv")]
        [Summary("Generates an invite link.")]
        public Task SayInvAsync()
            => ReplyAsync("Placeholder for [invite] content");

    }
}
