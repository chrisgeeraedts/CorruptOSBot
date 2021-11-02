using CorruptOSBot.Data;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Shared.Helpers.Discord
{
    public static class RoleHelper
    {
        private static Dictionary<string, Role> KnownRoles = new Dictionary<string, Role>();

        public static bool HasAnyRole(SocketUser user)
        {
            return ((SocketGuildUser)user).Roles.Any();
        }

        public static ulong GetRoleId(string name)
        {
            FillList();

            if (KnownRoles.ContainsKey(name.ToLower()))
            {
                return Convert.ToUInt64(KnownRoles[name.ToLower()].DiscordRoleId);
            }
            return 0;
        }

        public static bool IsMember(SocketUser user, SocketGuild guild)
        {
            var roles = RoleHelper.GetRoles();

            return HasRole(user, guild, roles.FirstOrDefault(x => x.Id == 7)) ||  //Rank 1
            HasRole(user, guild, roles.FirstOrDefault(x => x.Id == 8)) ||  //Rank 2
            HasRole(user, guild, roles.FirstOrDefault(x => x.Id == 9)) ||  //Rank 3
            HasRole(user, guild, roles.FirstOrDefault(x => x.Id == 10)) || //Rank 4
            HasRole(user, guild, roles.FirstOrDefault(x => x.Id == 11)) || //Rank 5
            HasRole(user, guild, roles.FirstOrDefault(x => x.Id == 12)) || //Rank 6
            HasRole(user, guild, roles.FirstOrDefault(x => x.Id == 14)) || //SOTW
            HasRole(user, guild, roles.FirstOrDefault(x => x.Id == 15)) || //BOTW
            HasRole(user, guild, roles.FirstOrDefault(x => x.Id == 17)) || //Rank 10
            HasRole(user, guild, roles.FirstOrDefault(x => x.Id == 18));   //Rank 7
        }
        public static bool HasStaffOrModOrOwnerRole(IGuildUser user, IGuild guild)
        {
            foreach (var item in RoleHelper.GetRoles().Where(x => x.IsStaff))
            {
                var hasStaff = HasRole(user, guild, item);
                if (hasStaff)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasStaffOrModOrOwnerRole(SocketUser user, IGuild guild)
        {
            return HasStaffOrModOrOwnerRole(((IGuildUser)user), guild);
        }

  

        public static bool HasRole(IGuildUser user, IGuild guild, Role role)
        {
            var discordRole = guild.Roles.FirstOrDefault(x => x.Id == Convert.ToUInt64(role.DiscordRoleId));
            if (discordRole != null)
            {
                return user.RoleIds.ToList().Contains(discordRole.Id);
            }
            return false;
        }

        public static bool HasRole(IGuildUser user, IGuild guild, long roleId)
        {
            var dbDiscordRole = GetRoles().FirstOrDefault(x => x.Id == roleId);
            var discordRole = guild.Roles.FirstOrDefault(x => x.Id == Convert.ToUInt64(dbDiscordRole.DiscordRoleId));
            return user.RoleIds.ToList().Contains(discordRole.Id);
        }

        public static bool HasRole(SocketUser user, IGuild guild, Role role)
        {
            return HasRole(((IGuildUser)user), guild, role);
        }

        public static bool HasRole(SocketUser user, IGuild guild, long roleId)
        {
            return HasRole(((IGuildUser)user), guild, roleId);
        }

        public static List<Role> GetRoles()
        {
            FillList();

            return KnownRoles.Values.ToList();
        }

        private static void FillList()
        {
            if (KnownRoles.Count == 0)
            {
                var roles = new DataHelper().GetRoles();
                foreach (var role in roles)
                {
                    KnownRoles.Add(role.Name.ToLower(), role);
                }
            }
        }
    }
}
