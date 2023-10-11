using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CorruptOSBot.Services
{
    public class TopKCService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 60 * 24 * 3; } // every 3  days
        public int BeforeTriggerTimeInMS { get => 1000 * 60 * 3; } // 3 minutes

        private ulong GuildId;

        public TopKCService(Discord.IDiscordClient client)
        {
            Program.Log(new LogMessage(LogSeverity.Info, "TopKCService", "Created, triggering every " + TriggerTimeInMS + "MS"));
            GuildId = ConfigHelper.GetGuildId();
        }

        public async Task Trigger(IDiscordClient client)
        {
            if (ToggleStateManager.GetToggleState(nameof(TopKCService)))
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "TopKCService", "Triggered"));

                // find current channel
                var guild = await client.GetGuildAsync(GuildId);
                var channel = await guild.GetChannelAsync(ChannelHelper.GetChannelId("top-boss-kc")) as SocketTextChannel;

                if (channel != null)
                {
                    // first update base data
                    await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneDayMS);

                    // clear messages in channel
                    var messages = await channel.GetMessagesAsync(3).FlattenAsync();
                    await channel.DeleteMessagesAsync(messages);

                    // Add new message
                    var embedMessageList = await EmbedHelper.CreateFullLeaderboardEmbed(TriggerTimeInMS);

                    foreach(var embedMessage in embedMessageList)
                    {
                        await channel.SendMessageAsync(embed: embedMessage);
                    }
                }
                else
                {
                    await Program.Log(new LogMessage(LogSeverity.Info, "TopKCService", "Trigger failed"));
                }
            }
        }
    }
}