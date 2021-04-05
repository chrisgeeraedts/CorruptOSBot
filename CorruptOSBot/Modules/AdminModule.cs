using CorruptOSBot.Helpers;
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
        public async Task SayPollAsync([Remainder]string pollquestion)
        {
            string title = string.Format("{0} has started a poll", (Context.User).Username);
            string description = pollquestion;

            // Post the poll
            var sent = await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultPollEmbed(title, description));
            
            // Add thumbs up emoji
            var emojiUp = new Emoji("\uD83D\uDC4D");
            await sent.AddReactionAsync(emojiUp);

            // Add thumbs down emoji
            var emojiDown = new Emoji("\uD83D\uDC4E");
            await sent.AddReactionAsync(emojiDown);

            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        [Command("clear")]
        [Summary("(number) - Clears posts above it.")]
        public async Task SayClearAsync(int number)
        {
            var messages = await Context.Channel.GetMessagesAsync(number).FlattenAsync();
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
        }
    }
}
