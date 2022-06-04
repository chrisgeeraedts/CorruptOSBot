using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CorruptOSBot.Services
{
    internal class SotWService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 60 * 8; } // every 8 hours
        public int BeforeTriggerTimeInMS { get => 1000 * 60 * 3; } // 3 minutes
        public int WOMWaitTriggerTimeInMS { get => 1000 * 60 * 15; } // 15 minutes

        private ulong GuildId;

        public SotWService(IDiscordClient client)
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

                    var channel = (SocketTextChannel)await guild.GetChannelAsync(ChannelHelper.GetChannelId(SettingsConstants.ChannelEventLeaderboard));

                    if (channel != null)
                    {
                        await channel.SendMessageAsync(embed: EmbedHelper.CreateWOMEmbedSotw());
                    }
                }
            }
        }
    }
}