using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
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
               PermissionManager.HasSpecificRole(Context.User, "Developer"))
            {

                using (CorruptPointsModel corruptosEntities = new CorruptPointsModel())
                {
                    var userId = Convert.ToInt64(Context.User.Id);
                    var currentDiscordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == userId);

                    if (currentDiscordUser != null)
                    {
                        await ReplyAsync(string.Format("Current CP: {0}", currentDiscordUser.CorruptPoints));
                    }
                }
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        //[Helpgroup(HelpGroup.Admin)]
        //[Command("cp-store")]
        //[Summary("!cp-store - Shows the available Corrupt Points items to buy")]
        //public async Task SayCpStoreAsync()
        //{
        //    if (ToggleStateManager.GetToggleState("point-cp-store", Context.User) &&
        //       PermissionManager.HasSpecificRole(Context.User, "Developer"))
        //    {
        //        using (CorruptPointsModel corruptosEntities = new CorruptPointsModel())
        //        {
        //            await ((SocketGuildUser)Context.User).SendMessageAsync(embed: CreateShopEmbed("Corrupt OS - Store"));
        //            foreach (var item in corruptosEntities.PointStores.ToList())
        //            {
        //                await ((SocketGuildUser)Context.User).SendMessageAsync(embed: CreateStoreItem(item));
        //            }
        //        }
        //    }

        //    // delete the command posted
        //    await Context.Message.DeleteAsync();
        //}

        [Helpgroup(HelpGroup.Admin)]
        [Command("cp-buy")]
        [Summary("!cp-buy {itemid}- Buys the selected item using its item ID")]
        public async Task SayCpStoreAsync(int itemId)
        {
            if (ToggleStateManager.GetToggleState("point-cp-buy", Context.User) &&
               PermissionManager.HasSpecificRole(Context.User, "Developer"))
            {
                using (CorruptPointsModel corruptosEntities = new CorruptPointsModel())
                {
                    //check if enough points

                    // get itemprice
                    var item = corruptosEntities.PointStores.FirstOrDefault(x => x.Id == itemId);

                    // get user
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
                            await ((SocketGuildUser)Context.User).SendMessageAsync(string.Format("Transaction completed - you bought: {0}. {1}", item.Id, item.StoreItemName));

                            // Spam bought message
                            await ReplyAsync(embed: CreateBoughtEmbed("Transaction completed!", item, Context.User));
                        }
                        else
                        {
                            await ((SocketGuildUser)Context.User).SendMessageAsync(string.Format("You already have bought {0}. {1}", item.Id, item.StoreItemName));
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





        private Embed CreateShopEmbed(string title)
        {
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;
            builder.Title = title;
            builder.ThumbnailUrl = "https://oldschool.runescape.wiki/images/thumb/7/79/Mahogany_prize_chest_built.png/250px-Mahogany_prize_chest_built.png?15f5b";
            return builder.Build();
        }

        private Embed CreateStoreItem(PointStore item)
        {
            var builder = new EmbedBuilder();
            builder.Color = Color.Blue;

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("**{0}**", item.StoreItemName));
            sb.AppendLine(item.StoreItemDescription);
            sb.AppendLine(string.Format("**{0}** CP", item.StoreItemValue));
            sb.AppendLine(string.Format("*Type ' **!cp-buy {0}** ' to buy this item*", item.StoreItemCommand));

            builder.Description = sb.ToString();
            builder.ThumbnailUrl = item.StoreItemImage;
            return builder.Build();
        }
    }
}
