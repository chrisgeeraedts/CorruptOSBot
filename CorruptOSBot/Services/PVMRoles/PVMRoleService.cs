using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using Discord;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Services
{
    public class PVMRoleService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 5; }
        private ulong GuildId;


        public PVMRoleService(Discord.IDiscordClient client)
        {
            Program.Log(new LogMessage(LogSeverity.Info, "PVMRoleService", "Created, trigering every " + TriggerTimeInMS + "MS"));
            GuildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
        }

        public async Task Trigger(Discord.IDiscordClient client)
        {
            if (RootAdminManager.GetToggleState(nameof(PVMRoleService)))
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "PVMRoleService", "Triggered"));

                try
                {
                    var guild = client.GetGuildAsync(GuildId).Result;

                    // Get all players in WOM
                    await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneHourMS);
                    var clanMembers = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails;
                    await Program.Log(new LogMessage(LogSeverity.Info, "PVMRoleService", "Loaded clanmembers"));

                    // iterate through all discord users
                    var allUsers = guild.GetUsersAsync().Result;
                    foreach (var discordUser in allUsers)
                    {
                        var name = DiscordHelper.GetAccountNameOrNickname(discordUser);
                        var clanMemberWom = clanMembers.FirstOrDefault(x => x.displayName.ToLower() == name.ToLower());

                        if (clanMemberWom != null)
                        {
                            // check if role change is needed
                            if (discordUser != null)
                            {
                                try
                                {
                                    await SetRolesCox(discordUser, guild, clanMemberWom.latestSnapshot.chambers_of_xeric.kills);
                                }
                                catch (Exception e)
                                {
                                    await Program.Log(new LogMessage(LogSeverity.Error, "PVMRoleService", "Failed at CoX roles: " + e.Message));
                                }

                                try
                                {
                                    await SetRolesTob(discordUser, guild, clanMemberWom.latestSnapshot.theatre_of_blood.kills);
                                }
                                catch (Exception e)
                                {
                                    await Program.Log(new LogMessage(LogSeverity.Error, "PVMRoleService", "Failed at Tob roles: " + e.Message));
                                }

                                try
                                {
                                    await SetRolesNm(discordUser, guild, clanMemberWom.latestSnapshot.nightmare.kills);
                                }
                                catch (Exception e)
                                {
                                    await Program.Log(new LogMessage(LogSeverity.Error, "PVMRoleService", "Failed at Nm roles: " + e.Message));
                                }
                            }
                        }
                    }


                    await Program.Log(new LogMessage(LogSeverity.Info, "PVMRoleService", "Completed"));
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Error, "PVMRoleService", "Failed: " + e.Message));
                }
            }
        }


        private async Task SetRolesCox(IGuildUser currentUser, IGuild guild, int kills)
        {
            await PvmSystemHelper.CheckAndUpdateAccountAsync(
                currentUser,
                guild,
                kills,
                new PvmSet()
                {
                    learner = Constants.CoxLearner,
                    intermediate = Constants.CoxIntermediate,
                    advanced = Constants.CoxAdvanced,
                    imageUrl = Constants.CoxImage
                },
                true,
                false);
        }

        private async Task SetRolesTob(IGuildUser currentUser, IGuild guild, int kills)
        {
            await PvmSystemHelper.CheckAndUpdateAccountAsync(
                currentUser,
                guild,
                kills,
                new PvmSet()
                {
                    learner = Constants.TobLearner,
                    intermediate = Constants.ToBIntermediate,
                    advanced = Constants.ToBAdvanced,
                    imageUrl = Constants.TobImage
                },
                true,
                false);
        }

        private async Task SetRolesNm(IGuildUser currentUser, IGuild guild, int kills)
        {
            await PvmSystemHelper.CheckAndUpdateAccountAsync(
                currentUser,
                guild,
                kills,
                new PvmSet()
                {
                    learner = Constants.NmLearner,
                    intermediate = Constants.NmIntermediate,
                    advanced = Constants.NmAdvanced,
                    imageUrl = Constants.nmImage
                },
                true,
                false);
        }
    }
}
