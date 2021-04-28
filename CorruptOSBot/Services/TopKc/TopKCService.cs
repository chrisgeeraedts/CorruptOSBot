using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Helpers.PVM;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Services
{

    public class TopKCService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 60 * 24 * 3; } // every 3  days
        private ulong GuildId;


        public TopKCService(Discord.IDiscordClient client)
        {
            Program.Log(new LogMessage(LogSeverity.Info, "TopKCService", "Created, triggering every " + TriggerTimeInMS + "MS"));
            GuildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
        }

        public async Task Trigger(IDiscordClient client)
        {
            if (ToggleStateManager.GetToggleState(nameof(TopKCService)))
            {
                // find current channel
                var guild = await client.GetGuildAsync(GuildId);
                var channel = await guild.GetChannelAsync(ChannelHelper.GetChannelId("top-boss-kc")) as SocketTextChannel;

                // first update base data
                await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneDayMS);

                // clear messages in channel
                var messages = await channel.GetMessagesAsync(3).FlattenAsync();
                await channel.DeleteMessagesAsync(messages);

                // Add new message
                await (channel as SocketTextChannel).SendMessageAsync(embed: await CreateFullLeaderboardEmbed());
            }
        }

        private async Task <Embed> CreateFullLeaderboardEmbed()
        {
            // connect the kcs per boss
            var result = await BossKCHelper.GetTopBossKC(WOMMemoryCache.OneDayMS);            

            var builder = new EmbedBuilder();
            builder.Color = Color.DarkGreen;

            BuildSet(result.Take(15), builder);
            BuildSet(result.Skip(15).Take(15), builder);
            BuildSet(result.Skip(30).Take(15), builder);

            builder.Title = "Top boss KC";
            builder.WithFooter(string.Format("Last updated: {0} | next update at: {1}", DateTime.Now, DateTime.Now.Add(TimeSpan.FromMilliseconds(TriggerTimeInMS))));

            return builder.Build();
        }


        private void BuildSet(IEnumerable<KcTopList> result, EmbedBuilder builder)
        {
            // First column
            var sb = new StringBuilder();
            foreach (var item in result)
            {
                if (item.KcPlayers.Count() > 0)
                {
                    sb.AppendLine(string.Format("{0} {1} {2} **({3})**", EmojiHelper.GetFullEmojiString(item.Boss), "\U0001f947", item.KcPlayers.Skip(0).First().Player, item.KcPlayers.Skip(0).First().Kc));
                }
                else
                {
                    sb.AppendLine(string.Format("{0} {1} {2}", EmojiHelper.GetFullEmojiString(item.Boss), "\U0001f947", "---"));
                }
            }
            builder.AddField("\u200b", sb.ToString(), true);

            // Second column
            var sb2 = new StringBuilder();
            foreach (var item in result)
            {
                if (item.KcPlayers.Count() > 1)
                {
                    sb2.AppendLine(string.Format("{0} {1} ({2})", "\U0001f948", item.KcPlayers.Skip(1).First().Player, item.KcPlayers.Skip(1).First().Kc));
                }
                else
                {
                    sb2.AppendLine(string.Format("{0} {1}", "\U0001f948", "---"));
                }
            }
            builder.AddField("\u200b", sb2.ToString(), true);

            // Second column
            var sb3 = new StringBuilder();
            foreach (var item in result)
            {
                if (item.KcPlayers.Count() > 1)
                {
                    sb3.AppendLine(string.Format("{0} {1} ({2})", "\U0001f949", item.KcPlayers.Skip(2).First().Player, item.KcPlayers.Skip(2).First().Kc));
                }
                else
                {
                    sb3.AppendLine(string.Format("{0} {1}", "\U0001f949", "---"));
                }

            }
            builder.AddField("\u200b", sb3.ToString(), true);
        }

        
    }
}
