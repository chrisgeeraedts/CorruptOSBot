using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Services
{
    internal class CleanUpService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 15; } // Every 15 minutes
        public int BeforeTriggerTimeInMS { get => 1000 * 60 * 15; } // 15 minutes

        private ulong GuildId;

        public CleanUpService(IDiscordClient client)
        {
            Program.Log(new LogMessage(LogSeverity.Info, "PromotionService", "Created, triggering every " + TriggerTimeInMS + "MS"));
            GuildId = ConfigHelper.GetGuildId();
        }

        public async Task Trigger(IDiscordClient client)
        {
            var guild = await client.GetGuildAsync(GuildId);

            var currentVoiceChannels = await guild.GetVoiceChannelsAsync();
            var constantVoiceChannels = new List<string>()
            {
                "Staff",
                "General",
                "COX",
                "TOA",
                "TOB",
                "Join to create a VC"
            };

            foreach (var channel in currentVoiceChannels.Where(item => !constantVoiceChannels.Any(item.Name.Contains)))
            {
                if ((channel as SocketGuildChannel).Users.Count == 0)
                {
                    await channel.DeleteAsync();
                }
            }
        }
    }
}