using Discord.WebSocket;
using System.Threading.Tasks;

namespace CorruptOSBot.Shared
{
    public interface IInterceptor
    {
        Task Trigger(SocketMessage arg, Discord.IDiscordClient client);
    }
}