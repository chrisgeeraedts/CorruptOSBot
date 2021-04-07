using CorruptOSBot.Extensions;
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
            Program.Log(new LogMessage(LogSeverity.Info, string.Empty, "PVMRoleService: created, trigering every " + TriggerTimeInMS + "MS"));
            GuildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
        }

        public async Task Trigger(Discord.IDiscordClient client)
        {
            if (RootAdminManager.GetToggleState(nameof(PVMRoleService)))
            {
                await Program.Log(new LogMessage(LogSeverity.Info, string.Empty, "PVMRoleService: triggered"));

                try
                {
                    var WomClient = new WiseOldManClient();
                    var guild = client.GetGuildAsync(GuildId).Result;

                    // Get all players in WOM
                    var clanMembers = WomClient.GetClanMembers(128);
                    await Program.Log(new LogMessage(LogSeverity.Info, string.Empty, "PVMRoleService: Loaded clanmembers"));

                    // iterate through all discord users
                    var allUsers = guild.GetUsersAsync().Result;
                    foreach (var discordUser in allUsers.Where(x => !string.IsNullOrEmpty(x.Nickname)))
                    {
                        var clanMemberWom = clanMembers.FirstOrDefault(x => x.displayName.ToLower() == discordUser.Nickname.ToLower());

                        if (clanMemberWom != null)
                        {
                            // Per WOM player, get the boss kc
                            var details = WomClient.GetPlayerDetails(clanMemberWom.id);

                            await Program.Log(new LogMessage(LogSeverity.Info, string.Empty, "PVMRoleService: Loaded details for :" + details.displayName));

                            // check if role change is needed
                            if (discordUser != null)
                            {
                                try
                                {
                                    await SetRolesCox(discordUser, guild, details.latestSnapshot.chambers_of_xeric.kills);
                                }
                                catch (Exception e)
                                {
                                    await Program.Log(new LogMessage(LogSeverity.Error, string.Empty, "PVMRoleService failed at CoX roles: " + e.Message));
                                }

                                try
                                {
                                    await SetRolesTob(discordUser, guild, details.latestSnapshot.theatre_of_blood.kills);
                                }
                                catch (Exception e)
                                {
                                    await Program.Log(new LogMessage(LogSeverity.Error, string.Empty, "PVMRoleService failed at Tob roles: " + e.Message));
                                }

                                try
                                {
                                    await SetRolesNm(discordUser, guild, details.latestSnapshot.nightmare.kills);
                                }
                                catch (Exception e)
                                {
                                    await Program.Log(new LogMessage(LogSeverity.Error, string.Empty, "PVMRoleService failed at Nm roles: " + e.Message));
                                }
                            }
                        }
                    }


                    Program.Log(new LogMessage(LogSeverity.Info, string.Empty, "PVMRoleService completed"));
                }
                catch (Exception e)
                {
                    Program.Log(new LogMessage(LogSeverity.Error, string.Empty, "PVMRoleService failed: " + e.Message));
                }
            }
        }


        private async Task SetRolesCox(IGuildUser currentUser, IGuild guild, int kills)
        {
            await PvmSystemManager.CheckAndUpdateAccountAsync(
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
            await PvmSystemManager.CheckAndUpdateAccountAsync(
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
            await PvmSystemManager.CheckAndUpdateAccountAsync(
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
