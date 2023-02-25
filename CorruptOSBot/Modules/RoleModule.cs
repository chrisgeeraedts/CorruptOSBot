using CorruptOSBot.Data;
using CorruptOSBot.Extensions;
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
                    await Program.Log(new LogMessage(LogSeverity.Error, "RoleModule", "Database issue - " + e.ToString()));
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
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "spam-bot-commands", Context.User))
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var userDiscordId = ((SocketGuildUser)Context.User).Id;

                    var discordAcc = corruptosEntities.DiscordUsers.FirstOrDefault(item => item.DiscordId == (long?)userDiscordId);

                    if (discordAcc != null)
                    {
                        var roles = corruptosEntities.Roles.Where(item => item.PointsToReach > discordAcc.Points).OrderBy(item => item.PointsToReach);

                        var description = new StringBuilder();

                        foreach (var role in roles)
                        {
                            var pointsNeed = role.PointsToReach - discordAcc.Points;

                            description.AppendLine($"{role.Name} requires {pointsNeed} more points");
                        }

                        await Context.Channel.SendMessageAsync(
                            embed: EmbedHelper.CreateDefaultEmbed(
                                $"{discordAcc.Username}\nCurrent Role: {discordAcc.Role.Name} Current Points: {discordAcc.Points}",
                                description.ToString()));
                    }
                    else
                    {
                        await Program.Log(new LogMessage(LogSeverity.Error, "RoleModule", $"Unable to find user {Context.User.Username} - Discord ID {Context.User.Id}"));
                        await Context.Channel.SendMessageAsync($"Unable to find user {Context.User.Username} - Discord ID {Context.User.Id}");
                    }
                }
            }
            else
            {
                await DiscordHelper.NotAllowedMessageToUser(Context, "!checkpoints", "spam-bot-commands");
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("add-points", false)]
        [Summary("!add-points {username} {points} - gives specified member the specified points")]
        public async Task AddPoints(string username, int points)
        {
            if (RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var user = GetUser(username, corruptosEntities);

                    if (user != null && !user.BlacklistedForPromotion)
                    {
                        var newPoints = user.Points += points;

                        user.Points = newPoints;

                        await UpdateDiscordUserRole(user, true, corruptosEntities, Context);

                        await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed($"{points} added to {user.Username}, giving a total of {newPoints}", string.Empty));

                        await corruptosEntities.SaveChangesAsync();
                    }
                    else if (user != null && user.BlacklistedForPromotion)
                    {
                        await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed($"User {user.Username} is blacklisted from promotion", string.Empty));
                    }
                    else
                    {
                        await SendUserNotFoundMessage(username);
                    }
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("sub-points", false)]
        [Summary("!sub-points {username} {points} - subtracts specified member the specified points")]
        public async Task SubPoints(string username, int points)
        {
            if (RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var user = GetUser(username, corruptosEntities);

                    if (user != null && !user.BlacklistedForPromotion)
                    {
                        var newPoints = user.Points -= points;

                        if (newPoints < 0)
                        {
                            newPoints = 0;
                        }

                        user.Points = newPoints;

                        await UpdateDiscordUserRole(user, false, corruptosEntities, Context);

                        await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed($"{points} removed from {user.Username}, giving a total of {newPoints}", string.Empty));

                        await corruptosEntities.SaveChangesAsync();
                    }
                    else if (user != null && user.BlacklistedForPromotion)
                    {
                        await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed($"User {user.Username} is blacklisted from promotion", string.Empty));
                    }
                    else
                    {
                        await SendUserNotFoundMessage(username);
                    }
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("set-points", false)]
        [Summary("!set-points {username} {points} - sets specified member the specified points")]
        public async Task SetPoints(string username, int points)
        {
            if (RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var user = GetUser(username, corruptosEntities);

                    if (user != null && !user.BlacklistedForPromotion)
                    {
                        var isPromotion = points > user.Points;

                        user.Points = points;

                        await UpdateDiscordUserRole(user, isPromotion, corruptosEntities, Context);

                        await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed($"Set {user.Username} points to {points}", string.Empty));

                        await corruptosEntities.SaveChangesAsync();
                    }
                    else if (user != null && user.BlacklistedForPromotion)
                    {
                        await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed($"User {user.Username} is blacklisted from promotion", string.Empty));
                    }
                    else
                    {
                        await SendUserNotFoundMessage(username);
                    }
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("set-role", false)]
        [Summary("!set-role {username} {role} - sets specified member the specified role")]
        public async Task SetRole(string username, string roleName)
        {
            if (RoleHelper.IsStaff(Context.User, Context.Guild))
            {
                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var user = GetUser(username, corruptosEntities);

                    if (user != null && !user.BlacklistedForPromotion)
                    {
                        var roleToAssign = corruptosEntities.Roles.FirstOrDefault(item => item.Name == roleName);

                        if (roleToAssign != null)
                        {
                            user.RoleId = roleToAssign.Id;
                        }

                        await corruptosEntities.SaveChangesAsync();
                    }
                    else if (user != null && user.BlacklistedForPromotion)
                    {
                        await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed($"User {user.Username} is blacklisted from promotion", string.Empty));
                    }
                    else
                    {
                        await SendUserNotFoundMessage(username);
                    }
                }
            }

            await Context.Message.DeleteAsync();
        }

        private static async Task UpdateDiscordUserRole(DiscordUser user, bool isPromotion, CorruptModel corruptosEntities, SocketCommandContext context)
        {
            if (user.DiscordId != null)
            {
                var roles = corruptosEntities.Roles.ToList();
                var channels = corruptosEntities.Channels.ToList();

                var roleToBeApplied = roles.FirstOrDefault(item => user.Points >= item.PointsToReach && user.Points <= item.MaximumPoints);

                if (roleToBeApplied.Id != user.RoleId && !user.Role.IsStaff)
                {
                    var discordRole = context.Guild.Roles.FirstOrDefault(item => item.Name == roleToBeApplied.Name);
                    var discordUser = context.Guild.Users.FirstOrDefault(item => item.Id == (ulong)user.DiscordId);

                    await Program.Log(new LogMessage(LogSeverity.Info, "RoleModule", $"Removing role: {user.Role.Name} with DiscordRoleId: {user.Role.DiscordRoleId} from user: {user.Username}"));
                    await discordUser.RemoveRoleAsync((ulong)user.Role.DiscordRoleId);
                    await Program.Log(new LogMessage(LogSeverity.Info, "RoleModule", $"Add role: {roleToBeApplied.Name} with DiscordRoleId: {roleToBeApplied.DiscordRoleId} to user: {user.Username}"));
                    await discordUser.AddRoleAsync((ulong)roleToBeApplied.DiscordRoleId);

                    user.RoleId = roleToBeApplied.Id;

                    if (isPromotion)
                    {
                        var rankRequestsChannel = context.Guild.Channels.FirstOrDefault(item => item.Name == "rank-requests");

                        await ((IMessageChannel)rankRequestsChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(string.Format("Rank promotion for {0}!", user.Username),
                            string.Format($"<@{user.DiscordId}> just got promoted to {discordRole.Name}!"),
                            roleToBeApplied.IconUrl, "https://static.wikia.nocookie.net/getsetgames/images/8/82/Level_up_icon.png/revision/latest?cb=20130804113035"));
                    }
                }
            }
        }

        private async Task SendUserNotFoundMessage(string username)
        {
            await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(string.Format($"User {username} not found"), string.Empty));
        }

        private DiscordUser GetUser(string username, CorruptModel corruptosEntities)
        {
            DiscordUser result = null;

            var runescapeAcc = corruptosEntities.RunescapeAccounts.FirstOrDefault(item => item.rsn.ToLower() == username.ToLower());

            if (runescapeAcc != null)
            {
                return runescapeAcc.DiscordUser;
            }

            return result;
        }

        private List<Embed> BuildMessageForBlacklist(IEnumerable<Data.DiscordUser> blacklistedDiscordUsers)
        {
            var result = new List<Embed>();
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