using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Events
{
    public static class EventManager
    {
        public static async Task JoinedGuild(SocketGuildUser arg)
        {
            var recruitingChannel = arg.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("User entered Discord",
                string.Format("<@{0}> ({0}) has joined the server", arg.Id)));
        }

        public static async Task LeavingGuild(SocketGuildUser arg)
        {
            var recruitingChannel = arg.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member left",
                string.Format("<@{0}> ({0}) has left the server", arg.Id)));

            var rsn = DiscordHelper.GetAccountNameOrNickname(arg);

            if (!string.IsNullOrEmpty(rsn))
            {
                new WiseOldManClient().RemoveGroupMember(rsn);
            }
        }

        public static async Task BannedFromGuild(SocketUser arg1, SocketGuild arg2)
        {
            var recruitingChannel = arg2.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));
            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member banned",
                string.Format("<@{0}>  ({0}) has been banned from the server", arg1.Id)));
        }
    }
}
