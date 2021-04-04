using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    [RequireOwner(Group = "Staff")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("poll")]
        [Summary("(your question) - Creates a yes/no poll.")]
        public Task SayPollAsync(string playerName)
            => ReplyAsync("Placeholder for [poll] content");


        [Command("clear")]
        [Summary("(number) - Clears posts above it.")]
        public Task SayClearAsync(int number)
            => ReplyAsync("Placeholder for [clear] content");


        [Command("whois")]
        [Summary("@member - Gives detailed info on a member in the server")]
        public async Task SayWhoisAsync(SocketUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }


        [Command("info")]
        [Summary("Displays information about the server/bot")]
        public Task SayInfoAsync()
            => ReplyAsync("Placeholder for [whois] content");
    }
}
