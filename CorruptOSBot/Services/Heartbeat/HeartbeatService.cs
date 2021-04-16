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
        public int TriggerTimeInMS { get => 1000 * 60; } // every minute
        private IGuild Guild { get; set; }
        private IMessageChannel Channel { get; set; }

        public HeartbeatService(Discord.IDiscordClient client)
        {
            Task.Delay(10000).ContinueWith(t => Setup(client));            
        }

        public async Task Trigger(Discord.IDiscordClient client)
        {
            if (RootAdminManager.GetToggleState(nameof(HeartbeatService)) && Channel != null)
            {
                await DiscordHelper.PostHeartbeat(Channel);
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
                }
            }
        }
    }
}
