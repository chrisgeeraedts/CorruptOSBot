using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace CorruptOSBot.Helpers
{

    public class PvmSet
    {
        public string learner { get; set; }
        public string intermediate { get; set; }
        public string advanced { get; set; }

        public string imageUrl { get; set; }
    }
    public static class PvmSystemManager
    {
        private static int intermediateRole = 50;
        private static int advancedRole = 250;
        internal static void CheckAndUpdateAccount(IGuildUser currentUser, IGuild guild, int kills, PvmSet pvmSet, bool message, bool overrideTrigger)
        {
            var learnerRole = guild.Roles.FirstOrDefault(x => x.Name == pvmSet.learner);
            var IntermediateRoleId = guild.Roles.FirstOrDefault(x => x.Name == pvmSet.intermediate);
            var AdvancedRoleId = guild.Roles.FirstOrDefault(x => x.Name == pvmSet.advanced);

            var haslearnerRole = currentUser.RoleIds.Any(x => x == learnerRole.Id);
            var hasIntermediate = currentUser.RoleIds.Any(x => x == IntermediateRoleId.Id);
            var hasAdvanced = currentUser.RoleIds.Any(x => x == AdvancedRoleId.Id);

            if (overrideTrigger || (haslearnerRole || hasIntermediate || hasAdvanced))
            {
                if (kills > advancedRole && !hasAdvanced)
                {
                    // upgrade role
                    SetRole(currentUser, guild, pvmSet.advanced, AdvancedRoleId.Id, pvmSet.imageUrl, message);
                    RemoveRole(currentUser, guild, IntermediateRoleId);
                    RemoveRole(currentUser, guild, learnerRole);
                    currentUser.SendMessageAsync(String.Format("You just got the {0} role!", pvmSet.advanced));
                }
                else if (kills > intermediateRole && !hasIntermediate && !hasAdvanced)
                {
                    // upgrade role
                    SetRole(currentUser, guild, pvmSet.intermediate, IntermediateRoleId.Id, pvmSet.imageUrl, message);
                    RemoveRole(currentUser, guild, learnerRole);
                    currentUser.SendMessageAsync(String.Format("You just got the {0} role!", pvmSet.intermediate));
                }
                else if (overrideTrigger || (!haslearnerRole && !hasIntermediate && !hasAdvanced))
                {
                    // upgrade role
                    SetRole(currentUser, guild, pvmSet.learner, IntermediateRoleId.Id, pvmSet.imageUrl, false);
                    currentUser.SendMessageAsync(String.Format("You just got the {0} role!", pvmSet.learner));
                }
            }
        }

        private static void RemoveRole(IGuildUser currentUser, IGuild guild, IRole role)
        {
            try
            {
                currentUser.RemoveRoleAsync(role);
                Console.WriteLine("PVMRoleService: Removed role for:" + currentUser.Nickname);
            }
            catch (Exception)
            {

            }
        }
        private static void SetRole(IGuildUser currentUser, IGuild guild, string roleName, ulong roleId, string imageUrl, bool showMessage)
        {
            var role = guild.Roles.FirstOrDefault(x => x.Name == roleName);
            currentUser.AddRoleAsync(role);
            string message = string.Format("<@{0}> just got promoted to <@&{1}>!", currentUser.Id, roleId);
            Console.WriteLine("PVMRoleService: Updated role for:" + currentUser.Nickname);
            if (showMessage)
            {
                var pvmgeneralChannel = guild.GetChannelsAsync().Result.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("pvm-general"));
                ((IMessageChannel)pvmgeneralChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("PVM promotion!", message,
                    imageUrl));
            }

        }
    }
}
