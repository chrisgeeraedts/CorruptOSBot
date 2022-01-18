using CorruptOSBot.Data;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Services
{
    internal class PromotionService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 60 * 24; } // Once a day
        //public int BeforeTriggerTimeInMS { get => 1000 * 60 * 3; } // 3 minutes

        public int BeforeTriggerTimeInMS { get => 1000 * 5; } // 3 minutes

        private ulong GuildId;

        private static List<DiscordUser> PromotedUsers { get; set; } = new List<DiscordUser>();

        public PromotionService(IDiscordClient client)
        {
            Program.Log(new LogMessage(LogSeverity.Info, "PromotionService", "Created, triggering every " + TriggerTimeInMS + "MS"));
            GuildId = ConfigHelper.GetGuildId();
        }

        public async Task Trigger(IDiscordClient client)
        {
            var currentDateTime = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);

            await Program.Log(new LogMessage(LogSeverity.Error, "PromotionService", $"Promotion Service was triggered at {currentDateTime:G}"));

            if (currentDateTime.Date == firstDayOfMonth.Date)
            {
                var guild = await client.GetGuildAsync(GuildId);

                using (CorruptModel corruptosEntities = new CorruptModel())
                {
                    var botConfigs = corruptosEntities.BotConfigurations.ToList();
                    var lastPromoRunConfig = botConfigs.FirstOrDefault(item => item.PropertyName == SettingsConstants.LastPromotionRun);

                    var lastPromoRunDateTime = DateTime.Parse(lastPromoRunConfig.PropertyValue);

                    // Incase of reboot on first day of the month
                    if (lastPromoRunDateTime.Date != currentDateTime.Date)
                    {
                        foreach (var user in corruptosEntities.DiscordUsers.Where(item => !item.BlacklistedForPromotion))
                        {
                            if (user.OriginallyJoinedAt.HasValue)
                            {
                                var joinDate = user.OriginallyJoinedAt.Value;
                                var daysInClan = (currentDateTime.Date - joinDate.Date).TotalDays;

                                if (daysInClan > 30)
                                {
                                    var pointsGained = user.Role.IsStaff ? SettingsConstants.StaffPointsPerMonth : SettingsConstants.NormalPointsPerMonth;

                                    user.Points += pointsGained;

                                    if (user.DiscordId.HasValue)
                                    {
                                        var discordUser = await guild.GetUserAsync((ulong)user.DiscordId);

                                        if (discordUser != null)
                                        {
                                            await UpdateDiscordUserRole(user, corruptosEntities, guild, discordUser);
                                        }
                                    }
                                }
                            }
                        }

                        var rolesWithPromotions = PromotedUsers.DistinctBy(item => item.Role.Name).Select(item => item.Role);
                        var channels = corruptosEntities.Channels.ToList();

                        foreach (var role in rolesWithPromotions)
                        {
                            var promotedUsersInRole = PromotedUsers.Where(item => item.Role.Name == role.Name);

                            var promotedString = new StringBuilder();

                            promotedUsersInRole.ForEach(item => promotedString.AppendLine($"<@{item.DiscordId}>"));

                            var rankRequestsChannel = channels.FirstOrDefault(item => item.Name == "rank-requests");
                            var rankRequestsDiscordChannel = await guild.GetChannelAsync((ulong)rankRequestsChannel.DiscordChannelId);

                            await ((IMessageChannel)rankRequestsDiscordChannel).SendMessageAsync(
                                embed: EmbedHelper.CreateDefaultEmbed($"Users promoted to {role.Name}!",
                                promotedString.ToString(),
                                role.IconUrl, "https://static.wikia.nocookie.net/getsetgames/images/8/82/Level_up_icon.png/revision/latest?cb=20130804113035")
                                );
                        }

                        // dd/mm/yyyy
                        lastPromoRunConfig.PropertyValue = currentDateTime.ToString("d");
                    }

                    await corruptosEntities.SaveChangesAsync();
                }
            }
        }

        private static async Task UpdateDiscordUserRole(DiscordUser user, CorruptModel corruptosEntities, IGuild guild, IGuildUser discordUser)
        {
            var roles = corruptosEntities.Roles.ToList();

            var roleToBeApplied = roles.FirstOrDefault(item => user.Points >= item.PointsToReach && user.Points <= item.MaximumPoints);

            if (roleToBeApplied.Id != user.RoleId && !user.Role.IsStaff)
            {
                var discordRole = guild.Roles.FirstOrDefault(item => item.Name == roleToBeApplied.Name);

                await discordUser.RemoveRoleAsync((ulong)user.Role.DiscordRoleId);
                await discordUser.AddRoleAsync(discordRole);

                user.RoleId = roleToBeApplied.Id;

                PromotedUsers.Add(user);
            }
        }
    }
}