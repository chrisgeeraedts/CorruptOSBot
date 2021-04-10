using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using Discord;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Services
{
    public class TopKCService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 60 * 24; } // every 24 hours
        private ulong GuildId;


        public TopKCService(Discord.IDiscordClient client)
        {
            Program.Log(new LogMessage(LogSeverity.Info, "TopKCService", "Created, triggering every " + TriggerTimeInMS + "MS"));
            GuildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
        }

        public async Task Trigger(Discord.IDiscordClient client)
        {
            if (RootAdminManager.GetToggleState(nameof(TopKCService)))
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "TopKCService", "Triggered"));

                try
                {
                    // Do stuff
                    await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneDayMS);
                    var clanMembers = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails;



                    await Program.Log(new LogMessage(LogSeverity.Info, "TopKCService", "Completed"));
                }
                catch (Exception e)
                {
                    await Program.Log(new LogMessage(LogSeverity.Error, "TopKCService", "Failed: " + e.Message));
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
