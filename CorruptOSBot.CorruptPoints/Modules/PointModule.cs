using CorruptOSBot.Data;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.CorruptPoints.Modules
{
    public class PointModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Admin)]
        [Command("cp")]
        [Summary("!cp - Shows your current amount of Corrupt Points")]
        public async Task SayCpAsync()
        {
            if (ToggleStateManager.GetToggleState("point-cp", Context.User) &&
               RoleHelper.HasRole(Context.User, Context.Guild, 3)) //bot dev
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var userId = Convert.ToInt64(Context.User.Id);
                    var currentDiscordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == userId);

                    if (currentDiscordUser != null)
                    {
                        await ReplyAsync(string.Format("**{1}**'s current CP: **{0}**", currentDiscordUser.CorruptPoints, currentDiscordUser.Username));
                    }
                }
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("cp-buy")]
        [Summary("!cp-buy {itemid}- Buys the selected item using its item ID")]
        public async Task SayCpBuyAsync(int itemId)
        {
            if (ToggleStateManager.GetToggleState("point-cp-buy", Context.User) &&
               RoleHelper.HasRole(Context.User, Context.Guild, 3)) //bot dev
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    // get itemprice
                    var item = corruptosEntities.PointStores.FirstOrDefault(x => x.Id == itemId);

                    // get user and check if enough points
                    var userId = Convert.ToInt64(Context.User.Id);
                    var currentDiscordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == userId);
                    var enoughPoints = false;
                    if (currentDiscordUser != null && item != null)
                    {
                        enoughPoints = currentDiscordUser.CorruptPoints >= item.StoreItemValue;
                    }

                    if (enoughPoints)
                    {
                        // check if we dont already have that item
                        var prevPointMutation = corruptosEntities.PointMutations.Any(x => x.TargetPlayerId == currentDiscordUser.Id && x.PointStoreItemId == item.Id);

                        if (!prevPointMutation)
                        {
                            // add the mutation
                            corruptosEntities.PointMutations.Add(new PointMutation()
                            {
                                PointChange = -item.StoreItemValue,
                                PointStore = item,
                                DateTime = DateTime.Now,
                                DiscordUser = currentDiscordUser
                            });

                            // substract the points
                            currentDiscordUser.CorruptPoints -= item.StoreItemValue;

                            // give the item
                            GiveRewardItem(item);

                            await corruptosEntities.SaveChangesAsync();

                            // Send user message
                            await ((SocketGuildUser)Context.User).SendMessageAsync(string.Format("Transaction completed - you bought: **{0}**. You have **{1}** CP left.", 
                                item.StoreItemName, 
                                currentDiscordUser.CorruptPoints));

                            // Spam bought message
                            await ReplyAsync(embed: CreateBoughtEmbed("Transaction completed!", item, Context.User));
                        }
                        else
                        {
                            await ((SocketGuildUser)Context.User).SendMessageAsync(string.Format("You already have bought **{1}**", item.StoreItemName));
                        }

                       
                    }
                    else
                    {
                        await ((SocketGuildUser)Context.User).SendMessageAsync(string.Format("Not enough CP for {0}. {1}", item.Id, item.StoreItemName));
                    }
                }
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("cp-add")]
        [Summary("!cp-add {amount} {playername}- Gives a player the given amount")]
        public async Task SayCpAddAsync(int amount)
        {
            if (ToggleStateManager.GetToggleState("point-cp-add", Context.User) &&
               RoleHelper.HasRole(Context.User, Context.Guild, 3)) //bot dev
            {
                await ReplyAsync(string.Format("Please add a name after the amount. Example: **'!cp-add 100 Of the Abbys'**"));

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("cp-add")]
        [Summary("!cp-add {amount} {playername}- Gives a player the given amount of CP")]
        public async Task SayCpAddAsync(int amount, [Remainder]string playername)
        {
            if (ToggleStateManager.GetToggleState("point-cp-add", Context.User) &&
               RoleHelper.HasRole(Context.User, Context.Guild, 3)) //bot dev
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var currentUsername = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

                    // get user from name
                    DiscordUser discordAccount = null;
                    var runescapeAccount = corruptosEntities.RunescapeAccounts.FirstOrDefault(x => x.rsn.ToLower() == playername.ToLower());
                    if (runescapeAccount != null)
                    {
                        discordAccount = runescapeAccount.DiscordUser;
                    }

                    if (discordAccount != null)
                    {
                        // add the points
                        discordAccount.CorruptPoints += amount;

                        // add the mutation
                        corruptosEntities.PointMutations.Add(new PointMutation()
                        {
                            PointChange = amount,
                            PointStore = null,
                            DateTime = DateTime.Now,
                            DiscordUser = discordAccount
                        });

                        // Send message to channel
                        if (runescapeAccount.rsn.ToLower() == discordAccount.Username.ToLower())
                        {
                            if (amount > 0)
                            {
                                await ReplyAsync(string.Format("**{0}** was given **{1} CP** by {2}. They now have **{3} CP**.", discordAccount.Username, amount, currentUsername, discordAccount.CorruptPoints));
                            }
                            else
                            {
                                await ReplyAsync(string.Format("**{1} CP** was removed from **{0}** by {2}. They now have **{3} CP**.", discordAccount.Username, amount, currentUsername, discordAccount.CorruptPoints));
                            }
                        }
                        else
                        {
                            if (amount > 0)
                            {
                                await ReplyAsync(string.Format("**{0}** (owner of {2}) was given **{1} CP** by {3}. They now have **{4} CP**.", discordAccount, amount, runescapeAccount.rsn, currentUsername, discordAccount.CorruptPoints));
                            }
                            else
                            {
                                await ReplyAsync(string.Format("**{1} CP** was removed from **{0}** (owner of {2}) by {3}. They now have **{4} CP**.", discordAccount, amount, runescapeAccount.rsn, currentUsername, discordAccount.CorruptPoints));
                            }   
                        }

                        // Send message to player given points
                        var uid = Convert.ToUInt64(discordAccount.DiscordId);
                        var targetedDiscordUser = Context.Guild.GetUser(uid);
                        if (targetedDiscordUser != null)
                        {
                            if (amount > 0)
                            {
                                await targetedDiscordUser.SendMessageAsync(String.Format("You were given **{0} CP** by **{1}**. You now have **{2}** CP.", amount, currentUsername, discordAccount.CorruptPoints));
                            }
                            else
                            {
                                await targetedDiscordUser.SendMessageAsync(String.Format("**{0} CP** was removed by **{1}**. You now have **{2}** CP.", amount, currentUsername, discordAccount.CorruptPoints));
                            }
                        }
                        

                        await corruptosEntities.SaveChangesAsync();
                    }
                    else
                    {
                        await ReplyAsync(string.Format("**{0}** was not found", discordAccount.Username));
                    }

                }
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        [Helpgroup(HelpGroup.Admin)]
        [Command("cp-set")]
        [Summary("!cp-set {amount} {playername}- Sets a player's CP to the given amount")]
        public async Task SayCpSetAsync(int amount, [Remainder]string playername)
        {
            if (ToggleStateManager.GetToggleState("point-cp-set", Context.User) &&
               RoleHelper.HasRole(Context.User, Context.Guild, 3)) //bot dev
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var currentUsername = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

                    // get user from name
                    DiscordUser discordAccount = null;
                    var runescapeAccount = corruptosEntities.RunescapeAccounts.FirstOrDefault(x => x.rsn.ToLower() == playername.ToLower());
                    if (runescapeAccount != null)
                    {
                        discordAccount = runescapeAccount.DiscordUser;
                    }

                    if (discordAccount != null)
                    {
                        // add the points
                        discordAccount.CorruptPoints = amount;

                        // add the mutation
                        corruptosEntities.PointMutations.Add(new PointMutation()
                        {
                            PointChange = amount,
                            PointStore = null,
                            DateTime = DateTime.Now,
                            DiscordUser = discordAccount
                        });

                        // Send message to channel
                        if (runescapeAccount.rsn.ToLower() == discordAccount.Username.ToLower())
                        {
                            await ReplyAsync(string.Format("The CP of **{0}** was set to **{1} CP** by {2}.", discordAccount.Username, amount, currentUsername));
                        }
                        else
                        {
                            await ReplyAsync(string.Format("The CP of **{0}** (owner of {2}) was set to **{1} CP** by {3}.", discordAccount, amount, runescapeAccount.rsn, currentUsername));
                        }

                        // Send message to player given points
                        var uid = Convert.ToUInt64(discordAccount.DiscordId);
                        var targetedDiscordUser = Context.Guild.GetUser(uid);
                        if (targetedDiscordUser != null)
                        {
                            await targetedDiscordUser.SendMessageAsync(String.Format("Your CP was set to **{0} CP** by **{1}**.", amount, currentUsername));
                        }


                        await corruptosEntities.SaveChangesAsync();
                    }
                    else
                    {
                        await ReplyAsync(string.Format("**{0}** was not found", discordAccount.Username));
                    }

                }
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        [Helpgroup(HelpGroup.Admin)]
        [Command("cp-history")]
        [Summary("!cp-history - Gets a list of the CP history for a specific player")]
        public async Task SayCPHistoryAsync([Remainder]string playername)
        {
            if (ToggleStateManager.GetToggleState("point-cp-history", Context.User) &&
               RoleHelper.HasRole(Context.User, Context.Guild, 3)) //bot dev
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {

                    // get user from name
                    DiscordUser discordAccount = null;
                    var runescapeAccount = corruptosEntities.RunescapeAccounts.FirstOrDefault(x => x.rsn.ToLower() == playername.ToLower());
                    if (runescapeAccount != null)
                    {
                        discordAccount = runescapeAccount.DiscordUser;
                    }

                    if (discordAccount != null)
                    {
                        if (discordAccount.PointMutations.Any())
                        {
                            var eb = new EmbedBuilder();
                            eb.Title = string.Format("Corrupt Points history for {0}", discordAccount.Username);

                            var sb = new StringBuilder();

                            foreach (var pointMutation in discordAccount.PointMutations.OrderByDescending(x => x.Id).Take(20))
                            {
                                if (pointMutation.PointChange > 0)
                                {
                                    sb.AppendLine(string.Format("🔼 **{0}** | {1} {2}", pointMutation.PointChange, pointMutation.DateTime, pointMutation.PointStore?.StoreItemName));
                                }
                                else
                                {
                                    sb.AppendLine(string.Format("🔻 **{0}** | {1} {2}", pointMutation.PointChange, pointMutation.DateTime, pointMutation.PointStore?.StoreItemName));
                                }
                            }
                            eb.AddField("\u200b", sb.ToString(), false);

                            await ReplyAsync(embed: eb.Build());
                        }
                        else
                        {
                            await ReplyAsync(string.Format("**{0}** has no CP history", discordAccount.Username));
                        }
                    }
                    else
                    {
                        await ReplyAsync(string.Format("**{0}** was not found", discordAccount.Username));
                    }
                }
            }

            //await ReplyAsync(string.Format("**{0}** was not found", discordAccount, amount));

            // delete the command posted
            await Context.Message.DeleteAsync();
        }





        private void GiveRewardItem(PointStore item)
        {
            //TODO: NO IDEA HOW TO DO THIS YET - FIGURE OUT LATER
        }

        private Embed CreateBoughtEmbed(string title, PointStore item, SocketUser user)
        {
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.Title = title;
            builder.Description = String.Format("{0} bought {1}", GetAccountNameOrNickname(user), item.StoreItemName);
            if (!string.IsNullOrEmpty(item.StoreItemImage))
            {
                builder.ImageUrl = item.StoreItemImage;
            }
            
            return builder.Build();
        }

        public static string GetAccountNameOrNickname(SocketUser user)
        {
            var currentUser = ((SocketGuildUser)user);
            var name = currentUser.Nickname ?? user.Username;
            return name;
        }
    }
}
