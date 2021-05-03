using CorruptOSBot.Data;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {

        [Helpgroup(HelpGroup.Admin)]
        [Command("initdb")]
        [Summary("!initdb - initializes the DB")]
        public async Task SayInitDBAsync()
        {
            if (ToggleStateManager.GetToggleState("initdb", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
            {
                await InitDB(Context);
            }
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("poll")]
        [Summary("!poll {your question} - Creates a yes/no poll.")]
        public async Task SayPollAsync([Remainder]string pollquestion)
        {
            if (ToggleStateManager.GetToggleState("poll", Context.User) &&
                (PermissionManager.HasSpecificRole(Context.User, "Staff") ||
                PermissionManager.HasSpecificRole(Context.User, "Moderator")))
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

        [Helpgroup(HelpGroup.Admin)]
        [Command("postid")]
        [Summary("!postid - Gets the current post's Id")]
        public async Task SaypostidAsync()
        {
            if (ToggleStateManager.GetToggleState("postid", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
            {

                var messages = await Context.Channel
                   .GetMessagesAsync(Context.Message, Direction.Before, 1)
                   .FlattenAsync();

                var message = messages.FirstOrDefault();
                if (message != null)
                {
                    var messageId = ((IUserMessage)message).Id;

                    await ReplyAsync(messageId.ToString());
                }



                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("channelid")]
        [Summary("!channelid - Gets the current channel's Id")]
        public async Task SaychannelIdAsync()
        {
            if (ToggleStateManager.GetToggleState("channelid", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
            {
                var channel = Context.Channel.Id.ToString();

                await ReplyAsync(channel);

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("guildid")]
        [Summary("!guildid - Gets the current guild Id")]
        public async Task SayguildidAsync()
        {
            if (ToggleStateManager.GetToggleState("guildid", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
            {
                var channel = Context.Guild.Id.ToString();

                await ReplyAsync(channel);

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("serverip")]
        [Summary("!serverip - Gets the current server's IP")]
        public async Task SayServerIdAsync()
        {
            if (ToggleStateManager.GetToggleState("serverip", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
                foreach (var item in ipHostInfo.AddressList)
                {
                    var serverIp = item.ToString();
                    await ReplyAsync(string.Format("{0}", serverIp));
                }


                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("clear")]
        [Summary("!clear {number} - Clears posts above it. (max 100)")]
        public async Task SayClearAsync(int number)
        { 
            if (ToggleStateManager.GetToggleState("clear", Context.User) && 
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

        [Helpgroup(HelpGroup.Admin)]
        [Command("toggle")]
        [Summary("!toggle {command string} - Toggles a command to be available.")]
        public async Task SayTogglecommandAsync(string command)
        {   
            if (DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
            {
                var currentState = ToggleStateManager.GetToggleState(command);
                await ToggleStateManager.ToggleModuleCommand(command, !currentState);
                var newState = ToggleStateManager.GetToggleState(command);
                await ReplyAsync(string.Format("command {0} was toggled from {1} to {2}", command, currentState, newState));
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("togglestates")]
        [Summary("!togglestates - Shows the current enabled and disabled commands")]
        public async Task SaytogglestatescommandAsync()
        {
            if (DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
            {
                var states = ToggleStateManager.GetToggleStates();

                var builder = new EmbedBuilder();
                builder.Color = Color.Blue;
                builder.WithFooter("Shows the current enabled and disabled commands, services and interceptors");
                builder.Title = "Toggle states";
                var sb = new StringBuilder();

                foreach (var item in states.Take(25))
                {
                    sb.AppendLine(string.Format("**{0}**: {1}", item.Functionality, item.Toggled));
                }
                builder.Description = sb.ToString();

                await ReplyAsync(embed: builder.Build());
            }
            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("getusers")]
        [Summary("!getusers - Gets all users on discord, showing their name or nickname (if set). This can be split up in multiple messages in order to comply with the 2000 character length cap on discord.")]
        public async Task SayGetUsersAsync()
        {
            if (ToggleStateManager.GetToggleState("getusers", Context.User) &&
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

        [Helpgroup(HelpGroup.Moderator)]
        [Command("getuser")]
        [Summary("!getuser {username}(optional) - Gets a single users on discord, showing their available information.")]
        public async Task SayGetUserAsync([Remainder]string username)
        {
            if (ToggleStateManager.GetToggleState("getuser", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Staff") ||
                DiscordHelper.HasRole(Context.User, Context.Guild, "Moderator"))
            {
                try
                {
                    IGuildUser user;
                    if (!username.StartsWith("<@!"))
                    {
                        user = await DiscordHelper.AsyncFindUserByName(username, Context);
                    }
                    else
                    {
                        user = await DiscordHelper.AsyncFindUserByMention(username, Context);
                    }

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

        private async Task InitDB(SocketCommandContext context)
        {
            CorruptOSBot.Data.CorruptModel corruptosEntities = new Data.CorruptModel();

            // Clear current data
            foreach (var item in corruptosEntities.DiscordUsers)
            {
                corruptosEntities.DiscordUsers.Remove(item);
            }
            foreach (var item in corruptosEntities.Bosses)
            {
                corruptosEntities.Bosses.Remove(item);
            }
            foreach (var item in corruptosEntities.RunescapeAccounts)
            {
                corruptosEntities.RunescapeAccounts.Remove(item);
            }

            // Add players
            var countDiscordUsers = await AddPlayersAndDiscordUsers(corruptosEntities);
            await ReplyAsync(string.Format("loaded {0} discord users", countDiscordUsers));

            // Save
            await corruptosEntities.SaveChangesAsync();
            await ReplyAsync(string.Format("Saved all entities to database"));
        }

        private async Task<int> AddPlayersAndDiscordUsers(Data.CorruptModel corruptosEntities)
        {
            var result = 0;
            await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneHourMS);
            var guildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
            var guild = ((Discord.IDiscordClient)Context.Client).GetGuildAsync(guildId).Result;
            // iterate through all discord users
            var allUsers = guild.GetUsersAsync().Result;

            foreach (var user in allUsers)
            {
                if (!user.IsBot && !user.IsWebhook)
                {
                    var discordUser = new Data.DiscordUser()
                    {
                        DiscordId = Convert.ToInt64(user.Id),
                        Username = DiscordHelper.GetAccountNameOrNickname(user),
                        OriginallyJoinedAt = user.JoinedAt?.DateTime
                    };

                    corruptosEntities.DiscordUsers.Add(discordUser);

                    var womIdentity = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.username.ToLower() == DiscordHelper.GetAccountNameOrNickname(user).ToLower());
                    corruptosEntities.RunescapeAccounts.Add(new Data.RunescapeAccount()
                    {
                        DiscordUser = discordUser,
                        rsn = DiscordHelper.GetAccountNameOrNickname(user),
                        wom_id = womIdentity?.id

                    });
                    result++;
                }
            }
            return result;
        }
               
        private EmbedBuilder BuildEmbedForUserInfo(IGuildUser user)
        {
            var embedBuilder = new EmbedBuilder();
            embedBuilder.Title = DiscordHelper.GetAccountNameOrNickname(user);

            var rsAccounts = new List<RunescapeAccount>();
            var isBlackListed = false;
            using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
            {
                var userId = Convert.ToInt64(user.Id);
                rsAccounts = corruptosEntities.RunescapeAccounts.Where(x => x.DiscordUser.DiscordId == userId).ToList();
                var discorduser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == userId);
                if (discorduser != null)
                {
                    isBlackListed = discorduser.BlacklistedForPromotion;
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("**ID:** {0}", user.Id));
            sb.AppendLine(string.Format("**Name:** {0}", user.Username));
            sb.AppendLine(string.Format("**Nickname:** {0}", user.Nickname));
            sb.AppendLine(string.Format("**Status:** {0}", user.Status));
            sb.AppendLine(string.Format("**Promotion Blacklist:** {0}", isBlackListed));


            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("**Roles:**");

            foreach (var roleId in user.RoleIds)
            {
                sb.AppendLine(string.Format("- {0}", Context.Guild.GetRole(roleId).Name).Replace("@", string.Empty));
            }


            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("**Additional:**");
            sb.AppendLine(string.Format("**Created at:** {0}", user.CreatedAt.ToString("r")));
            sb.AppendLine(string.Format("**Joined at:** {0}", user.JoinedAt?.ToString("r")));
            sb.AppendLine(string.Format("**Premium since:** {0}", user.PremiumSince));
            sb.AppendLine(string.Format("**Webhook:** {0}", user.IsWebhook));
            sb.AppendLine(string.Format("**Bot:** {0}", user.IsBot));


            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("**RS Accounts:**");

            if (rsAccounts.Any())
            {
                foreach (var rsAccount in rsAccounts)
                {
                    sb.AppendLine(string.Format("**{0}** : {1}", rsAccount.account_type, rsAccount.rsn));
                }
            }
            else
            {
                sb.AppendLine("< no runescape accounts >");
            }

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

        [Command("overthrownathan")]
        [Summary("Prepare!")]
        public async Task SayOverthrowNathanAsync()
        {
            if (ToggleStateManager.GetToggleState("overthrownathan", Context.User))
            {
                var message = await Context.Channel.SendMessageAsync("**Now is not yet the time...** | this message will selfdestruct in 5 seconds... ;)");

                await Context.Message.DeleteAsync();
                await Task.Delay(5000).ContinueWith(t => message.DeleteAsync());
            }
        }
    }
}
