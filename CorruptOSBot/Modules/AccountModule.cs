using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
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
            if (ToggleStateManager.GetToggleState("add-alt", Context.User))
            {
                await AddAlt(Context, rsn, "alt");
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }
        
        [Helpgroup(HelpGroup.Member)]
        [Command("add-iron")]
        [Summary("!add-iron {rsn} - Adds an runescape iron account to your discord account")]
        public async Task SayAddIronAsync([Remainder] string rsn)
        {
            if (ToggleStateManager.GetToggleState("add-iron", Context.User))
            {
                await AddAlt(Context, rsn, "iron");
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        [Helpgroup(HelpGroup.Member)]
        [Command("change-alt")]
        [Summary("!change-alt {old name}|{new name} - changes an existing alt name from the old to a new one")]
        public async Task SayChangeAltBAsync([Remainder] string commandString)
        {
            if (ToggleStateManager.GetToggleState("change-alt", Context.User))
            {
                // grab actual props
                Dictionary<string, string> commands;
                if (TryGetEntireCommand(commandString, new string[] { "oldName", "newName" }, '|', out commands))
                {
                    var oldName = commands["oldName"];
                    var newName = commands["newName"];

                    // change in database
                    using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
                    {
                        // ensure this account is owner of the rsn
                        var discordId = Context.User.Id;
                        var isOwner = corruptosEntities.RunescapeAccounts.Any(x =>
                        x.DiscordUser.DiscordId == Convert.ToInt64(discordId) &&
                        x.rsn == oldName);

                        if (isOwner)
                        {
                            var rsAccount = corruptosEntities.RunescapeAccounts.FirstOrDefault(x =>
                                x.DiscordUser.DiscordId == Convert.ToInt64(discordId) &&
                                x.rsn == oldName);

                            if (rsAccount != null)
                            {
                                // we found the rs account, so change it
                                rsAccount.rsn = newName;
                            }
                            else
                            {
                                // we didnt found it, add it
                                await AddAlt(Context, newName, "alt");
                            }
                                                                                 
                            // change in WOM
                            var client = new WiseOldManClient();
                            client.PostNameChange(oldName, newName);

                            corruptosEntities.SaveChanges();
                        }
                    }
                }
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        private async Task AddAlt(SocketCommandContext context, string rsn, string type)
        {
            try
            {
                using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
                {
                    // Find discord dataset
                    long discordId = Convert.ToInt64(context.User.Id);
                    var discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == discordId);

                    var isRsnAlreadyLinked = corruptosEntities.RunescapeAccounts.Any(x => x.rsn == rsn && x.DiscordUser != null && x.DiscordUserId != discordUser.DiscordId);
                    // check if account is linked to rsn
                    if (discordUser != null)
                    {
                        if (isRsnAlreadyLinked)
                        {
                            await context.User.SendMessageAsync(String.Format("The RSN **{0}** is already linked to an an Corrupt OS Discord account!", rsn));
                        }
                        else if (discordUser.RunescapeAccounts.Any(x => x.rsn == rsn))
                        {
                            // message if it does
                            await context.User.SendMessageAsync(String.Format("You already have linked the RSN **{0}** to your Corrupt OS Discord account!", rsn));
                        }
                        else
                        {
                            // add it to WOM                
                            var client = new WiseOldManClient();
                            var addedClanMember = client.AddGroupMember(rsn);

                            if (addedClanMember == null)
                            {
                                // could be null as its already a member, try to load normally
                                addedClanMember = client.SearchUsersByName(rsn).FirstOrDefault();
                            }

                            // add it to db
                            corruptosEntities.RunescapeAccounts.Add(new Data.RunescapeAccount()
                            {
                                DiscordUser = discordUser,
                                rsn = rsn,
                                wom_id = addedClanMember?.id,
                                account_type = type,
                            });

                            // add and message if it doesnt
                            await context.User.SendMessageAsync(String.Format("**{0}** was linked to your Corrupt OS Discord account!", rsn));

                            var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
                            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed:
                                EmbedHelper.CreateDefaultEmbed(string.Format("Member added {0}", type),
                                string.Format("{0} added '{1}' as their {2}", discordUser.Username, rsn, type)));
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



        private bool TryGetEntireCommand(string commandString, string[] commandKeys, char splitter, out Dictionary<string, string> result)
        {
            result = new Dictionary<string, string>();
            var splittedString = commandString.Split(splitter);
            var index = 0;
            foreach (var item in splittedString)
            {
                if (commandKeys.Length >= index)
                {
                    var key = commandKeys[index];
                    var value = item;
                    result.Add(key, value);
                }
                index++;
            }
            return true;
        }
    }
}
