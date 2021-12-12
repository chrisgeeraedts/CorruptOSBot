using Discord;
using Discord.WebSocket;
using System.Collections.Generic;

namespace CorruptOSBot.Helpers.Discord
{
    public static partial class DiscordNameHelper
    {
        public static List<string> DiscordUsers { get; set; }

        public static string GetAccountNameOrNickname(SocketUser user)
        {
            var currentUser = (user as SocketGuildUser);
            if (currentUser == null)
            {
                var name = user.Username;
                return name;
            }
            else
            {
                var name = currentUser.Nickname ?? user.Username;
                return name;
            }
            return string.Empty;
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
    }
}