using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Modules;
using CorruptOSBot.Services;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace CorruptOSBot
{
    internal class SotWService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 60 * 8; } // every 8 hours
        public int BeforeTriggerTimeInMS { get => 1000 * 60 * 3; } // 3 minutes
        public int WOMWaitTriggerTimeInMS { get => 1000 * 60 * 15; } // 15 minutes
        private ulong GuildId;


        public SotWService(Discord.IDiscordClient client)
        {
            Program.Log(new LogMessage(LogSeverity.Info, "SotWService", "Created, triggering every " + TriggerTimeInMS + "MS"));
            GuildId = ConfigHelper.GetGuildId();
        }

        public async Task Trigger(IDiscordClient client)
        {
            if (ToggleStateManager.GetToggleState(nameof(SotWService)))
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "SotWService", "Triggered"));

                var WOMClient = new WiseOldManClient();
                if (WOMClient.UpdateAllParticipants())
                {

                    System.Threading.Thread.Sleep(WOMWaitTriggerTimeInMS);

                    var guild = await client.GetGuildAsync(GuildId);

                    var channel = await guild.GetChannelAsync(ChannelHelper.GetChannelId(SettingsConstants.ChannelEventGeneral)) as SocketTextChannel;

                    if (channel != null)
                    {

                        await (channel as SocketTextChannel).SendMessageAsync(embed: EmbedHelper.CreateWOMEmbedSotw());

                    }
                }
            }
        }
    }
}