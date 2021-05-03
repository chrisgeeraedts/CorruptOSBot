using CorruptOSBot.Data;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
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
        [Helpgroup(HelpGroup.Moderator)]
        [Command("promotions-blacklist")]
        [Summary("!promotions-blacklist {username} - Adds or remove a username to/from the promotion blacklist ")]
        public async Task SayPromotionblacklistAsync([Remainder]string username)
        {
            if (ToggleStateManager.GetToggleState("promotion-blacklist", Context.User) &&
                (PermissionManager.HasSpecificRole(Context.User, "Staff") ||
                PermissionManager.HasSpecificRole(Context.User, "Moderator")))
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
                    try
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
                    catch (Exception e)
                    {
                        await Program.Log(new LogMessage(LogSeverity.Error, "PromotionModule", "Database issue - " + e.Message));
                    }
                    
                }
                else
                {
                    await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(string.Format("User {0} not found", username), string.Empty));
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Helpgroup(HelpGroup.Moderator)]
        [Command("promotions-blacklist")]
        [Summary("!promotions-blacklist - shows the promotion blacklist ")]
        public async Task SayPromotionblacklistAsync()
        {
            if (ToggleStateManager.GetToggleState("promotion-blacklist", Context.User) &&
                (PermissionManager.HasSpecificRole(Context.User, "Staff") ||
                PermissionManager.HasSpecificRole(Context.User, "Moderator")))
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
        
        [Helpgroup(HelpGroup.Moderator)]
        [Command("promotions")]
        [Summary("!promotions - Gets a list of accounts that need promotions")]
        public async Task SayPromotionsAsync()
        {
            if (ToggleStateManager.GetToggleState("promotions", Context.User) &&
                (
                PermissionManager.HasSpecificRole(Context.User, "Staff") || 
                PermissionManager.HasSpecificRole(Context.User, "Moderator")))
            {
                // get players 
                await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneHourMS);
                var guildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
                var guild = ((Discord.IDiscordClient)Context.Client).GetGuildAsync(guildId).Result;
                // iterate through all discord users
                var allUsers = guild.GetUsersAsync().Result;

                var promotionSet = GetSupposedRank(allUsers, guild);
                var updatedPromotionSet = GetCurrentRank(promotionSet, guild);

                await SendEmbedMessages(updatedPromotionSet, Rank.OG);
                await SendEmbedMessages(updatedPromotionSet, Rank.Sergeant);
                await SendEmbedMessages(updatedPromotionSet, Rank.Corperal);
                await SendEmbedMessages(updatedPromotionSet, Rank.Recruit);
                //await SendInactivityEmbedMessage(updatedPromotionSet);

                // delete the command posted
                await Context.Message.DeleteAsync();
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

        private async Task SendEmbedMessages(List<PromotionSet> differences, Rank rank)
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
            var shouldPost = false;
            var result = BuildInactivityMessageForRank(differences, out shouldPost);
            if (shouldPost)
            {
                await Context.Channel.SendMessageAsync(embed: result);
            }
        }

        private DateTime GetLastDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        private List<Embed> BuildMessageForRank(List<PromotionSet> differences, Rank rank, out bool hasUsers)
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
                        string titled = string.Format("{0} at {1} (last day of the current month)", rank, GetLastDayOfMonth(DateTime.Now));
                        result.Add(EmbedHelper.CreateDefaultEmbed(titled, sb.ToString()));
                        sb.Clear();
                    }
                    else
                    {
                        sb.AppendLine(string.Format("**{0}** has **{1}** and should have **{2}** based on **{3}** days in Discord", 
                            DiscordHelper.GetAccountNameOrNickname(item.User), 
                            item.CurrentRank, 
                            item.ShouldHaveRank, 
                            item.DaysInDiscord));
                    }
                }
                string title = string.Format("{0} at {1} (last day of the current month)", rank, GetLastDayOfMonth(DateTime.Now));
                result.Add(EmbedHelper.CreateDefaultEmbed(title, sb.ToString()));
            }
            return result;
        }

        private Embed BuildInactivityMessageForRank(List<PromotionSet> differences, out bool hasUsers)
        {
            var sb = new StringBuilder();
            hasUsers = differences.Any(x => x.ShouldHaveRank == Rank.Inactive && x.CurrentRank != x.ShouldHaveRank);
            if (hasUsers)
            {
                foreach (var item in differences.Where(x => x.ShouldHaveRank == Rank.Inactive && x.CurrentRank != x.ShouldHaveRank))
                {
                    sb.AppendLine(string.Format("**{0}** has **{1}** and should have **{2}** based on **{3}** days inactivity on WOM", DiscordHelper.GetAccountNameOrNickname(item.User), item.CurrentRank, item.ShouldHaveRank, item.DaysInDiscord));
                }

                return EmbedHelper.CreateDefaultEmbed(Rank.Inactive.ToString(), sb.ToString());
            }
            else
            {
                return EmbedHelper.CreateDefaultEmbed(Rank.Inactive.ToString(), string.Format("All {0} users have the correct rank", Rank.Inactive));
            }
        }

        private List<PromotionSet> GetSupposedRank(IReadOnlyCollection<Discord.IGuildUser> users, Discord.IGuild guild)
        {
            var dataset = new Dictionary<long?, DiscordUser>();

            try
            {
                using (var database = new Data.CorruptModel())
                {
                    dataset = database.DiscordUsers.ToDictionary(x => x.DiscordId);
                }
            }
            catch (Exception e)
            {
                Program.Log(new LogMessage(LogSeverity.Error, "GetSupposedRank", "Failed to get joindata from database - " + e.Message));
            }
           
            var result = new List<PromotionSet>();

            var daysInYear = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
            var daysIn6Months = Convert.ToInt32(daysInYear / 2);
            var daysIn3Months = Convert.ToInt32(daysIn6Months / 2);
            var daysIn1Month = Convert.ToInt32(daysIn6Months / 3);

            var dateTimeCurrent = GetLastDayOfMonth(DateTime.Now);

            foreach (var user in users.Where(x => !x.IsBot && !x.IsWebhook))
            {
                var joinDate = user.JoinedAt.Value.DateTime;
                var blacklisted = false;
                var hasLeft = false;
                var userId = Convert.ToInt64(user.Id);

                // if we have a database entry, use that join date
                if (dataset.ContainsKey(userId) && dataset[userId].OriginallyJoinedAt.HasValue)
                {
                    joinDate = dataset[userId].OriginallyJoinedAt.Value;
                }
                if (dataset.ContainsKey(userId))
                {
                    blacklisted = dataset[userId].BlacklistedForPromotion;
                    hasLeft = dataset[userId].LeavingDate.HasValue;
                }

                if (joinDate != null &&
                    joinDate != DateTime.MinValue &&
                    !DiscordHelper.HasRole(user, guild, "inactive") &&
                    !DiscordHelper.HasRole(user, guild, "Clan Friend") &&
                    !HasStaffOrModOrOwnerRole(user, guild) &&
                    !blacklisted &&
                    !hasLeft)
                {
                    int daysInactive = -1;
                    // check if inactive
                    if (!IsInactive(user, out daysInactive))
                    {
                        var daysFromJoining = Convert.ToInt32(dateTimeCurrent.Subtract(joinDate).TotalDays);

                        if (daysFromJoining >= daysInYear)
                        {
                            result.Add(new PromotionSet() { User = user, ShouldHaveRank = Rank.OG, DaystillRank = 9999, UserId = user.Id, DaysInDiscord = daysFromJoining });
                        }
                        else if (daysFromJoining >= daysIn6Months)
                        {
                            result.Add(new PromotionSet() { User = user, ShouldHaveRank = Rank.Sergeant, DaystillRank = daysInYear - daysFromJoining, UserId = user.Id, DaysInDiscord = daysFromJoining });
                        }
                        else if (daysFromJoining >= daysIn3Months)
                        {
                            result.Add(new PromotionSet() { User = user, ShouldHaveRank = Rank.Corperal, DaystillRank = daysIn6Months - daysFromJoining, UserId = user.Id, DaysInDiscord = daysFromJoining });
                        }
                        else if (daysFromJoining >= daysIn1Month)
                        {
                            result.Add(new PromotionSet() { User = user, ShouldHaveRank = Rank.Recruit, DaystillRank = daysIn3Months - daysFromJoining, UserId = user.Id, DaysInDiscord = daysFromJoining });
                        }
                        else
                        {
                            result.Add(new PromotionSet() { User = user, ShouldHaveRank = Rank.Smiley, DaystillRank = daysIn1Month - daysFromJoining, UserId = user.Id, DaysInDiscord = daysFromJoining });
                        }
                    }
                    else
                    {
                        result.Add(new PromotionSet() { User = user, ShouldHaveRank = Rank.Inactive, DaystillRank = -1, UserId = user.Id, DaysInDiscord = daysInactive });
                    }
                }
            }
            return result;
        }

        private int GetRoleRank(Rank rank)
        {
            switch (rank)
            {
                case Rank.OG:
                    return 100;
                case Rank.Sergeant:
                    return 80;
                case Rank.Corperal:
                    return 50;
                case Rank.Recruit:
                    return 30;
                case Rank.Smiley:
                    return 10;
                case Rank.Inactive:
                    return -1;
                default:
                    return -100;
            }
        }

        private string GetRoleIcon(Rank rank)
        {
            switch (rank)
            {
                case Rank.OG:
                    return Constants.OGImage;
                case Rank.Sergeant:
                    return Constants.SergeantImage;
                case Rank.Corperal:
                    return Constants.CorporalImage;
                case Rank.Recruit:
                    return Constants.RecruitImage;
                case Rank.Smiley:
                    return Constants.SmileyImage;
                case Rank.Inactive:
                    return Constants.InactiveImage;
                default:
                    return string.Empty;
            }
        }

        private List<PromotionSet> GetCurrentRank(List<PromotionSet> users, Discord.IGuild guild)
        {
            var result = new List<PromotionSet>();
            var roleId_OG = guild.Roles.FirstOrDefault(x => x.Name == "OG");
            var roleId_Sergeant = guild.Roles.FirstOrDefault(x => x.Name == "Sergeant");
            var roleId_Corporal = guild.Roles.FirstOrDefault(x => x.Name == "Corporal");
            var roleId_Recruit = guild.Roles.FirstOrDefault(x => x.Name == "Recruit");
            var roleId_Smiley = guild.Roles.FirstOrDefault(x => x.Name == "Smiley");
            var roleId_Inactive = guild.Roles.FirstOrDefault(x => x.Name == "Inactive");
            foreach (var user in users)
            {
                if (user.User.RoleIds.ToList().Contains(roleId_Inactive.Id))
                {
                    user.CurrentRank = Rank.Inactive;
                    result.Add(user);
                }
                else if (roleId_OG != null && user.User.RoleIds.ToList().Contains(roleId_OG.Id))
                {
                    user.CurrentRank = Rank.OG;
                    result.Add(user);
                }
                else if (roleId_Sergeant != null && user.User.RoleIds.ToList().Contains(roleId_Sergeant.Id))
                {
                    user.CurrentRank = Rank.Sergeant;
                    result.Add(user);
                }
                else if (roleId_Corporal != null && user.User.RoleIds.ToList().Contains(roleId_Corporal.Id))
                {
                    user.CurrentRank = Rank.Corperal;
                    result.Add(user);
                }
                else if (roleId_Recruit != null && user.User.RoleIds.ToList().Contains(roleId_Recruit.Id))
                {
                    user.CurrentRank = Rank.Recruit;
                    result.Add(user);
                }
                else if (roleId_Smiley != null && user.User.RoleIds.ToList().Contains(roleId_Smiley.Id))
                {
                    user.CurrentRank = Rank.Smiley;
                    result.Add(user);
                }
                else if (roleId_Smiley != null && user.User.RoleIds.ToList().Contains(roleId_Smiley.Id))
                {
                    user.CurrentRank = Rank.Smiley;
                    result.Add(user);
                }
            }
            return result;
        }

        private bool IsInactive(Discord.IGuildUser user, out int daysInactive)
        {
            daysInactive = -1;
            var name = DiscordHelper.GetAccountNameOrNickname(user);
            var womMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == name.ToLower());

            if (womMember != null && womMember.lastChangedAt.HasValue)
            {
                daysInactive = Convert.ToInt32(DateTime.Now.Subtract(womMember.lastChangedAt.Value).TotalDays);
                return daysInactive > 31;
            }
            return false;
        }

        private bool HasStaffOrModOrOwnerRole(Discord.IGuildUser user, Discord.IGuild guild)
        {
            return 
                DiscordHelper.HasRole(user, guild, "Staff") || 
                DiscordHelper.HasRole(user, guild, "Moderator") || 
                DiscordHelper.HasRole(user, guild, "Clan Owner");
        }
    }

    public class PromotionSet
    {
        public Rank ShouldHaveRank { get; set; }
        public Rank CurrentRank { get; set; }
        public int DaystillRank { get; set; }
        public int DaysInDiscord { get; set; }
        public Discord.IGuildUser User { get; set; }
        public ulong UserId { get; set; }
    }

    public enum Rank
    {
        Default,
        OG,
        Sergeant,
        Corperal,
        Recruit,
        Smiley,
        Inactive
    }
}