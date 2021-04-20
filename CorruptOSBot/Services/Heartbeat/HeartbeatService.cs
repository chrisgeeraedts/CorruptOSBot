using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using Discord;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Services
{

    public class HeartbeatService : IService
    {
        public TimeSpan TimeOnline { get; set; }
        public DateTime TimeStarted { get; set; }
        public int TriggerTimeInMS { get => 1000 * 60; } // every minute
        private IMessageChannel Channel { get; set; }

        public HeartbeatService(IDiscordClient client)
        {
            //Task.Delay(10000).ContinueWith(t => Setup(client));            
        }

        public async Task Trigger(IDiscordClient client)
        {
            if (RootAdminManager.GetToggleState(nameof(HeartbeatService)))
            {
                if (Channel != null)
                {
                    TimeOnline = DateTime.Now.Subtract(TimeStarted);
                    await DiscordHelper.PostHeartbeat(Channel, TimeOnline);
                }
                else if (Channel == null)
                {
                    await Setup(client);
                }
            }
        }

        private async Task Setup(Discord.IDiscordClient client)
        {
            var guildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
            var guild = client.GetGuildAsync(guildId).Result;
            if (guild != null)
            {
                Channel = (IMessageChannel)guild.GetChannelsAsync().Result.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("bot-heartbeat"));
                if (Channel != null)
                {
                    await DiscordHelper.PostComeOnline(Channel);
                    TimeStarted = DateTime.Now;
                }
            }
        }
    }
}
