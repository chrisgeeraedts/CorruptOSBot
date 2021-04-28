using CorruptOSBot.Modules;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CorruptOSBot.TheHunt
{
    public static class ModuleInjector
    {
        public static void Inject(Dictionary<ulong, Func<SocketMessage,
            Discord.IDiscordClient,
            Task>> result,
            IServiceProvider _services,
            Discord.Commands.CommandService _commands)
        {
            SetToggles();
            AddHuntChannelInterceptors(result);
            _commands.AddModuleAsync<HuntModule>(_services);


            HuntManager.Init();
        }

        private static void SetToggles()
        {
            ToggleStateManager.ToggleStates.Add(nameof(PostCopyInterceptor), true);
        }

        private static void AddHuntChannelInterceptors(Dictionary<ulong, Func<SocketMessage, Discord.IDiscordClient, Task>> result)
        {
            var teamchannelIds = ConfigHelper.GetSettingProperty("thehuntsourcechannels");
            var teamchannelIdSplitted = teamchannelIds.Split(';');
            foreach (var teamchannelId in teamchannelIdSplitted)
            {
                ulong teamchannelIdAsUlong = Convert.ToUInt64(teamchannelId);
                result.Add(teamchannelIdAsUlong, new PostCopyInterceptor().Trigger);
            }
        }
    }
}
