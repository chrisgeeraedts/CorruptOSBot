using System;
using System.Linq;
using System.Threading.Tasks;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;

namespace CorruptOSBot.Helpers.PVM
{
    public static class PvmSystemHelper
    {
        private static int intermediateRole = 50;
        private static int advancedRole = 250;
        internal static async Task CheckAndUpdateAccountAsync(IGuildUser currentUser, IGuild guild, int kills, PvmSet pvmSet, bool message, bool overrideTrigger)
        {
            var learnerRole = guild.Roles.FirstOrDefault(x => x.Name.ToLower() == pvmSet.learner.ToLower());
            var IntermediateRoleId = guild.Roles.FirstOrDefault(x => x.Name.ToLower() == pvmSet.intermediate.ToLower());
            var AdvancedRoleId = guild.Roles.FirstOrDefault(x => x.Name.ToLower() == pvmSet.advanced.ToLower());

            var haslearnerRole = currentUser.RoleIds.Any(x => x == learnerRole.Id);
            var hasIntermediate = currentUser.RoleIds.Any(x => x == IntermediateRoleId.Id);
            var hasAdvanced = currentUser.RoleIds.Any(x => x == AdvancedRoleId.Id);

            if (overrideTrigger || (haslearnerRole || hasIntermediate || hasAdvanced))
            {
                if (kills >= advancedRole && !hasAdvanced)
                {
                    // upgrade role
                    await SetRole(currentUser, guild, pvmSet.advanced, AdvancedRoleId.Id, pvmSet.imageUrl, message);
                    await RemoveRole(currentUser, guild, IntermediateRoleId);
                    await RemoveRole(currentUser, guild, learnerRole);
                    await currentUser.SendMessageAsync(String.Format("You just got the {0} role!", pvmSet.advanced));
                }
                else if (kills >= intermediateRole && !hasIntermediate && !hasAdvanced)
                {
                    // upgrade role
                    await SetRole(currentUser, guild, pvmSet.intermediate, IntermediateRoleId.Id, pvmSet.imageUrl, message);
                    await RemoveRole(currentUser, guild, learnerRole);
                    await currentUser.SendMessageAsync(String.Format("You just got the {0} role!", pvmSet.intermediate));
                }
                else if ((overrideTrigger && !haslearnerRole && !hasIntermediate && !hasAdvanced) || (!haslearnerRole && !hasIntermediate && !hasAdvanced))
                {
                    // upgrade role
                    await SetRole(currentUser, guild, pvmSet.learner, IntermediateRoleId.Id, pvmSet.imageUrl, false);
                    await currentUser.SendMessageAsync(String.Format("You just got the {0} role!", pvmSet.learner));
                }
                else
                {
                    if (overrideTrigger && hasAdvanced)
                    {
                        await currentUser.SendMessageAsync(String.Format("You already have the {0} role!", pvmSet.advanced));
                    }
                    else if (overrideTrigger && hasIntermediate)
                    {
                        await currentUser.SendMessageAsync(String.Format("You already have the {0} role!", pvmSet.intermediate));
                    } else if (overrideTrigger && haslearnerRole)
                    {
                        await currentUser.SendMessageAsync(String.Format("You already have the {0} role!", pvmSet.learner));
                    }
                }
            }
        }


        internal static async Task CheckAndUpdateAccountCMAsync(IGuildUser currentUser, IGuild guild, int kills, PvmSetCM pvmSet, bool message, bool overrideTrigger)
        {
            var roleId = guild.Roles.FirstOrDefault(x => x.Name.ToLower() == pvmSet.role.ToLower());

            var hasRole = currentUser.RoleIds.Any(x => x == roleId.Id);

            if (overrideTrigger || (hasRole))
            {
                if (kills > 0)
                {
                    if (!hasRole)
                    {
                        // upgrade role
                        await SetRole(currentUser, guild, pvmSet.role, roleId.Id, pvmSet.imageUrl, message);
                        await currentUser.SendMessageAsync(String.Format("You just got the {0} role!", pvmSet.role));
                    }
                    else
                    {
                        if (overrideTrigger && hasRole)
                        {
                            await currentUser.SendMessageAsync(String.Format("You already have the {0} role!", pvmSet.role));
                        }
                    }
                }
                else
                {
                    await currentUser.SendMessageAsync(String.Format("You dont have enough KC for the {0} role!", pvmSet.role));
                }
            }
        }

        internal static async Task CheckAndUpdateAccountNoKCAsync(IGuildUser currentUser, IGuild guild, PvmSetCM pvmSet, bool message, bool overrideTrigger)
        {
            var roleId = guild.Roles.FirstOrDefault(x => x.Name.ToLower() == pvmSet.role.ToLower());

            var hasRole = currentUser.RoleIds.Any(x => x == roleId.Id);

            if (overrideTrigger || hasRole)
            {
                if (!hasRole)
                {
                    // upgrade role
                    await SetRole(currentUser, guild, pvmSet.role, roleId.Id, pvmSet.imageUrl, message);
                    await currentUser.SendMessageAsync(String.Format("You just got the {0} role!", pvmSet.role));
                }
                else
                {
                    if (overrideTrigger && hasRole)
                    {
                        await currentUser.SendMessageAsync(String.Format("You already have the {0} role!", pvmSet.role));
                    }
                }
            }
        }

        private static async Task RemoveRole(IGuildUser currentUser, IGuild guild, IRole role)
        {
            try
            {
                await currentUser.RemoveRoleAsync(role);
                await Program.Log(new LogMessage(LogSeverity.Info, "PVMRoleService", "Removed role for:" + DiscordNameHelper.GetAccountNameOrNickname(currentUser)));
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "PVMRoleService", "Failed to remove role - " + e.Message));
            }
        }
        private static async Task SetRole(IGuildUser currentUser, IGuild guild, string roleName, ulong roleId, string imageUrl, bool showMessage)
        {
            try
            {
                var role = guild.Roles.FirstOrDefault(x => x.Name.ToLower() == roleName.ToLower());
                await currentUser.AddRoleAsync(role);
                await Program.Log(new LogMessage(LogSeverity.Info, "PVMRoleService", "Updated role for:" + DiscordNameHelper.GetAccountNameOrNickname(currentUser)));
                if (showMessage)
                {
                    var pvmgeneralChannel = guild.GetChannelsAsync().Result.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("pvm-general"));
                    await ((IMessageChannel)pvmgeneralChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(string.Format("PVM promotion for {0}!", DiscordNameHelper.GetAccountNameOrNickname(currentUser)),
                        string.Format("<@{0}> just got promoted to <@&{1}>!", currentUser.Id, roleId),
                        imageUrl, "https://static.wikia.nocookie.net/getsetgames/images/8/82/Level_up_icon.png/revision/latest?cb=20130804113035"));
                }
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "PVMRoleService", "Failed to remove role - " + e.Message));
            }
        }
    }
}
