using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
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
        [Summary("(Staff/Mod) !poll {your question} - Creates a yes/no poll.")]
        public async Task SayPollAsync([Remainder]string pollquestion)
        {
            if (RootAdminManager.GetToggleState("poll", Context.User) &&
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
        [Summary("(Dev) !clear {number} - Clears posts above it. (max 100)")]
        public async Task SayClearAsync(int number)
        { 
            if (RootAdminManager.GetToggleState("clear", Context.User) && 
                DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
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
        [Summary("(Dev) !toggle {command string} - Toggles a command to be available.")]
        public async Task SayTogglecommandAsync(string command)
        {   
            if (DiscordHelper.HasRole(Context.User, Context.Guild, "Developer")
                && RootAdminManager.GetCommandExist(command))
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
        [Summary("(Dev) !togglestates - Shows the current enabled and disabled commands")]
        public async Task SaytogglestatescommandAsync()
        {
            if (DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
            {
                var states = RootAdminManager.GetToggleStates();

                var builder = new EmbedBuilder();
                builder.Color = Color.Blue;
                builder.WithFooter("Shows the current enabled and disabled commands, services and interceptors");
                builder.Title = "Toggle states";
                var sb = new StringBuilder();

                foreach (var item in states.Take(25))
                {
                    sb.AppendLine(string.Format("**{0}**: {1}", item.Key, item.Value));
                }
                builder.Description = sb.ToString();

                await ReplyAsync(embed: builder.Build());
            }
            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        [Command("getusers")]
        [Summary("(Dev) !getusers - Gets all users on discord, showing their name or nickname (if set). This can be split up in multiple messages in order to comply with the 2000 character length cap on discord.")]
        public async Task SayGetUsersAsync()
        {
            if (RootAdminManager.GetToggleState("getusers", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
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


        [Command("getuser")]
        [Summary("(Staff) !getuser {username}(optional) - Gets a single users on discord, showing their available information.")]
        public async Task SayGetUserAsync(IGuildUser user)
        {
            if (RootAdminManager.GetToggleState("getuser", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Staff"))
            {
                try
                {
                    await Context.Channel.SendMessageAsync(embed: BuildEmbedForUserInfo(user).Build());
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Info, nameof(SayGetUserAsync), string.Format("Failed: {0}", e.Message)));
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Command("getuser")]
        [Summary("(Staff) !getuser {username}(optional) - Gets a single users on discord, showing their available information.")]
        public async Task SayGetUserAsync(string username)
        {
            if (RootAdminManager.GetToggleState("getuser", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Staff"))
            {
                try
                {
                    var guildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
                    var guild = await ((IDiscordClient)Context.Client).GetGuildAsync(guildId);
                    var allUsers = await guild.GetUsersAsync();
                    var user = allUsers.FirstOrDefault(x => x.Nickname == username);
                    if (user != null)
                    {
                        await Context.Channel.SendMessageAsync(embed: BuildEmbedForUserInfo(user).Build());
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(string.Format("User {0} not found", username), string.Empty));
                    }
                   
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Info, nameof(SayGetUserAsync), string.Format("Failed: {0}", e.Message)));
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }



        [Command("overthrownathan")]
        [Summary("Prepare!")]
        public async Task SayOverthrowNathanAsync()
        {
            if (RootAdminManager.GetToggleState("overthrownathan", Context.User))
            {
                var currentUser = ((SocketGuildUser)Context.User);

                var message = await Context.Channel.SendMessageAsync("**Now is not yet the time...** | this message will selfdestruct in 5 seconds... ;)");

                await Context.Message.DeleteAsync();
                await Task.Delay(5000).ContinueWith(t => message.DeleteAsync());
                
            }
        }


        private EmbedBuilder BuildEmbedForUserInfo(IGuildUser user)
        {
            var embedBuilder = new EmbedBuilder();
            embedBuilder.Title = DiscordHelper.GetAccountNameOrNickname(user);

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("**ID:** {0}", user.Id));
            sb.AppendLine(string.Format("**Name:** {0}", user.Username));
            sb.AppendLine(string.Format("**Nickname:** {0}", user.Nickname));
            sb.AppendLine(string.Format("**Status:** {0}", user.Status));


            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("**Roles:**");

            foreach (var roleId in user.RoleIds)
            {
                sb.AppendLine(string.Format("- {0}", Context.Guild.GetRole(roleId).Name).Replace("@", string.Empty));
            }


            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("**Additional:**");
            sb.AppendLine(string.Format("**Created at:** {0}", user.CreatedAt));
            sb.AppendLine(string.Format("**Joined at:** {0}", user.JoinedAt));
            sb.AppendLine(string.Format("**Premium since:** {0}", user.PremiumSince));
            sb.AppendLine(string.Format("**Webhook:** {0}", user.IsWebhook));
            sb.AppendLine(string.Format("**Bot:** {0}", user.IsBot));

            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("**Activities:**");

            if (user.Activities.Any())
            {
                foreach (var activity in user.Activities)
                {
                    sb.AppendLine(string.Format("{0} : {1}", activity.Name, activity.Details));
                }
            }
            else
            {
                sb.AppendLine("< no activities >");
            }


            embedBuilder.Description = sb.ToString();
            embedBuilder.ThumbnailUrl = user.GetAvatarUrl();

            return embedBuilder;
        }
    }
}
