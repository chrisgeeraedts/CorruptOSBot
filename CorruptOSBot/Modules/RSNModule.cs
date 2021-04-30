using CorruptOSBot.Extensions;
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
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class RSNModule : ModuleBase<SocketCommandContext>
    {
        [Command("rsn")]
        [Summary("!rsn {your name} - changes your nickname in the server and Wise Old Man.")]
        public async Task SayRSNAsync([Remainder]string username)
        {
            if (ToggleStateManager.GetToggleState("rsn", Context.User))
            {
                //user that actually started the command
                var currentUser = Context.User;

                // the prefered rsn name the user posted
                var preferedNickname = username;

                // check rank, if he has a smiley, its a namechange, otherwise, a new member
                var hasSmileyRole = ((SocketGuildUser)currentUser).Roles.Any(x => x.Name == "Smiley");

                if (!hasSmileyRole)
                {
                    await CreateNewMember(currentUser, preferedNickname);
                }
                else
                {
                    await NameChangeMember(currentUser, preferedNickname);
                }
            }
        }


        [Command("rsncf")]
        [Summary("!rsncf {your name} - changes your nickname in the server and sets you as a Clanfriend.")]
        public async Task SayRSNCFAsync([Remainder]string username)
        {
            if (ToggleStateManager.GetToggleState("rsncf", Context.User) &&
                DiscordHelper.IsInChannel(Context.Channel.Id, "welcome", Context.User))
            {
                //user that actually started the command
                var currentUser = Context.User;

                // the prefered rsn name the user posted
                var preferedNickname = username;

                // check rank, if he has a smiley, its a namechange, otherwise, a new member
                var hasClanFriendRole = ((SocketGuildUser)currentUser).Roles.Any(x => x.Name == "Clan Friend");

                if (!hasClanFriendRole)
                {
                    await CreateNewClanFriendMember(currentUser, preferedNickname);
                }
                else
                {
                    await currentUser.SendMessageAsync(String.Format("You already have the {0} role!", "Clan Friend"));
                }
            }
        }

        private async Task NameChangeMember(SocketUser currentUser, string preferedNickname)
        {
            string previousName = ((SocketGuildUser)currentUser).Nickname;

            // change nickname
            await ((SocketGuildUser)currentUser).ModifyAsync(x =>
            {
                x.Nickname = preferedNickname;
            });

            // post to recruiting channel
            var recruitingChannel =  Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member name change",
                string.Format("{0} has changed their name to <@{1}>", previousName, ((SocketGuildUser)currentUser).Id)));

            // update WOM
            new WiseOldManClient().PostNameChange(previousName, preferedNickname);

            try
            {
                using(var model = new Data.CorruptModel())
                {
                    // get the rsn account
                    var rsaccount = model.RunescapeAccounts.FirstOrDefault(x => x.rsn.ToLower() == previousName);

                    // make sure he is owner
                    var userId = Convert.ToInt64(currentUser.Id);
                    if (rsaccount.DiscordUser != null && rsaccount.DiscordUserId == userId)
                    {
                        // change name
                        rsaccount.rsn = preferedNickname;
                        await model.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "RSNModule", "Failed to save discorduser in discord - " + e.Message));
            }

            // delete the command posted
            await Context.Message.DeleteAsync();           
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
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Smiley");
                await ((SocketGuildUser)currentUser).AddRoleAsync(role);

                // change nickname
                await ((SocketGuildUser)currentUser).ModifyAsync(x =>
                {
                    x.Nickname = preferedNickname;
                });

                // post to general channel
                var generalChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("general"));
                await ((IMessageChannel)generalChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member joined", 
                    string.Format("<@{0}> Welcome to Corrupt OS", currentUser.Id)));

                // post to recruitment channel
                var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
                await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: 
                    EmbedHelper.CreateDefaultEmbed("Member joined",
                    string.Format("<@{0}>  ({0}) has set their RSN to **{1}**!", currentUser.Id, preferedNickname)));

                // add to WOM
                var groupMember = new WiseOldManClient().AddGroupMember(preferedNickname);

                // send welcome message
                await DiscordHelper.SendWelcomeMessageToUser(Context.User, Context.Guild, false);

                try
                {
                    await DataHelper.AddNewDiscordUserAndRSN(Context.User, preferedNickname, groupMember);
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Error, "RSNModule", "Failed to save discorduser in discord - " + e.Message));
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
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

        private async Task CreateNewClanFriendMember(SocketUser currentUser, string preferedNickname)
        {
            // check runewatch
            var runewatchEntries = new RunewatchClient().GetRunewatchEntries();

            var runewatchEntry = runewatchEntries.FirstOrDefault(x => x.accused_rsn == preferedNickname);
            bool isSafeAccount = runewatchEntry == null;

            if (isSafeAccount)
            {
                // update the role
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Clan Friend");
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

                // delete the command posted
                await Context.Message.DeleteAsync();
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
