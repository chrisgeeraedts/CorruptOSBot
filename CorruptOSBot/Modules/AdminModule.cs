using CorruptOSBot.Helpers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("poll")]
        [Summary("(your question) - Creates a yes/no poll.")]
        public async Task SayPollAsync([Remainder]string pollquestion)
        {
            if (RootAdminManager.GetToggleState("poll") &&
                (RootAdminManager.HasSpecificRole(Context.User, "Staff") ||
                RootAdminManager.HasSpecificRole(Context.User, "Moderator")))
            {
                var currentUser = ((SocketGuildUser)Context.User);
                if (!string.IsNullOrEmpty(currentUser.Nickname))
                {
                    string title = string.Format("{0} has started a poll", currentUser.Nickname);
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
            }
        }


        [Command("clear")]
        [Summary("(number) - Clears posts above it. (max 100)")]
        public async Task SayClearAsync(int number)
        { 
            var hasDevRole = ((SocketGuildUser)Context.User).Roles.Any(x => x.Name == "Developer");
            if (RootAdminManager.GetToggleState("clear") && hasDevRole)
            {
                // max it 
                if (number > 100)
                {
                    number = 100;
                }
                var messages = await Context.Channel.GetMessagesAsync(number + 1).FlattenAsync();
                await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
            }
        }

        [Command("toggle")]
        [Summary("(command string) - Toggles a command to be available.")]
        public async Task SayTogglecommandAsync(string command)
        {
            var hasDevRole = ((SocketGuildUser)Context.User).Roles.Any(x => x.Name == "Developer");

            if (hasDevRole && RootAdminManager.GetCommandExist(command))
            {
                var currentState = RootAdminManager.GetToggleState(command);
                RootAdminManager.ToggleModuleCommand(command, !currentState);
                var newState = RootAdminManager.GetToggleState(command);
                await ReplyAsync(string.Format("command {0} was toggled from {1} to {2}", command, currentState, newState));
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Command("togglestates")]
        [Summary("Shows the current enabled and disabled commands")]
        public async Task SaytogglestatescommandAsync()
        {
            var hasDevRole = ((SocketGuildUser)Context.User).Roles.Any(x => x.Name == "Developer");

            if (hasDevRole)
            {
                var builder = new EmbedBuilder();
                builder.Color = Color.Blue;
                builder.Description = "Shows the current enabled and disabled commands, services and interceptors";
                builder.Title = "Toggle states";

                foreach (var item in RootAdminManager.GetToggleStates())
                {
                    builder.AddField(item.Key, item.Value, true);
                }

                await ReplyAsync(embed: builder.Build());
            }
            // delete the command posted
            await Context.Message.DeleteAsync();
        }
    }
}
