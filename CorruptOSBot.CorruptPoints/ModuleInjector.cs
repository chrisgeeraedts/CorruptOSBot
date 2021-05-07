using CorruptOSBot.CorruptPoints.Modules;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CorruptOSBot.CorruptPoints
{
    public static class ModuleInjector
    {
        public static string Title = "Corrupt Points v0.3";

        public static void Inject(Dictionary<ulong, Func<SocketMessage,
            Discord.IDiscordClient,
            Task>> result,
            IServiceProvider _services,
            Discord.Commands.CommandService _commands)
        {;
            _commands.AddModuleAsync<PointModule>(_services);
        }
    }
}
