using CorruptOSBot.Data;
using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Text;
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
            DiscordUser originalUser = null;
            // get the original name
            using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
            {
                var id = Convert.ToInt64(arg.Id);
                originalUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == id);
            }



            var recruitingChannel = arg.Guild.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("<@{0}> ({1}) has left the server", arg.Id, originalUser?.Username));
            sb.AppendLine(string.Format(Environment.NewLine));
            sb.AppendLine(string.Format("**Runescape accounts linked to this discord user:**"));
            if (originalUser != null)
            {
                foreach (var rsn in originalUser.RunescapeAccounts)
                {
                    sb.AppendLine(string.Format("- {0}", rsn.rsn));
                }
            }

            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member left",
                sb.ToString()));

            if (originalUser != null)
            {
                foreach (var rsn in originalUser.RunescapeAccounts)
                {
                    new WiseOldManClient().RemoveGroupMember(rsn.rsn);
                }
            }

            await new DataHelper().SetDiscorduserLeaving(arg.Id);
        }

        public static async Task BannedFromGuild(SocketUser arg1, SocketGuild arg2)
        {

            DiscordUser originalUser = null;
            // get the original name
            using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
            {
                var id = Convert.ToInt64(arg1.Id);
                originalUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == id);
            }


            var recruitingChannel = arg2.Channels.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("recruiting"));

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("<@{0}>  ({1}) has been banned from the server", arg1.Id, originalUser?.Username));
            sb.AppendLine(string.Format(Environment.NewLine));
            sb.AppendLine(string.Format("**Runescape accounts linked to this discord user:**"));

            if (originalUser != null)
            {
                foreach (var rsn in originalUser.RunescapeAccounts)
                {
                    sb.AppendLine(string.Format("- {0}", rsn.rsn));
                }
            }


            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member banned",
                sb.ToString()));

            await new DataHelper().SetDiscorduserLeaving(arg1.Id);

            if (originalUser != null)
            {
                foreach (var rsn in originalUser.RunescapeAccounts)
                {
                    new WiseOldManClient().RemoveGroupMember(rsn.rsn);
                }
            }
        }
    }
}
