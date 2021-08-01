using CorruptOSBot.Modules;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CorruptOSBot.Bingo
{
    public static class ModuleInjector
    {
        public static string Title = "BINGO v0.1";

        public static void Inject(Dictionary<ulong, Func<SocketMessage,
            Discord.IDiscordClient,
            Task>> result,
            IServiceProvider _services,
            Discord.Commands.CommandService _commands)
        {;
            _commands.AddModuleAsync<BingoModule>(_services);
        }
    }
}
