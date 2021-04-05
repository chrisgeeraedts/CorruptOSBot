using CorruptOSBot.Helpers;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Events
{
    public static class EventManager
    {
        public static async Task LeavingGuild(SocketGuildUser arg)
        {
            var recruitingChannel = arg.Guild.Channels.FirstOrDefault(x => x.Id == 827967957371060264);
            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member left",
                string.Format("<@{0}> has left the server", arg.Id)));
        }
    }
}
