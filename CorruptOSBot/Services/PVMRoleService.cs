using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Services
{
    public class PVMRoleService : IService
    {
        public int TriggerTimeInMS { get => 300000; }
        private int intermediateRole = 50;
        private int advancedRole = 250;
        private ulong GuildId;


        public PVMRoleService(Discord.IDiscordClient client)
        {
            GuildId = Convert.ToUInt64(ConfigurationManager.AppSettings["GuildId"]);
        }

        public void Trigger(Discord.IDiscordClient client)
        {
            Console.WriteLine("trigger PVMRoleService");

            var WomClient = new WiseOldManClient();            
            var guild = client.GetGuildAsync(GuildId).Result;

            // Get all players in WOM
            var clanMembers = WomClient.GetClanMembers(128);

            // iterate through all discord users
            var allUsers = guild.GetUsersAsync().Result;
            foreach (var discordUser in allUsers.Where(x => !string.IsNullOrEmpty(x.Nickname)))
            {
                var clanMemberWom = clanMembers.FirstOrDefault(x => x.displayName.ToLower() == discordUser.Nickname.ToLower());

                if (clanMemberWom != null)
                {
                    // Per WOM player, get the boss kc
                    var details = WomClient.GetPlayerDetails(clanMemberWom.id);

                    // check if role change is needed
                    if (discordUser != null)
                    {
                        SetRolesCox(discordUser, guild, details.latestSnapshot.chambers_of_xeric.kills);
                        SetRolesTob(discordUser, guild, details.latestSnapshot.theatre_of_blood.kills);
                        SetRolesNm(discordUser, guild, details.latestSnapshot.nightmare.kills);
                    }
                }
            }
        }


        private void SetRolesCox(IGuildUser currentUser, IGuild guild, int kills)
        {
            var CoxlearnerRole = guild.Roles.FirstOrDefault(x => x.Name == Constants.CoxLearner);
            var hasCoxlearnerRole = currentUser.RoleIds.Any(x => x == CoxlearnerRole.Id);

            if (hasCoxlearnerRole)
            {
                var CoxIntermediateRoleId = guild.Roles.FirstOrDefault(x => x.Name == Constants.CoxIntermediate);
                var CoxAdvancedRoleId = guild.Roles.FirstOrDefault(x => x.Name == Constants.CoxAdvanced);

                if (CoxIntermediateRoleId != null && CoxAdvancedRoleId != null)
                {
                    var hasCoxIntermediate = currentUser.RoleIds.Any(x => x == CoxIntermediateRoleId.Id);
                    var hasCoxAdvanced = currentUser.RoleIds.Any(x => x == CoxAdvancedRoleId.Id);

                    if (kills > intermediateRole && !hasCoxIntermediate)
                    {
                        // upgrade role
                        SetRole(currentUser, guild, Constants.CoxIntermediate, CoxIntermediateRoleId.Id, Constants.CoxImage);
                    }

                    if (kills > advancedRole && !hasCoxAdvanced)
                    {
                        // upgrade role
                        SetRole(currentUser, guild, Constants.CoxAdvanced, CoxAdvancedRoleId.Id, Constants.CoxImage);
                    }
                }
            }
        }

        private void SetRolesTob(IGuildUser currentUser, IGuild guild, int kills)
        {
            var ToBlearnerRole = guild.Roles.FirstOrDefault(x => x.Name == Constants.TobLearner);
            var hasToBlearnerRole = currentUser.RoleIds.Any(x => x == ToBlearnerRole.Id);

            if (hasToBlearnerRole)
            {
                var ToBIntermediateRoleId = guild.Roles.FirstOrDefault(x => x.Name == Constants.ToBIntermediate);
                var ToBAdvancedRoleId = guild.Roles.FirstOrDefault(x => x.Name == Constants.ToBAdvanced);

                if (ToBIntermediateRoleId != null && ToBAdvancedRoleId != null)
                {
                    var hasToBIntermediate = currentUser.RoleIds.Any(x => x == ToBIntermediateRoleId.Id);
                    var hasToBAdvanced = currentUser.RoleIds.Any(x => x == ToBAdvancedRoleId.Id);

                    if (kills > intermediateRole && !hasToBIntermediate)
                    {
                        // upgrade role
                        SetRole(currentUser, guild, Constants.ToBIntermediate, ToBIntermediateRoleId.Id, Constants.TobImage);
                    }

                    if (kills > advancedRole && !hasToBAdvanced)
                    {
                        // upgrade role
                        SetRole(currentUser, guild, Constants.ToBAdvanced, ToBAdvancedRoleId.Id, Constants.TobImage);
                    }
                }
            }
        }

        private void SetRolesNm(IGuildUser currentUser, IGuild guild, int kills)
        {
            var NmlearnerRole = guild.Roles.FirstOrDefault(x => x.Name == Constants.NmLearner);
            var hasNmlearnerRole = currentUser.RoleIds.Any(x => x == NmlearnerRole.Id);

            if (hasNmlearnerRole)
            {
                var NmIntermediateRoleId = guild.Roles.FirstOrDefault(x => x.Name == Constants.NmIntermediate);
                var NmAdvancedRoleId = guild.Roles.FirstOrDefault(x => x.Name == Constants.NmAdvanced);

                if (NmIntermediateRoleId != null && NmAdvancedRoleId != null)
                {
                    var hasNmIntermediate = currentUser.RoleIds.Any(x => x == NmIntermediateRoleId.Id);
                    var hasNmAdvanced = currentUser.RoleIds.Any(x => x == NmAdvancedRoleId.Id);

                    if (kills > intermediateRole && !hasNmIntermediate)
                    {
                        // upgrade role
                        SetRole(currentUser, guild, Constants.NmIntermediate, NmIntermediateRoleId.Id, Constants.nmImage);
                    }

                    if (kills > advancedRole && !hasNmAdvanced)
                    {
                        // upgrade role
                        SetRole(currentUser, guild, Constants.NmAdvanced, NmAdvancedRoleId.Id, Constants.nmImage);
                    }
                }
            }
        }

        private void SetRole(IGuildUser currentUser, IGuild guild, string roleName, ulong roleId, string imageUrl)
        {
            var role = guild.Roles.FirstOrDefault(x => x.Name == roleName);
            currentUser.AddRoleAsync(role);

            var pvmgeneralChannel = guild.GetChannelsAsync().Result.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("pvm-general"));
            
            ((IMessageChannel)pvmgeneralChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("PVM promotion!", string.Format("<@{0}> just got promoted to <@&{1}>!", currentUser.Id, roleId),
                imageUrl));
        }
    }
}
