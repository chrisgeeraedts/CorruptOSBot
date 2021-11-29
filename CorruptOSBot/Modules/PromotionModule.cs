using CorruptOSBot.Data;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
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
    public class PromotionModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Staff)]
        [Command("promotions-blacklist")]
        [Summary("!promotions-blacklist {username} - Adds or remove a username to/from the promotion blacklist ")]
        public async Task SayPromotionblacklistAsync([Remainder] string username)
        {
            if (ToggleStateManager.GetToggleState("promotion-blacklist", Context.User) &&
                (RoleHelper.HasStaffOrModOrOwnerRole(Context.User, Context.Guild)))
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

                    // delete the command posted
                    await Context.Message.DeleteAsync();
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Error, "PromotionModule", "Database issue - " + e.ToString()));
                }
            }
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("promotions-blacklist")]
        [Summary("!promotions-blacklist - shows the promotion blacklist ")]
        public async Task SayPromotionblacklistAsync()
        {
            if (ToggleStateManager.GetToggleState("promotion-blacklist", Context.User) &&
                RoleHelper.HasStaffOrModOrOwnerRole(Context.User, Context.Guild))
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

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Helpgroup(HelpGroup.Staff)]
        [Command("promotions")]
        [Summary("!promotions - Gets a list of accounts that need promotions")]
        public async Task SayPromotionsAsync()
        {
            await Context.Channel.SendMessageAsync("Currently down");

            //if (ToggleStateManager.GetToggleState("promotions", Context.User) &&
            //    (RoleHelper.HasStaffOrModOrOwnerRole(Context.User, Context.Guild)))
            //{
            //    // get players
            //    await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneHourMS);
            //    var guildId = ConfigHelper.GetGuildId();
            //    var guild = ((Discord.IDiscordClient)Context.Client).GetGuildAsync(guildId).Result;

            //    var allUsersFromDB = new List<DiscordUser>();
            //    using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
            //    {
            //        allUsersFromDB = corruptosEntities.DiscordUsers.ToList();
            //    }

            //    var promotionSet = GetSupposedRank(allUsersFromDB, guild);
            //    var updatedPromotionSet = GetCurrentRank(promotionSet);

            //    await SendEmbedMessages(updatedPromotionSet, RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 12)); //Cadet
            //    await SendEmbedMessages(updatedPromotionSet, RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 11)); //Sergeant
            //    await SendEmbedMessages(updatedPromotionSet, RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 10)); //Novice
            //    await SendEmbedMessages(updatedPromotionSet, RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 9)); //Corporal
            //    await SendEmbedMessages(updatedPromotionSet, RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 8)); //Peon
            //    await SendInactivityEmbedMessage(updatedPromotionSet);

            //    // delete the command posted
            //    await Context.Message.DeleteAsync();
            //}
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
                        await Context.Channel.SendMessageAsync($"Rank: {user.Role.Name} \nPoints: {user.Points}");
                    }
                }
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
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

        private async Task SendEmbedMessages(List<PromotionSet> differences, Role rank)
        {
            var shouldPost = false;
            var result = BuildMessageForRank(differences, rank, out shouldPost);
            if (shouldPost)
            {
                foreach (var item in result)
                {
                    await Context.Channel.SendMessageAsync(embed: item);
                }
            }
        }

        private async Task SendInactivityEmbedMessage(List<PromotionSet> differences)
        {
            bool shouldPost;
            var result = BuildInactivityMessageForRank(differences, out shouldPost);
            if (shouldPost)
            {
                await Context.Channel.SendMessageAsync(embed: result);
            }
        }

        //private DateTime GetLastDayOfMonth(DateTime dateTime)
        //{
        //    return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        //}

        private List<Embed> BuildMessageForRank(List<PromotionSet> differences, Role rank, out bool hasUsers)
        {
            var result = new List<Discord.Embed>();
            hasUsers = false;
            var sb = new StringBuilder();
            var usersWithChanges = differences.Where(x =>
                x.ShouldHaveRank == rank &&
                x.CurrentRank != x.ShouldHaveRank
                //&& GetRoleRank(x.ShouldHaveRank) > GetRoleRank(x.CurrentRank) <-- use this for only upgrades
                );

            if (usersWithChanges.Any())
            {
                hasUsers = true;
                foreach (var item in usersWithChanges)
                {
                    // ensure that we do not go over the 2k content cap
                    if (sb.ToString().Length > 1900)
                    {
                        // reset, since we can only post 2000 characters
                        string titled = string.Format("{0} at {1}", rank.Name, DateTime.Now);
                        result.Add(EmbedHelper.CreateDefaultEmbed(titled, sb.ToString()));
                        sb.Clear();
                    }
                    else
                    {
                        sb.AppendLine(string.Format("**{0}** has **{1}** and should have **{2}** based on **{3}** days in Discord",
                            DiscordNameHelper.GetAccountNameOrNickname(item.User),
                            item.CurrentRank.Name,
                            item.ShouldHaveRank.Name,
                            item.DaysInDiscord));
                    }
                }
                string title = string.Format("{0} at {1}", rank, DateTime.Now);
                result.Add(EmbedHelper.CreateDefaultEmbed(title, sb.ToString(), rank.IconUrl));
            }
            return result;
        }

        private Embed BuildInactivityMessageForRank(List<PromotionSet> differences, out bool hasUsers)
        {
            var sb = new StringBuilder();
            var inactiveRole = RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 19); // Inactive
            hasUsers = differences.Any(x =>
            x.ShouldHaveRank.Id == inactiveRole.Id &&
            x.CurrentRank != x.ShouldHaveRank);
            if (hasUsers)
            {
                foreach (var item in differences.Where(x =>
                x.ShouldHaveRank.Id == inactiveRole.Id &&
                x.CurrentRank.Id != x.ShouldHaveRank.Id))
                {
                    sb.AppendLine(string.Format("**{0}** has **{1}** and should have **{2}** based on **{3}** days inactivity on WOM",
                        DiscordNameHelper.GetAccountNameOrNickname(item.User),
                        item.CurrentRank.Name,
                        item.ShouldHaveRank.Name,
                        item.DaysInDiscord));
                }

                return EmbedHelper.CreateDefaultEmbed(inactiveRole.Name.ToString(), sb.ToString());
            }
            else
            {
                return EmbedHelper.CreateDefaultEmbed(inactiveRole.Name.ToString(), string.Format("All {0} users have the correct rank", inactiveRole.Name));
            }
        }

        private List<PromotionSet> GetSupposedRank(List<DiscordUser> users, IGuild guild)
        {
            var result = new List<PromotionSet>();
            var availableRoles = RoleHelper.GetRoles().Where(x => x.CanUpgradeTo);
            var inactiveRole = RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 19); // Inactive
            var clanFriendRole = RoleHelper.GetRoles().FirstOrDefault(x => x.Id == 20); //Clan friend

            var dateTimeCurrent = DateTime.Now; //GetLastDayOfMonth(DateTime.Now);

            var discordUsers = guild.GetUsersAsync().Result;

            foreach (var user in users)
            {
                var joinDate = user.OriginallyJoinedAt.Value;
                var blacklisted = user.BlacklistedForPromotion;
                var hasLeft = user.LeavingDate.HasValue;
                var userId = Convert.ToUInt64(user.DiscordId);

                // grab the active discord account from the GUILD
                var discordUser = discordUsers.FirstOrDefault(x => x.Id == userId);

                if (discordUser != null &&
                    joinDate != null &&
                    joinDate != DateTime.MinValue &&
                    !RoleHelper.HasRole(discordUser, guild, inactiveRole) &&
                    !RoleHelper.HasRole(discordUser, guild, clanFriendRole) &&
                    !RoleHelper.HasStaffOrModOrOwnerRole(discordUser, guild) &&
                    !blacklisted &&
                    !hasLeft)
                {
                    int daysInactive = -1;
                    // check if inactive
                    if (!IsInactive(discordUser, out daysInactive))
                    {
                        var daysFromJoining = Convert.ToInt32(dateTimeCurrent.Subtract(joinDate).TotalDays);
                        //var shouldHaveThisRank = availableRoles.Where(x => daysFromJoining >= x.DaysToReach).OrderByDescending(x => x.DaysToReach).FirstOrDefault();
                        var baseRank = availableRoles.FirstOrDefault(x => x.Id == 7);

                        //if (shouldHaveThisRank != null)
                        //{
                        //    result.Add(new PromotionSet() { User = discordUser, ShouldHaveRank = shouldHaveThisRank, DaystillRank = 9999, UserId = discordUser.Id, DaysInDiscord = daysFromJoining });
                        //}
                        //else
                        //{
                        //    result.Add(new PromotionSet() { User = discordUser, ShouldHaveRank = baseRank, DaystillRank = baseRank.DaysToReach - daysFromJoining, UserId = discordUser.Id, DaysInDiscord = daysFromJoining });
                        //}
                    }
                    else
                    {
                        result.Add(new PromotionSet() { User = discordUser, ShouldHaveRank = inactiveRole, DaystillRank = -1, UserId = discordUser.Id, DaysInDiscord = daysInactive });
                    }
                }
            }
            return result;
        }

        private string GetRoleIcon(Role rank)
        {
            return rank.IconUrl;
        }

        private List<PromotionSet> GetCurrentRank(List<PromotionSet> users)
        {
            var result = new List<PromotionSet>();
            var availableRoles = RoleHelper.GetRoles().Where(x => x.CanUpgradeTo);

            foreach (var user in users)
            {
                foreach (var userRoles in user.User.RoleIds)
                {
                    //var role = availableRoles.OrderByDescending(x => x.DaysToReach).FirstOrDefault(x => Convert.ToUInt64(x.DiscordRoleId) == userRoles);
                    //if (role != null)
                    //{
                    //    user.CurrentRank = role;
                    //    result.Add(user);
                    //}
                }
            }
            return result;
        }

        private bool IsInactive(Discord.IGuildUser user, out int daysInactive)
        {
            daysInactive = -1;
            var name = DiscordNameHelper.GetAccountNameOrNickname(user);
            var womMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == name.ToLower());

            if (womMember != null && womMember.lastChangedAt.HasValue)
            {
                daysInactive = Convert.ToInt32(DateTime.Now.Subtract(womMember.lastChangedAt.Value).TotalDays);
                return daysInactive > 31;
            }
            return false;
        }
    }

    public class PromotionSet
    {
        public Role ShouldHaveRank { get; set; }
        public Role CurrentRank { get; set; }
        public int DaystillRank { get; set; }
        public int DaysInDiscord { get; set; }
        public Discord.IGuildUser User { get; set; }
        public ulong UserId { get; set; }
    }
}