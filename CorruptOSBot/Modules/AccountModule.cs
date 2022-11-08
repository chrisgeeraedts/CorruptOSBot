using CorruptOSBot.Data;
using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class AccountModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("add-alt")]
        [Summary("!add-alt {rsn} - Adds an runescape alt account to your discord account")]
        public async Task SayAddAltBAsync([Remainder] string rsn)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "spam-bot-commands", Context.User))
            {
                if (ToggleStateManager.GetToggleState("add-alt", Context.User))
                {
                    await AddAlt(Context, rsn, "alt");
                }
            }
            else
            {
                await DiscordHelper.NotAllowedMessageToUser(Context, "!add-alt", "spam-bot-commands");
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("add-iron")]
        [Summary("!add-iron {rsn} - Adds an runescape iron account to your discord account")]
        public async Task SayAddIronAsync([Remainder] string rsn)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "spam-bot-commands", Context.User))
            {
                if (ToggleStateManager.GetToggleState("add-iron", Context.User))
                {
                    await AddAlt(Context, rsn, "iron");
                }
            }
            else
            {
                await DiscordHelper.NotAllowedMessageToUser(Context, "!add-iron", "spam-bot-commands");
            }


            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("force-add-alt")]
        [Summary("!force-add-alt {discorduser} {rsn} - Adds an runescape alt account to the given discord account")]
        public async Task SayForceAddAltBAsync(string discordUsername, string rsn)
        {
            if (ToggleStateManager.GetToggleState("force-add-alt", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                DiscordUser discordUser = null;

                using (CorruptModel corruptosEntities = new Data.CorruptModel())
                {
                    discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.Username.ToLower() == discordUsername.ToLower());
                }

                if (discordUser != null)
                {
                    await AddAlt(Context, rsn, "alt", discordUser.DiscordId);
                }
                else
                {
                    var message = await ReplyAsync(string.Format("That discord user (**{0}**) is **not found**", discordUsername));
                    await Task.Delay(5000).ContinueWith(t => message.DeleteAsync());
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("force-add-iron")]
        [Summary("!force-add-iron {discorduser} {rsn} - Adds an runescape alt account to the given discord account")]
        public async Task SayForceAddIronBAsync(string discordUsername, string rsn)
        {
            if (ToggleStateManager.GetToggleState("force-add-alt", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                DiscordUser discordUser = null;

                using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
                {
                    discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.Username.ToLower() == discordUsername.ToLower());
                }

                if (discordUser != null)
                {
                    await AddAlt(Context, rsn, "iron", discordUser.DiscordId);
                }
                else
                {
                    var message = await ReplyAsync(string.Format("That discord user (**{0}**) is **not found**", discordUsername));
                    await Task.Delay(5000).ContinueWith(t => message.DeleteAsync());
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("change-alt")]
        [Summary("!change-alt {old name} {new name} - changes an existing alt name from the old to a new one")]
        public async Task SayChangeAltBAsync(string oldName, string newName)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "spam-bot-commands", Context.User))
            {
                if (ToggleStateManager.GetToggleState("change-alt", Context.User))
                {
                    await ChangeAlt(oldName, newName);
                }
            }
            else
            {
                await DiscordHelper.NotAllowedMessageToUser(Context, "!change-alt", "spam-bot-commands");
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("force-change-alt")]
        [Summary("!force-change-alt {old name} {new name} - changes an existing alt name from the old to a new one")]
        public async Task SayForceChangeAltBAsync(string oldName, string newName)
        {
            if (ToggleStateManager.GetToggleState("force-change-alt", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                await ChangeAlt(oldName, newName, true);
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("delete-alt")]
        [Summary("!delete-alt {rsn} - Removes this runescape account from your discord account")]
        public async Task SayDeleteAltBAsync([Remainder] string rsn)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "spam-bot-commands", Context.User))
            {
                if (ToggleStateManager.GetToggleState("delete-alt", Context.User))
                {
                    await DeleteAlt(Context, rsn);
                }
            }
            else
            {
                await DiscordHelper.NotAllowedMessageToUser(Context, "!delete-alt", "spam-bot-commands");
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("force-delete-account")]
        [Summary("!force-delete-account {rsn} - Removes this runescape account from the given discord account")]
        public async Task SayForceDeleteAltBAsync(string rsn)
        {
            if (ToggleStateManager.GetToggleState("force-delete-account", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                await DeleteAlt(Context, rsn, true);
            }

            await Context.Message.DeleteAsync();
        }

        private async Task AddAlt(SocketCommandContext context, string rsn, string type, long? overrideDiscordUserId = null)
        {
            try
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    // Find discord dataset
                    long discordId = overrideDiscordUserId ?? Convert.ToInt64(context.User.Id);
                    var discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == discordId);

                    var isRsnAlreadyLinked = corruptosEntities.RunescapeAccounts.Any(x =>
                    x.rsn.ToLower() == rsn.ToLower() &&
                    x.DiscordUser != null &&
                    x.DiscordUserId != discordUser.Id);
                    // check if account is linked to rsn
                    if (discordUser != null)
                    {
                        if (isRsnAlreadyLinked)
                        {
                            await context.User.SendMessageAsync(String.Format("The RSN **{0}** is already linked to an Affliction Discord account!", rsn));
                        }
                        else if (discordUser.RunescapeAccounts.Any(x => x.rsn.ToLower() == rsn.ToLower()))
                        {
                            if (!overrideDiscordUserId.HasValue)
                            {
                                // message if it does
                                await context.User.SendMessageAsync(String.Format("You already have linked the RSN **{0}** to your Affliction Discord account!", rsn));
                            }
                        }
                        else
                        {
                            // add it to WOM
                            //var client = new WiseOldManClient();
                            //var addedClanMember = client.AddGroupMember(rsn);

                            //if (addedClanMember == null)
                            //{
                            //    // could be null as its already a member, try to load normally
                            //    addedClanMember = client.SearchUsersByName(rsn).FirstOrDefault();
                            //}

                            // add it to db
                            corruptosEntities.RunescapeAccounts.Add(new Data.RunescapeAccount()
                            {
                                DiscordUser = discordUser,
                                rsn = rsn,
                                //wom_id = addedClanMember?.id,
                                account_type = type,
                            });

                            if (!overrideDiscordUserId.HasValue)
                            {
                                // add and message if it doesnt
                                await context.User.SendMessageAsync(String.Format("**{0}** was linked to your Affliction Discord account!", rsn));
                            }

                            var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
                            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed:
                                EmbedHelper.CreateDefaultEmbed(string.Format("Member added {0}", type),
                                string.Format("**{0}** added '**{1}**' as their **{2}**", discordUser.Username, rsn, type)));
                        }
                    }

                    // Save
                    await corruptosEntities.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "AddAlt", "Failed adding alt to database - " + e.Message));
            }
        }

        private async Task DeleteAlt(SocketCommandContext context, string rsn, bool forceOverride = false)
        {
            try
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    // Find discord dataset
                    RunescapeAccount runescapeAccountDB = null;
                    long discordId = Convert.ToInt64(context.User.Id);
                    var discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == discordId);

                    if (!forceOverride)
                    {
                        var trimmedRsn = rsn.Trim(' ').ToLower();
                        runescapeAccountDB = corruptosEntities.RunescapeAccounts.FirstOrDefault(x =>
                        x.rsn.ToLower() == trimmedRsn &&
                        x.DiscordUser != null && x.DiscordUserId == discordUser.Id);
                    }
                    else
                    {
                        var trimmedRsn = rsn.Trim(' ').ToLower();
                        var foo = corruptosEntities.RunescapeAccounts.ToList();
                        runescapeAccountDB = corruptosEntities.RunescapeAccounts.FirstOrDefault(
                            x => x.rsn.ToLower() == trimmedRsn);
                    }

                    // check if account is linked to rsn
                    if (runescapeAccountDB == null)
                    {
                        await context.User.SendMessageAsync($"The RSN **{rsn}** is not linked to your Affliction Discord account!");
                    }
                    else
                    {
                        // remove it from WOM
                        //var client = new WiseOldManClient();
                        //client.RemoveGroupMember(rsn);

                        corruptosEntities.RunescapeAccounts.Remove(runescapeAccountDB);

                        if (!forceOverride)
                        {
                            // add and message if it doesnt
                            await context.User.SendMessageAsync(String.Format("**{0}** was removed from your Affliction Discord account!", rsn));

                            var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
                            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed:
                                EmbedHelper.CreateDefaultEmbed(string.Format("Runescape account was removed from Discord user"),
                                string.Format(" **'{1}'** was removed from **{0}**", discordUser.Username, rsn)));
                        }
                        else
                        {
                            var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
                            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed:
                                EmbedHelper.CreateDefaultEmbed(string.Format("Runescape account was removed from Discord user"),
                                string.Format(" **'{1}'** was removed from **{0}**", runescapeAccountDB.DiscordUser.Username, rsn)));
                        }
                    }

                    // Save
                    await corruptosEntities.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "AddAlt", "Failed adding alt to database - " + e.Message));
            }
        }

        private async Task ChangeAlt(string oldName, string newName, bool forceOverride = false)
        {
            try
            {
                // change in database
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    // ensure this account is owner of the rsn
                    var discordId = Convert.ToInt64(Context.User.Id);

                    var isOwner = corruptosEntities.RunescapeAccounts.Any(x =>
                    x.DiscordUser.DiscordId == discordId &&
                    x.rsn.ToLower() == oldName.ToLower());

                    if (forceOverride || isOwner)
                    {
                        RunescapeAccount rsAccount = null;
                        if (isOwner)
                        {
                            rsAccount = corruptosEntities.RunescapeAccounts.FirstOrDefault(x =>
                            x.DiscordUser.DiscordId == discordId &&
                            x.rsn.ToLower() == oldName.ToLower());
                        }
                        else if (forceOverride)
                        {
                            rsAccount = corruptosEntities.RunescapeAccounts.FirstOrDefault(x =>
                            x.rsn.ToLower() == oldName.ToLower());
                        }

                        if (rsAccount != null)
                        {
                            // we found the rs account, so change it
                            rsAccount.rsn = newName;

                            // change in WOM
                            var client = new WiseOldManClient();
                            var result = client.PostNameChange(oldName, newName);

                            if (string.IsNullOrEmpty(result))
                            {
                                // add and message if it doesnt
                                if (!forceOverride)
                                {
                                    await Context.User.SendMessageAsync(String.Format("Your runescape account linked to the Affliction Discord account was changed from **{0}** to **{1}**", oldName, newName));
                                }

                                var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
                                await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed:
                                    EmbedHelper.CreateDefaultEmbed(string.Format("Member changed {0}", oldName),
                                    string.Format("The runescape account '**{0}**' was renamed to '**{1}**'", oldName, newName)));

                                corruptosEntities.SaveChanges();
                            }
                            else
                            {
                                await Context.User.SendMessageAsync(String.Format("Name change failed: {0}", result));
                                await Program.Log(new LogMessage(LogSeverity.Error, "ChangeAlt", String.Format("Name change failed: {0} - {1} => {2}", result, oldName, newName)));
                            }
                        }
                        else
                        {
                            await Context.User.SendMessageAsync(String.Format("The RSN **{0}** is not found!", oldName));
                        }
                    }
                    else
                    {
                        // message if it does
                        await Context.User.SendMessageAsync(String.Format("This RSN (**{0}**) is not linked to your Affliction Discord Account!", oldName));
                    }
                }
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "ChangeAlt", "Failed adding alt to database - " + e.Message));
            }
        }
    }
}