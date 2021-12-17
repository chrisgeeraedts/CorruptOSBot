using CorruptOSBot.Data;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var roles = GetRoles();
            var result = false;

            for (var i = 1; i < 13; i++)
            {
                result = HasRole(user, guild, roles.FirstOrDefault(x => x.Id == i));
            }

            return result;
        }

        public static bool IsStaff(SocketUser user, IGuild guild)
        {
            foreach (var item in GetRoles().Where(x => x.IsStaff))
            {
                if (HasRole(user, guild, item))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasRole(SocketUser user, IGuild guild, Role role)
        {
            var discordRole = guild.Roles.FirstOrDefault(x => x.Id == Convert.ToUInt64(role.DiscordRoleId));

            if (discordRole != null)
            {
                var guildUser = (IGuildUser)user;

                return guildUser.RoleIds.ToList().Contains(discordRole.Id);
            }

            return false;
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