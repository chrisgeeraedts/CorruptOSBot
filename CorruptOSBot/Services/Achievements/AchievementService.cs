using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Services;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace CorruptOSBot
{
    internal class AchievementService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 60 * 24; } // every 3  days
        private ulong GuildId;


        public AchievementService(Discord.IDiscordClient client)
        {
            Program.Log(new LogMessage(LogSeverity.Info, "AchievementService", "Created, triggering every " + TriggerTimeInMS + "MS"));
            GuildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
        }

        public async Task Trigger(IDiscordClient client)
        {
            if (RootAdminManager.GetToggleState(nameof(AchievementService)))
            {
                // find current channel
                var guild = await client.GetGuildAsync(GuildId);
                var channel = await guild.GetChannelAsync(ChannelHelper.GetChannelId("achievements")) as SocketTextChannel;

                // Add new message
                if (channel != null)
                {
                    await (channel as SocketTextChannel).SendMessageAsync(embed: EmbedHelper.CreateWOMEmbed());
                }
            }
        }
    }
}