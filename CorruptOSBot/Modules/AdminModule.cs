﻿using CorruptOSBot.Extensions.WOM;
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

        [Command("initdb")]
        [Summary("(Dev) !initdb - initializes the DB")]
        public async Task SayInitDBAsync()
        {
            if (ToggleStateManager.GetToggleState("initdb", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Developer"))
            {
                await InitDB(Context);
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

            // Add bosses
            var countBosses = FillBosses(corruptosEntities);
            await ReplyAsync(string.Format("loaded {0} bosses", countBosses));

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

        private static int FillBosses(Data.CorruptModel corruptosEntities)
        {
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.sire.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.sire) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.sire.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.sire) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.bryophyta.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.bryophyta) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.chamber.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.chamber) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.fanatic.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.fanatic) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.prime.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.prime) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.crazyarc.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.crazyarc) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.mole.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.mole) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.kq.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.kq) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.kree.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.kree) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.nightmare.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.nightmare) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.scorpia.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.scorpia) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.gaunt.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.gaunt) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.thermy.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.thermy) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.venny.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.venny) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.todt.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.todt) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.hydra.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.hydra) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.callisto.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.callisto) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.chambercm.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.chambercm) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.sara.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.sara) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.rex.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.rex) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.derangedarc.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.derangedarc) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.gargs.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.gargs) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.kbd.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.kbd) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.kril.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.kril) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.obor.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.obor) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.skotizo.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.skotizo) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.corruptgaunt.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.corruptgaunt) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.zuk.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.zuk) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.vetion.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.vetion) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.zalcano.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.zalcano) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.barrows.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.barrows) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.cerb.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.cerb) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.chaosele.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.chaosele) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.corp.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.corp) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.supreme.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.supreme) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.bandos.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.bandos) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.hespori.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.hespori) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.kraken.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.kraken) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.mimic.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.mimic) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.sarachnis.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.sarachnis) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.tempor.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.tempor) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.tob.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.tob) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.jad.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.jad) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.vorkath.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.vorkath) });
            corruptosEntities.Bosses.Add(new Data.Boss() { Bossname = EmojiEnum.zulrah.ToString(), EmojiName = EmojiHelper.GetFullEmojiString(EmojiEnum.zulrah) });

            return corruptosEntities.Bosses.Count();
        }

        [Command("poll")]
        [Summary("(Staff/Mod) !poll {your question} - Creates a yes/no poll.")]
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

        [Command("postid")]
        [Summary("(admin) Gets the current post's Id")]
        public async Task SaypostidAsync()
        {
            if (ToggleStateManager.GetToggleState("postid", Context.User))
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

        [Command("channelid")]
        [Summary("(admin) Gets the current channel's Id")]
        public async Task SaychannelIdAsync()
        {
            if (ToggleStateManager.GetToggleState("channelid", Context.User))
            {
                var channel = Context.Channel.Id.ToString();

                await ReplyAsync(channel);

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }


        [Command("guildid")]
        [Summary("(admin) Gets the current guild Id")]
        public async Task SayguildidAsync()
        {
            if (ToggleStateManager.GetToggleState("guildid", Context.User))
            {
                var channel = Context.Guild.Id.ToString();

                await ReplyAsync(channel);

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Command("serverip")]
        [Summary("(admin) Gets the current server's IP")]
        public async Task SayServerIdAsync()
        {
            if (ToggleStateManager.GetToggleState("serverip", Context.User))
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


        [Command("clear")]
        [Summary("(Dev) !clear {number} - Clears posts above it. (max 100)")]
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

        [Command("toggle")]
        [Summary("(Dev) !toggle {command string} - Toggles a command to be available.")]
        public async Task SayTogglecommandAsync(string command)
        {   
            if (DiscordHelper.HasRole(Context.User, Context.Guild, "Developer")
                && RootAdminManager.GetCommandExist(command))
            {
                var currentState = ToggleStateManager.GetToggleState(command);
                ToggleStateManager.ToggleModuleCommand(command, !currentState);
                var newState = ToggleStateManager.GetToggleState(command);
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
                var states = ToggleStateManager.GetToggleStates();

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

        [Command("getuser")]
        [Summary("(Staff) !getuser {username}(optional) - Gets a single users on discord, showing their available information.")]
        public async Task SayGetUserAsync([Remainder]string username)
        {
            if (ToggleStateManager.GetToggleState("getuser", Context.User) &&
                DiscordHelper.HasRole(Context.User, Context.Guild, "Staff"))
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
