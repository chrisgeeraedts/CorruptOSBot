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
    internal class AchievementService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 60 * 24; } // Once a day
        public int BeforeTriggerTimeInMS { get => 1000 * 60 * 3; } // 3 minute
        private ulong GuildId;

        public AchievementService(IDiscordClient client)
        {
            Program.Log(new LogMessage(LogSeverity.Info, "AchievementService", "Created, triggering every " + TriggerTimeInMS + "MS"));
            GuildId = ConfigHelper.GetGuildId();
        }

        public async Task Trigger(IDiscordClient client)
        {
            if (ToggleStateManager.GetToggleState(nameof(AchievementService)))
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "AchievementService", "Triggered"));

                // find current channel
                var guild = await client.GetGuildAsync(GuildId);
                var channel = await guild.GetChannelAsync(ChannelHelper.GetChannelId(SettingsConstants.ChannelAchievements)) as SocketTextChannel;

                // Add new message
                if (channel != null)
                {
                    var embed = EmbedHelper.CreateWOMEmbed();
                    if (embed != null)
                    {
                        await channel.SendMessageAsync(embed: embed);
                    }
                }
            }
        }
    }
}