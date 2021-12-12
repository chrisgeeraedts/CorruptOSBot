using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class RSNModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Everybody)]
        [Command("rsn")]
        [Summary("!rsn {your name} - changes your nickname in the server and Wise Old Man.")]
        public async Task SayRSNAsync([Remainder] string username)
        {
            if (ToggleStateManager.GetToggleState("rsn", Context.User))
            {
                //user that actually started the command
                var currentUser = Context.User;

                // the prefered rsn name the user posted
                var preferedNickname = username;

                // check rank, if he has a proper rank, its a namechange, otherwise, a new member
                var isMember = RoleHelper.IsMember(Context.User, Context.Guild);

                if (!isMember)
                {
                    await CreateNewMember(currentUser, preferedNickname);
                }
                else
                {
                    await NameChangeMember(currentUser, preferedNickname);
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Helpgroup(HelpGroup.Everybody)]
        [Command("rsncf")]
        [Summary("!rsncf {your name} - changes your nickname in the server and sets you as a Clanfriend.")]
        public async Task SayRSNCFAsync([Remainder] string username)
        {
            if (ToggleStateManager.GetToggleState("rsncf", Context.User) &&
                DiscordHelper.IsInChannel(Context.Channel.Id, "welcome", Context.User))
            {
                var dbRole = RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 20); //Clan friend

                if (dbRole != null)
                {
                    //user that actually started the command
                    var currentUser = Context.User;

                    // the prefered rsn name the user posted
                    var preferedNickname = username;

                    // check rank, if he has a clanfriend, its a namechange, otherwise, a new member
                    var hasClanFriendRole = ((SocketGuildUser)currentUser).Roles.Any(x => x.Id == Convert.ToUInt64(dbRole.DiscordRoleId));

                    if (!hasClanFriendRole)
                    {
                        await CreateNewClanFriendMember(currentUser, preferedNickname);
                    }
                    else
                    {
                        await currentUser.SendMessageAsync(String.Format("You already have the {0} role!", dbRole.Name));
                    }
                }
                else
                {
                    await ReplyAsync("Something broke with upgrading - please contact SGNathy");
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("force-rsn")]
        [Summary("!force-rsn {your name} - changes your nickname in the server and Wise Old Man.")]
        public async Task SaForceyRSNAsync([Remainder] string commandString)
        {
            if (ToggleStateManager.GetToggleState("rsn", Context.User))
            {
                // grab actual props
                Dictionary<string, string> commands;
                if (DiscordHelper.TryGetEntireCommand(commandString, new string[] { "oldName", "newName" }, '|', out commands))
                {
                    var oldName = commands["oldName"];
                    var newName = commands["newName"];

                    using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
                    {
                        var discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.Username == oldName);

                        if (discordUser != null)
                        {
                            var discUser = Context.Guild.GetUser(Convert.ToUInt64(discordUser.DiscordId));
                            if (discUser != null)
                            {
                                await NameChangeMember(discUser, newName);
                            }
                            else
                            {
                                var message = await ReplyAsync("Discord User not found!");
                                await Task.Delay(5000).ContinueWith(t => message.DeleteAsync());
                            }
                        }
                        else
                        {
                            var message = await ReplyAsync("Discord User not found!");
                            await Task.Delay(5000).ContinueWith(t => message.DeleteAsync());
                        }
                    }
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        private async Task NameChangeMember(SocketUser currentUser, string preferedNickname, bool forced = false)
        {
            string previousName = ((SocketGuildUser)currentUser).Nickname;

            // change nickname
            await ((SocketGuildUser)currentUser).ModifyAsync(x =>
            {
                x.Nickname = preferedNickname;
            });

            // post to recruiting channel
            var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member name change",
                string.Format("{0} has changed their name to <@{1}> {2}", previousName, ((SocketGuildUser)currentUser).Id, (forced ? "(Forced by staff)" : string.Empty))));

            // update WOM
            new WiseOldManClient().PostNameChange(previousName, preferedNickname);

            try
            {
                using (var model = new Data.CorruptModel())
                {
                    // get the rsn account
                    var rsaccount = model.RunescapeAccounts.FirstOrDefault(x => x.rsn.ToLower() == previousName);

                    // make sure he is owner
                    var userId = Convert.ToInt64(currentUser.Id);
                    if (rsaccount.DiscordUser != null && rsaccount.DiscordUserId == userId)
                    {
                        // change name
                        rsaccount.rsn = preferedNickname;
                        rsaccount.DiscordUser.Username = preferedNickname;
                        await model.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "RSNModule", "Failed to save discorduser in discord - " + e.Message));
            }
        }

        private async Task CreateNewMember(SocketUser currentUser, string preferedNickname)
        {
            // check runewatch
            var runewatchEntries = new RunewatchClient().GetRunewatchEntries();

            var runewatchEntry = runewatchEntries.FirstOrDefault(x => x.accused_rsn == preferedNickname);
            bool isSafeAccount = runewatchEntry == null;

            if (isSafeAccount)
            {
                // update the role
                var dbRole = RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 7); //Recruit
                if (dbRole != null)
                {
                    // update the role
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Id == Convert.ToUInt64(dbRole.DiscordRoleId));
                    await ((SocketGuildUser)currentUser).AddRoleAsync(role);

                    // change nickname
                    await ((SocketGuildUser)currentUser).ModifyAsync(x =>
                    {
                        x.Nickname = preferedNickname;
                    });

                    // post to general channel
                    var generalChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("General"));
                    if (generalChannel != null)
                    {
                        await ((IMessageChannel)generalChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member joined",
                        string.Format("<@{0}> Welcome to Affliction", currentUser.Id),
                        "https://blog.memberclicks.com/hubfs/Onboarding_New_Members-1.jpg"));
                    }
                    else
                    {
                        await Program.Log(new LogMessage(LogSeverity.Error, "RSNModule", "Failed to find General Channel"));
                    }

                    // post to recruitment channel
                    var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
                    if (recruitingChannel != null)
                    {
                        await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed:
                            EmbedHelper.CreateDefaultEmbed("Member joined",
                            string.Format("<@{0}> ({1}) has set their RSN to **{1}**!", currentUser.Id, preferedNickname)));
                    }
                    else
                    {
                        await Program.Log(new LogMessage(LogSeverity.Error, "RSNModule", "Failed to find Changes Channel"));
                    }

                    // add to WOM
                    var groupMember = new WiseOldManClient().AddGroupMember(preferedNickname);

                    // send welcome message
                    await DiscordHelper.SendWelcomeMessageToUser(Context.User, Context.Guild, false);

                    try
                    {
                        await new Helpers.DataHelper().AddNewDiscordUserAndRSN(Context.User, preferedNickname, groupMember);
                    }
                    catch (Exception e)
                    {
                        await Program.Log(new LogMessage(LogSeverity.Error, "RSNModule", "Failed to save discorduser in discord - " + e.Message));
                    }
                }
            }
            else
            {
                // kick user
                var guildUser = Context.Guild.GetUser(Context.User.Id);
                await guildUser.KickAsync();

                // post that user got kicked due to runewatch
                // post to recruitment channel
                var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
                if (recruitingChannel != null)
                {
                    await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member kicked!",
                    string.Format("{0} joined but was on RuneWatch - Therefor kicked! Reasoning: {1} | Evidence rating: {2} | Date: {3}",
                    preferedNickname,
                    runewatchEntry.reason,
                    runewatchEntry.evidence_rating,
                    runewatchEntry.published_date),
                    null,
                    "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/32/Actions-im-kick-user-icon.png"));
                }
                else
                {
                    await Program.Log(new LogMessage(LogSeverity.Error, "RSNModule", "Failed to find Changes Channel"));
                }
            }
        }

        private async Task CreateNewClanFriendMember(SocketUser currentUser, string preferedNickname)
        {
            // check runewatch
            var runewatchEntries = new RunewatchClient().GetRunewatchEntries();

            var runewatchEntry = runewatchEntries.FirstOrDefault(x => x.accused_rsn == preferedNickname);
            bool isSafeAccount = runewatchEntry == null;

            if (isSafeAccount)
            {
                // update the role
                var dbRole = RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 20); //clan friend

                if (dbRole != null)
                {
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Id == Convert.ToUInt64(dbRole.DiscordRoleId));
                    await ((SocketGuildUser)currentUser).AddRoleAsync(role);

                    // change nickname
                    await ((SocketGuildUser)currentUser).ModifyAsync(x =>
                    {
                        x.Nickname = preferedNickname;
                    });

                    // post to recruitment channel
                    var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
                    await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed:
                        EmbedHelper.CreateDefaultEmbed("Clanfriend joined",
                        string.Format("<@{0}>  ({0}) has set their RSN to **{1}**! as a Clan Friend", currentUser.Id, preferedNickname)));

                    // send welcome message
                    await DiscordHelper.SendWelcomeMessageToUser(Context.User, Context.Guild, true);
                }
                else
                {
                    await ReplyAsync("Something broke with upgrading - please contact SGNathy");
                }
            }
            else
            {
                // kick user
                var guildUser = Context.Guild.GetUser(Context.User.Id);
                await guildUser.KickAsync();

                // post that user got kicked due to runewatch
                // post to recruitment channel
                var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
                await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member kicked!",
                    string.Format("{0} joined but was on RuneWatch - Therefor kicked! Reasoning: {1} | Evidence rating: {2} | Date: {3}",
                    preferedNickname,
                    runewatchEntry.reason,
                    runewatchEntry.evidence_rating,
                    runewatchEntry.published_date),
                    null,
                    "https://icons.iconarchive.com/icons/oxygen-icons.org/oxygen/32/Actions-im-kick-user-icon.png"));
            }
        }
    }
}