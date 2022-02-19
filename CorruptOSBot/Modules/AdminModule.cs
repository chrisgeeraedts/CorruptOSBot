using CorruptOSBot.Data;
using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using CorruptOSBot.Shared.Helpers.Discord;
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
            if (ToggleStateManager.GetToggleState("initdb", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                await InitDB(Context);
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("poll")]
        [Summary("!poll {your question} - Creates a yes/no poll.")]
        public async Task SayPollAsync([Remainder] string pollquestion)
        {
            if (ToggleStateManager.GetToggleState("poll", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                var currentUser = (SocketGuildUser)Context.User;
                var name = DiscordNameHelper.GetAccountNameOrNickname(currentUser);

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
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("postid")]
        [Summary("!postid - Gets the current post's Id")]
        public async Task SaypostidAsync()
        {
            if (ToggleStateManager.GetToggleState("postid", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
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
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("channelid")]
        [Summary("!channelid - Gets the current channel's Id")]
        public async Task SaychannelIdAsync()
        {
            if (ToggleStateManager.GetToggleState("channelid", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                await ReplyAsync(Context.Channel.Id.ToString());
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("guildid")]
        [Summary("!guildid - Gets the current guild Id")]
        public async Task SayguildidAsync()
        {
            if (ToggleStateManager.GetToggleState("guildid", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                await ReplyAsync(Context.Guild.Id.ToString());
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("serverip")]
        [Summary("!serverip - Gets the current server's IP")]
        public async Task SayServerIdAsync()
        {
            if (ToggleStateManager.GetToggleState("serverip", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
                foreach (var item in ipHostInfo.AddressList)
                {
                    var serverIp = item.ToString();
                    await ReplyAsync(string.Format("{0}", serverIp));
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("clear")]
        [Summary("!clear {number} - Clears posts above it. (max 10)")]
        public async Task SayClearAsync(int number)
        {
            if (ToggleStateManager.GetToggleState("clear", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                // Limit at 10, Thanks Zeke
                if (number > 10)
                {
                    number = 10;
                }

                var messages = await Context.Channel.GetMessagesAsync(number + 1).FlattenAsync();

                try
                {
                    await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
                }
                catch (Exception ex)
                {
                    await Program.Log(new LogMessage(LogSeverity.Error, "AdminModule", "Clear command issue - " + ex.Message.ToString()));
                    await Context.Message.DeleteAsync();
                }
            }
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("toggle")]
        [Summary("!toggle {command string} - Toggles a command to be available.")]
        public async Task SayToggleCommandAsync(string command)
        {
            if (RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                var currentState = ToggleStateManager.GetToggleState(command);
                await ToggleStateManager.ToggleModuleCommand(command, !currentState);
                var newState = ToggleStateManager.GetToggleState(command);
                await ReplyAsync(string.Format("command {0} was toggled from {1} to {2}", command, currentState, newState));
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("toggles")]
        [Summary("!toggles - Shows the current enabled and disabled commands")]
        public async Task SayToggleStatesCommandAsync()
        {
            if (RoleHelper.IsStaff(Context.User, Context.Guild))
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

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("getusers")]
        [Summary("!getusers - Gets all users on discord, showing their name or nickname (if set). This can be split up in multiple messages in order to comply with the 2000 character length cap on discord.")]
        public async Task SayGetUsersAsync()
        {
            if (ToggleStateManager.GetToggleState("getusers", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                try
                {
                    StringBuilder builder = new StringBuilder();

                    DiscordHelper.DiscordUsers = new List<string>();
                    var guildId = ConfigHelper.GetGuildId();
                    var guild = await ((IDiscordClient)Context.Client).GetGuildAsync(guildId);
                    var allUsers = await guild.GetUsersAsync();
                    foreach (var discordUser in allUsers.Where(x => !x.IsBot && !x.IsWebhook))
                    {
                        DiscordHelper.DiscordUsers.Add(DiscordNameHelper.GetAccountNameOrNickname(discordUser));
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
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("getuser")]
        [Summary("!getuser {username}(optional) - Gets a single user on discord, showing their available information.")]
        public async Task SayGetUserAsync([Remainder] string username)
        {
            if (ToggleStateManager.GetToggleState("getuser", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                try
                {
                    IGuildUser user;
                    if (!username.StartsWith("<@!"))
                    {
                        //first, try to find the user based on the base username
                        user = await DiscordHelper.AsyncFindUserByName(username, Context);

                        // if we cant find it, then load all the alts and try with them
                        if (user == null)
                        {
                            using (CorruptModel corruptosEntities = new Data.CorruptModel())
                            {
                                var alt = corruptosEntities.RunescapeAccounts.FirstOrDefault(x => x.rsn.ToLower() == username.ToLower());
                                if (alt != null && alt.DiscordUser != null)
                                {
                                    user = await DiscordHelper.AsyncFindUserByName(alt.DiscordUser.Username, Context);
                                }
                            }
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed($"User:{username} not found into database.", string.Empty));
                        }
                    }
                    else
                    {
                        user = await DiscordHelper.AsyncFindUserByMention(username, Context);
                    }

                    if (user != null)
                    {
                        await Context.Channel.SendMessageAsync(embed: BuildEmbedForUserInfo(user, true).Build());
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
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("getuser")]
        [Summary("!getuser - Displays info about user who send command")]
        public async Task SayGetUserSelfAsync()
        {
            if (ToggleStateManager.GetToggleState("getuser", Context.User) && DiscordHelper.IsInChannel(Context.Channel.Id, "spam-bot-command", Context.User))
            {
                try
                {
                    IGuildUser user = (IGuildUser)Context.User;
                    await Context.Channel.SendMessageAsync(embed: BuildEmbedForUserInfo(user).Build());
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Info, nameof(SayGetUserAsync), string.Format("Failed: {0}", e.Message)));
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("compare")]
        [Summary("!compare - compares discordusers and database discord users")]
        public async Task SayCompareAsync()
        {
            if (ToggleStateManager.GetToggleState("compare", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                // grab discordusers
                var guildId = ConfigHelper.GetGuildId();
                var guild = ((IDiscordClient)Context.Client).GetGuildAsync(guildId).Result;
                var discordUsers = guild.GetUsersAsync().Result;

                // grab database discord suers
                var databaseDiscordUsers = new List<DiscordUser>();
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    databaseDiscordUsers = corruptosEntities.DiscordUsers.Where(x => x.LeavingDate == null).ToList();
                }

                // compare the discord users vs the database
                var comparisonList = GetComparisonList(discordUsers, databaseDiscordUsers);

                var sb = new StringBuilder();
                var eb = new EmbedBuilder();

                var embeds = new List<Embed>();

                int iterator = 0;
                foreach (var item in comparisonList.Where(x => x.ExistsInDB == false || x.ExistsInDiscord == false))
                {
                    if (iterator < 20)
                    {
                        // keep adding
                        sb.AppendLine(string.Format("**{0}** | DB:**{1}** | DISC:**{2}** | {3}", item.DiscordId, item.ExistsInDB, item.ExistsInDiscord, item.UsernameInDiscord ?? item.UsernameInDB));
                        iterator++;
                    }
                    else
                    {
                        // add final then build and add embed
                        sb.AppendLine(string.Format("**{0}** | DB:**{1}** | DISC:**{2}** | {3}", item.DiscordId, item.ExistsInDB, item.ExistsInDiscord, item.UsernameInDiscord ?? item.UsernameInDB));
                        eb.Description = sb.ToString();

                        embeds.Add(eb.Build());

                        sb = new StringBuilder();
                        eb = new EmbedBuilder();

                        iterator = 0;
                    }
                }

                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    foreach (var item in comparisonList.Where(x => x.ExistsInDB == false || x.ExistsInDiscord == false))
                    {
                        var discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.Username == item.UsernameInDB);
                        if (discordUser != null)
                        {
                            discordUser.LeavingDate = new DateTime(2021, 5, 24, 20, 0, 0);
                        }
                    }
                    corruptosEntities.SaveChanges();
                }

                // add the final rows
                eb.Description = sb.ToString();
                embeds.Add(eb.Build());

                foreach (var item in embeds)
                {
                    // reply the list
                    await ReplyAsync(embed: item);
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("fullcompare")]
        [Summary("!fullcompare - compares discordusers and database discord users")]
        public async Task SayCompare2Async()
        {
            if (ToggleStateManager.GetToggleState("compare", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                // compare the discord users vs the database
                var comparisonList = GetFullComparisonList();

                var sb = new StringBuilder();
                var eb = new EmbedBuilder();

                var embeds = new List<Embed>();

                int iterator = 0;
                foreach (var item in comparisonList.Values.Where(x => !x.IsInBoth))
                {
                    if (iterator < 20)
                    {
                        // keep adding
                        sb.AppendLine(string.Format("**{0}** | DB:**{1}** | DISC:**{2}** | WOM:**{3}**", item.Username, item.ExistsInDB, item.ExistsInDiscord, item.ExistsInWOM));
                        iterator++;
                    }
                    else
                    {
                        // add final then build and add embed
                        sb.AppendLine(string.Format("**{0}** | DB:**{1}** | DISC:**{2}** | WOM:**{3}**", item.Username, item.ExistsInDB, item.ExistsInDiscord, item.ExistsInWOM));
                        eb.Description = sb.ToString();

                        embeds.Add(eb.Build());

                        sb = new StringBuilder();
                        eb = new EmbedBuilder();

                        iterator = 0;
                    }
                }

                // add the final rows
                eb.Description = sb.ToString();
                embeds.Add(eb.Build());

                foreach (var item in embeds)
                {
                    // reply the list
                    await ReplyAsync(embed: item);
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("post", false)]
        [Summary("!post {message} - posts message contents from the bot")]
        public async Task PostMessage([Remainder] string message)
        {
            if (RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                await Context.Channel.SendMessageAsync(message);
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("dev", false)]
        [Summary("!dev - Dev command")]
        public async Task Dev()
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "clan-bot", Context.User) && Context.User.Id == SettingsConstants.GMKirbyDiscordId )
            {
                //await Context.Channel.SendMessageAsync(embed: await EmbedHelper.CreateFullLeaderboardEmbed(1000));
            }

            await Context.Message.DeleteAsync();
        }

        private List<ComparisonResult> GetComparisonList(IReadOnlyCollection<IGuildUser> discordUsers, List<DiscordUser> databaseDiscordUsers)
        {
            var comparisonList = new List<ComparisonResult>();
            foreach (var item in discordUsers.Where(x => !x.IsBot && !x.IsWebhook))
            {
                // check if exists in DB
                var userId = Convert.ToInt64(item.Id);
                var dbUser = databaseDiscordUsers.FirstOrDefault(x => x.DiscordId == userId);
                if (dbUser != null)
                {
                    comparisonList.Add(new ComparisonResult()
                    {
                        DiscordId = item.Id,
                        ExistsInDB = true,
                        ExistsInDiscord = true,
                        UsernameInDB = dbUser.Username,
                        UsernameInDiscord = item.Username
                    });
                }
                else
                {
                    comparisonList.Add(new ComparisonResult()
                    {
                        DiscordId = item.Id,
                        ExistsInDB = false,
                        ExistsInDiscord = true,
                        UsernameInDB = string.Empty,
                        UsernameInDiscord = item.Username
                    });
                }
            }

            // now we check the database vs the discord users
            foreach (var item in databaseDiscordUsers)
            {
                // check if we already have handled this id
                var userId = Convert.ToUInt64(item.DiscordId);
                bool handled = comparisonList.Any(x => x.DiscordId == userId);

                if (!handled)
                {
                    // check if exists in Discord

                    var discordUser = discordUsers.FirstOrDefault(x => x.Id == userId);
                    if (discordUser != null)
                    {
                        comparisonList.Add(new ComparisonResult()
                        {
                            DiscordId = userId,
                            ExistsInDB = true,
                            ExistsInDiscord = true,
                            UsernameInDB = item.Username,
                            UsernameInDiscord = discordUser.Username
                        });
                    }
                    else
                    {
                        comparisonList.Add(new ComparisonResult()
                        {
                            DiscordId = 0,
                            ExistsInDB = true,
                            ExistsInDiscord = false,
                            UsernameInDB = item.Username,
                            UsernameInDiscord = null
                        });
                    }
                }
            }
            return comparisonList;
        }

        private Dictionary<string, FullComparisonResult> GetFullComparisonList()
        {
            var result = new Dictionary<string, FullComparisonResult>();

            var guildId = ConfigHelper.GetGuildId();
            var guild = ((IDiscordClient)Context.Client).GetGuildAsync(guildId).Result;
            var discordUsers = guild.GetUsersAsync().Result;
            AppendCompareListForDiscord(result, discordUsers);

            // grab database discord suers
            var databaseDiscordUsers = new List<DiscordUser>();
            using (CorruptModel corruptosEntities = new CorruptModel())
            {
                databaseDiscordUsers = corruptosEntities.DiscordUsers.Where(x => x.LeavingDate == null).ToList();
            }
            AppendCompareListForDatabase(result, databaseDiscordUsers);

            var clanMembers = new WiseOldManClient().GetClanMembers();
            AppendCompareListForWiseOldMan(result, clanMembers);

            return result;
        }

        private void AppendCompareListForDiscord(Dictionary<string, FullComparisonResult> result, IReadOnlyCollection<IGuildUser> discordUsers)
        {
            var filteredList = discordUsers.Where(x => !x.IsBot && !x.IsWebhook);

            foreach (var item in filteredList)
            {
                if (!result.ContainsKey(item.Username.ToLower()))
                {
                    var newEntry = new FullComparisonResult();
                    newEntry.ExistsInDiscord = true;
                    newEntry.Username = item.Username;
                    result.Add(item.Username.ToLower(), newEntry);
                }
                else
                {
                    result[item.Username.ToLower()].ExistsInDiscord = true;
                }
            }
        }

        private void AppendCompareListForDatabase(Dictionary<string, FullComparisonResult> result, List<DiscordUser> databaseDiscordUsers)
        {
            var filteredList = databaseDiscordUsers;

            foreach (var item in filteredList)
            {
                if (!result.ContainsKey(item.Username.ToLower()))
                {
                    var newEntry = new FullComparisonResult();
                    newEntry.ExistsInDB = true;
                    newEntry.Username = item.Username;
                    result.Add(item.Username.ToLower(), newEntry);
                }
                else
                {
                    result[item.Username.ToLower()].ExistsInDB = true;
                }
            }
        }

        private void AppendCompareListForWiseOldMan(Dictionary<string, FullComparisonResult> result, List<ClanMember> wiseOldManClanMembers)
        {
            var filteredList = wiseOldManClanMembers;

            foreach (var item in filteredList)
            {
                if (!result.ContainsKey(item.username.ToLower()))
                {
                    var newEntry = new FullComparisonResult();
                    newEntry.ExistsInWOM = true;
                    newEntry.Username = item.username;
                    result.Add(item.username.ToLower(), newEntry);
                }
                else
                {
                    result[item.username.ToLower()].ExistsInWOM = true;
                }
            }
        }

        private async Task InitDB(SocketCommandContext context)
        {
            using (CorruptModel corruptosEntities = new Data.CorruptModel())
            {
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
        }

        private async Task<int> AddPlayersAndDiscordUsers(CorruptModel corruptosEntities)
        {
            var result = 0;
            await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneHourMS);
            var guildId = ConfigHelper.GetGuildId();
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
                        Username = DiscordNameHelper.GetAccountNameOrNickname(user),
                        OriginallyJoinedAt = user.JoinedAt?.DateTime
                    };

                    corruptosEntities.DiscordUsers.Add(discordUser);

                    //var womIdentity = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.username.ToLower() == DiscordNameHelper.GetAccountNameOrNickname(user).ToLower());
                    corruptosEntities.RunescapeAccounts.Add(new Data.RunescapeAccount()
                    {
                        DiscordUser = discordUser,
                        rsn = DiscordNameHelper.GetAccountNameOrNickname(user),
                        //wom_id = womIdentity?.id
                    });
                    result++;
                }
            }
            return result;
        }

        private EmbedBuilder BuildEmbedForUserInfo(IGuildUser user, bool adminFormat = false)
        {
            var embedBuilder = new EmbedBuilder();
            embedBuilder.Title = DiscordNameHelper.GetAccountNameOrNickname(user);

            var rsAccounts = new List<RunescapeAccount>();
            var isBlackListed = false;
            var joinDate = user.JoinedAt?.DateTime;
            var discordUser = new DiscordUser();

            using (CorruptModel corruptosEntities = new CorruptModel())
            {
                var userId = Convert.ToInt64(user.Id);
                rsAccounts = corruptosEntities.RunescapeAccounts.Where(x => x.DiscordUser.DiscordId == userId).ToList();
                discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == userId);

                if (discordUser != null)
                {
                    isBlackListed = discordUser.BlacklistedForPromotion;
                    joinDate = discordUser.OriginallyJoinedAt;
                }
            }
            embedBuilder.Url = string.Format("https://wiseoldman.net/players/{0}", rsAccounts.FirstOrDefault(x => x.account_type == "main"));

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("**ID:** {0}", user.Id));
            sb.AppendLine(string.Format("**Name:** {0}", user.Username));
            sb.AppendLine(string.Format("**Nickname:** {0}", user.Nickname));

            if (adminFormat)
            {
                sb.AppendLine(string.Format("**Status:** {0}", user.Status));
                sb.AppendLine(string.Format("**Promotion Blacklist:** {0}", isBlackListed));
            }

            sb.AppendLine(Environment.NewLine);

            if (adminFormat)
            {
                sb.AppendLine("**Roles:**");
                foreach (var roleId in user.RoleIds)
                {
                    sb.AppendLine(string.Format("- {0}", Context.Guild.GetRole(roleId).Name).Replace("@", string.Empty));
                }
            }

            if (discordUser != null)
            {
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine("**Points:");
                sb.AppendLine($"**Current Points**: {discordUser.Points}");
            }

            sb.AppendLine(Environment.NewLine);

            if (adminFormat)
            {
                sb.AppendLine("**Additional:**");
                sb.AppendLine(string.Format("**Created at:** {0}", user.CreatedAt.ToString("r")));
                sb.AppendLine(string.Format("**Joined at:** {0}", joinDate?.ToString("r")));
                sb.AppendLine(string.Format("**Premium since:** {0}", user.PremiumSince?.ToString("r")));
                sb.AppendLine(string.Format("**Webhook:** {0}", user.IsWebhook));
                sb.AppendLine(string.Format("**Bot:** {0}", user.IsBot));
            }

            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("**RS Accounts:**");

            if (rsAccounts.Any())
            {
                foreach (var rsAccount in rsAccounts)
                {
                    var urlString = string.Format("https://wiseoldman.net/players/{0}", rsAccount.rsn).Replace(" ", "%20");
                    var uriString = string.Format("**{0}** : [{1}]({2})", rsAccount.account_type, rsAccount.rsn, urlString);
                    sb.AppendLine(uriString);
                }
            }
            else
            {
                sb.AppendLine("< no runescape accounts >");
            }

            sb.AppendLine(Environment.NewLine);

            if (adminFormat)
            {
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
            }

            embedBuilder.Description = sb.ToString();
            embedBuilder.ThumbnailUrl = user.GetAvatarUrl();

            return embedBuilder;
        }

        #region Joke Commands

        [Command("overthrownathan")]
        [Summary("Prepare!")]
        public async Task SayOverthrowNathanAsync()
        {
            await OverthrowNath();
        }

        [Command("overthrownath")]
        [Summary("Prepare!")]
        public async Task SayOverthrowNathAsync()
        {
            await OverthrowNath();
        }

        private async Task OverthrowNath()
        {
            if (ToggleStateManager.GetToggleState("overthrownathan", Context.User))
            {
                var message = await Context.Channel.SendMessageAsync("Operation Completed");

                await Context.Message.DeleteAsync();
                await Task.Delay(5000).ContinueWith(t => message.DeleteAsync());
            }
        }

        #endregion April first
    }

    public class ComparisonResult
    {
        public string UsernameInDB { get; set; }
        public string UsernameInDiscord { get; set; }
        public bool ExistsInDB { get; set; }
        public bool ExistsInDiscord { get; set; }
        public ulong? DiscordId { get; set; }
        public bool IsInBoth { get { return ExistsInDB && ExistsInDiscord; } }
    }

    public class FullComparisonResult
    {
        public string Username { get; set; }
        public bool ExistsInDB { get; set; }
        public bool ExistsInDiscord { get; set; }
        public bool ExistsInWOM { get; set; }

        public bool IsInBoth
        { get { return ExistsInDB && ExistsInDiscord && ExistsInWOM; } }
    }
}