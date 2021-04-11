using CorruptOSBot.Helpers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                var name = DiscordHelper.GetAccountNameOrNickname(currentUser);
                if (!string.IsNullOrEmpty(name))
                {
                    string title = string.Format("{0} has started a poll", name);
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


        [Command("getusers")]
        [Summary("(admin) Gets all users on discord, showing their name or nickname (if set). This can be split up in multiple messages in order to comply with the 2000 character length cap on discord.")]
        public async Task SayGetUsersAsync()
        {
            var hasDevRole = ((SocketGuildUser)Context.User).Roles.Any(x => x.Name == "Developer");
            if (RootAdminManager.GetToggleState("getusers") &&
                hasDevRole)
            {
                try
                {
                    StringBuilder builder = new StringBuilder();

                    DiscordHelper.DiscordUsers = new List<string>();
                    var guildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
                    var guild = await ((IDiscordClient)Context.Client).GetGuildAsync(guildId);
                    var allUsers = await guild.GetUsersAsync();
                    foreach (var discordUser in allUsers.Where(x => !x.IsBot && !x.IsWebhook))
                    {
                        DiscordHelper.DiscordUsers.Add(DiscordHelper.GetAccountNameOrNickname(discordUser));
                    }

                    foreach (var discordUser in DiscordHelper.DiscordUsers)
                    {
                        if (!string.IsNullOrEmpty(discordUser))
                        {
                            string s = string.Format("{0}", discordUser);
                            builder.AppendLine(s);
                        }

                        // ensure that we do not go over the 2k content cap
                        if (builder.ToString().Length > 1900)
                        {
                            // reset, since we can only post 2000 characters
                            await Context.Channel.SendMessageAsync(builder.ToString());
                            builder = new StringBuilder();
                        }
                    }

                    await Context.Channel.SendMessageAsync(builder.ToString());
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Info, nameof(SayGetUsersAsync), string.Format("Failed: {0}", e.Message)));
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }
    }
}
