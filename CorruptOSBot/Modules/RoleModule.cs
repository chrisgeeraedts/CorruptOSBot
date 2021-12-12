using CorruptOSBot.Data;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class RoleModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Staff)]
        [Command("promotions-blacklist")]
        [Summary("!promotions-blacklist {username} - Adds or remove a username to/from the promotion blacklist ")]
        public async Task SayPromotionblacklistAsync([Remainder] string username)
        {
            if (ToggleStateManager.GetToggleState("promotion-blacklist", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
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
                        using (var database = new Data.CorruptModel())
                        {
                            var userid = Convert.ToInt64(user.Id);
                            var discordUserFromDB = database.DiscordUsers.FirstOrDefault(x => x.DiscordId == userid);

                            if (discordUserFromDB != null)
                            {
                                discordUserFromDB.BlacklistedForPromotion = !discordUserFromDB.BlacklistedForPromotion;
                                await database.SaveChangesAsync();

                                if (discordUserFromDB.BlacklistedForPromotion)
                                {
                                    await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(string.Format("User {0} added to promotion blacklist", username), string.Empty));
                                }
                                else
                                {
                                    await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(string.Format("User {0} removed from promotion blacklist", username), string.Empty));
                                }
                            }
                            else
                            {
                                await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(string.Format("User {0} not found in database - unable to blacklist", username), string.Empty));
                            }
                        }
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(string.Format("User {0} not found", username), string.Empty));
                    }
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Error, "PromotionModule", "Database issue - " + e.ToString()));
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("promotions-blacklist")]
        [Summary("!promotions-blacklist - shows the promotion blacklist ")]
        public async Task SayPromotionblacklistAsync()
        {
            if (ToggleStateManager.GetToggleState("promotion-blacklist", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                try
                {
                    using (var database = new Data.CorruptModel())
                    {
                        var blacklistedDiscordUsers = database.DiscordUsers.Where(x => x.BlacklistedForPromotion);
                        var embeds = BuildMessageForBlacklist(blacklistedDiscordUsers);
                        if (embeds.Any())
                        {
                            foreach (var embed in embeds)
                            {
                                await Context.Channel.SendMessageAsync(embed: embed);
                            }
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Promotions Blacklist", "No discord users on the promotions blacklist"));
                        }
                    }
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Error, "PromotionModule", "Database issue - " + e.Message));
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("checkpoints")]
        [Summary("!checkpoints - Shows memeber their points")]
        public async Task CheckpointsAsync()
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "bot-command", Context.User))
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var user = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.Username.ToLower() == Context.User.Username.ToString().ToLower());

                    if (user != null)
                    {
                        var roles = corruptosEntities.Roles.Where(item => item.PointsToReach > user.Points).OrderBy(item => item.PointsToReach);

                        var description = new StringBuilder();

                        foreach (var role in roles)
                        {
                            var pointsNeed = role.PointsToReach - user.Points;

                            description.AppendLine($"{role.Name} requires {pointsNeed} points");
                        }

                        await Context.Channel.SendMessageAsync(
                            embed: EmbedHelper.CreateDefaultEmbed(
                                $"{Context.User.Username}\nCurrent Role: {user.Role.Name} Current Points: {user.Points}",
                                description.ToString()));
                    }
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("add-points", false)]
        [Summary("!add-points {username} - gives specified member the specified points")]
        public async Task AddPoints(string username, int points)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "bot-command", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var users = corruptosEntities.DiscordUsers.ToList();

                    var user = corruptosEntities.RunescapeAccounts.FirstOrDefault(x => x.rsn.ToLower() == username.ToLower());

                    if (user != null)
                    {
                        var newPoints = user.DiscordUser.Points += points;

                        user.DiscordUser.Points = newPoints;

                        await UpdateDiscordUserRole(user.DiscordUser, true, corruptosEntities, Context);

                        await corruptosEntities.SaveChangesAsync();
                    }
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("sub-points", false)]
        [Summary("!sub-points {username} - subtracts specified member the specified points")]
        public async Task SubPoints(string username, int points)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "bot-command", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var user = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());

                    if (user != null)
                    {
                        var newPoints = user.Points -= points;

                        if (newPoints < 0)
                        {
                            newPoints = 0;
                        }

                        user.Points = newPoints;

                        await UpdateDiscordUserRole(user, false, corruptosEntities, Context);

                        await corruptosEntities.SaveChangesAsync();
                    }
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("set-points", false)]
        [Summary("!set-points {username} - sets specified member the specified points")]
        public async Task SetPoints(string username, int points)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "bot-command", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var user = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());

                    if (user != null)
                    {
                        var isPromotion = points > user.Points;

                        user.Points = points;

                        await UpdateDiscordUserRole(user, isPromotion, corruptosEntities, Context);

                        await corruptosEntities.SaveChangesAsync();
                    }
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("set-role", false)]
        [Summary("!set-role {username} - sets specified member the specified role")]
        public async Task SetRank(string username, int roleId)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "clan-bot", Context.User) && RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var user = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
                    var role = corruptosEntities.Roles.FirstOrDefault(x => x.Id == roleId);

                    if (user != null && role != null)
                    {
                        user.RoleId = role.Id;

                        var discordRole = Context.Guild.Roles.FirstOrDefault(item => item.Name == role.Name);

                        await (Context.User as IGuildUser).RemoveRoleAsync((ulong)user.Role.DiscordRoleId);
                        await (Context.User as IGuildUser).AddRoleAsync(discordRole);
                        await corruptosEntities.SaveChangesAsync();
                    }
                }
            }

            await Context.Message.DeleteAsync();
        }

        private static async Task UpdateDiscordUserRole(DiscordUser user, bool isPromotion, CorruptModel corruptosEntities, SocketCommandContext context)
        {
            var roles = corruptosEntities.Roles.ToList();
            var channels = corruptosEntities.Channels.ToList();

            var roleToBeApplied = roles.FirstOrDefault(item => user.Points >= item.PointsToReach && user.Points <= item.MaximumPoints);

            if (roleToBeApplied.Id != user.RoleId && !user.Role.IsStaff)
            {
                var discordRole = context.Guild.Roles.FirstOrDefault(item => item.Name == roleToBeApplied.Name);

                await (context.User as IGuildUser).RemoveRoleAsync((ulong)user.Role.DiscordRoleId);
                await (context.User as IGuildUser).AddRoleAsync(discordRole);

                user.RoleId = roleToBeApplied.Id;

                if (isPromotion)
                {
                    var generalChannel = context.Guild.Channels.FirstOrDefault(item => item.Name == "general");

                    await ((IMessageChannel)generalChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(string.Format("Rank promotion for {0}!", user.Username),
                        string.Format("<@{0}> just got promoted to <@&{1}>!", user.DiscordId, discordRole.Id),
                        roleToBeApplied.IconUrl, "https://static.wikia.nocookie.net/getsetgames/images/8/82/Level_up_icon.png/revision/latest?cb=20130804113035"));
                }
            }
        }

        private List<Embed> BuildMessageForBlacklist(IEnumerable<Data.DiscordUser> blacklistedDiscordUsers)
        {
            var result = new List<Discord.Embed>();
            var sb = new StringBuilder();

            if (blacklistedDiscordUsers.Any())
            {
                foreach (var item in blacklistedDiscordUsers)
                {
                    // ensure that we do not go over the 2k content cap
                    if (sb.ToString().Length > 1900)
                    {
                        // reset, since we can only post 2000 characters
                        result.Add(EmbedHelper.CreateDefaultEmbed("Promotions Blacklist", sb.ToString()));
                        sb.Clear();
                    }
                    else
                    {
                        sb.AppendLine(string.Format("- **{0}**", item.Username));
                    }
                }
                result.Add(EmbedHelper.CreateDefaultEmbed("Promotions Blacklist", sb.ToString()));
            }
            return result;
        }
    }
}