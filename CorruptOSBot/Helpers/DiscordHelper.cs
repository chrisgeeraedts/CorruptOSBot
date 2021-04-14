﻿using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace CorruptOSBot.Helpers
{
    public static class DiscordHelper
    {
        public static List<string> DiscordUsers { get; set; }


        public static string GetAccountNameOrNickname(SocketUser user)
        {
            var currentUser = ((SocketGuildUser)user);
            var name = currentUser.Nickname ?? user.Username;
            return name;
        }

        public static string GetAccountNameOrNickname(SocketGuildUser user)
        {
            var currentUser = user;
            var name = currentUser.Nickname ?? user.Username;
            return name;
        }

        public static string GetAccountNameOrNickname(IGuildUser user)
        {
            var currentUser = user;
            var name = currentUser.Nickname ?? user.Username;
            return name;
        }

        public static bool HasRole(IGuildUser user, IGuild guild, string roleName)
        {
            var roleId_Inactive = guild.Roles.FirstOrDefault(x => x.Name.ToLower() == roleName.ToLower());
            return user.RoleIds.ToList().Contains(roleId_Inactive.Id);
        }
    }
}