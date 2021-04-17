using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using CorruptOSBot.Helpers;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Services
{

    public class HeartbeatService : IService
    {
        public TimeSpan TimeOnline { get; set; }
        public DateTime TimeStarted { get; set; }
        public int TriggerTimeInMS { get => 1000 * 60; } // every minute
        private IGuild Guild { get; set; }
        private IMessageChannel Channel { get; set; }

        public HeartbeatService(IDiscordClient client)
        {
            //Task.Delay(10000).ContinueWith(t => Setup(client));            
        }

        public async Task Trigger(IDiscordClient client)
        {
            if (RootAdminManager.GetToggleState(nameof(HeartbeatService)) && Channel != null)
            {
                TimeOnline = DateTime.Now.Subtract(TimeStarted);
                await DiscordHelper.PostHeartbeat(Channel, TimeOnline);
            }
            else if(Channel == null)
            {
                await Setup(client);
            }
        }

        private async Task Setup(Discord.IDiscordClient client)
        {
            var guildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
            Guild = client.GetGuildAsync(guildId).Result;
            if (Guild != null)
            {
                Channel = (IMessageChannel)Guild.GetChannelsAsync().Result.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("bot-heartbeat"));
                if (Channel != null)
                {
                    await DiscordHelper.PostComeOnline(Channel);
                    TimeStarted = DateTime.Now;
                }
            }
        }
    }
}
