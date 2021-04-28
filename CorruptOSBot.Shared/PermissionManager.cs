using Discord.WebSocket;
using System.Linq;

namespace CorruptOSBot.Shared
{
    public static class PermissionManager
    {
        public static bool HasSpecificRole(SocketUser user, string roleName)
        {
            return ((SocketGuildUser)user).Roles.Any(x => x.Name == roleName);
        }
    }
}
