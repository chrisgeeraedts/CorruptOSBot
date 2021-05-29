using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Helpers.Discord
{
    public static partial class DiscordHelper
    {
        public static List<string> DiscordUsers { get; set; }

        internal static bool IsInChannel(ulong channelId, string channelName, SocketUser userAdditional = null)
        {
            // override for admin
            if (userAdditional != null && userAdditional.Id == 174621705581494272) //override ID
            {
                return true;
            }

            return ChannelHelper.GetChannelId(channelName) == channelId;
        }

        public static async Task<IGuildUser> AsyncFindUserByName(string username, global::Discord.Commands.SocketCommandContext context)
        {
            var guildId = ConfigHelper.GetGuildId();
            var guild = await ((IDiscordClient)context.Client).GetGuildAsync(guildId);
            var allUsers = await guild.GetUsersAsync();
            return allUsers.FirstOrDefault(x => DiscordNameHelper.GetAccountNameOrNickname(x).ToLower() == username.ToLower());
        }

        public static async Task<IGuildUser> AsyncFindUserByMention(string mention, global::Discord.Commands.SocketCommandContext context)
        {
            var guildId = ConfigHelper.GetGuildId();
            var guild = await ((IDiscordClient)context.Client).GetGuildAsync(guildId);
            var allUsers = await guild.GetUsersAsync();
            return allUsers.FirstOrDefault(x => x.Mention == mention);
        }
        internal static async Task PostComeOnline(IMessageChannel channel)
        {
            await (channel).SendMessageAsync(string.Format("Bot (v{1}) **Online** : [{0}] ", DateTime.Now, RootAdminManager.GetBotVersion()));
        }

        internal static async Task PostHeartbeat(IMessageChannel channel, TimeSpan timeOnline)
        {
            await (channel).SendMessageAsync(string.Format("Bot (v{2}) online since {3} for **{0}** minutes | **Heartbeat** : [{1}] ", Convert.ToInt64(timeOnline.TotalMinutes), DateTime.Now, RootAdminManager.GetBotVersion(), Program.OnlineFrom.ToString("r")));
        }

        
        internal static async Task NotAlloweddMessageToUser(SocketUser user, string command, string allowedChannel)
        {
            await ((SocketGuildUser)user).SendMessageAsync(string.Format("That command (**{0}**) is **not allowed** in this channel but only in the following channel(s): **{1}**!", command, allowedChannel));
        }

        internal static async Task SendWelcomeMessageToUser(SocketUser user, SocketGuild guild, bool isClanFriend)
        {
            var eb = new EmbedBuilder();
            eb.Title = "Welcome to Corrupt OS";

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Hi **{0}**, and welcome to Corrupt OS!", DiscordNameHelper.GetAccountNameOrNickname(user)));
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("**Important channels:**");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(string.Format("**<#{0}>**", ChannelHelper.GetChannelId("Announcements")));
            sb.AppendLine("This will tell you what is happening and what changes we are making.");
            sb.AppendLine(string.Format("**<#{0}>**", ChannelHelper.GetChannelId("Suggestions")));
            sb.AppendLine("Help shape the clan and give us new ways we can make events more entertaining!");
            sb.AppendLine(string.Format("**<#{0}>**", ChannelHelper.GetChannelId("Support")));
            sb.AppendLine("Any issues you can open a ticket and we will be with you ASAP");
            sb.AppendLine(string.Format("**<#{0}>**", ChannelHelper.GetChannelId("Set-Pvm-Roles")));
            sb.AppendLine("Doing the relevant command will allow you to be pinged to join a team for PvM content. ");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("We have made our own bot for the discord. It has really helpful commands. To see these type **!help** in our Discord's channels");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("We hope you have a great time and join in with our **Skill of the week** starting **Sunday 12pm UK** time till **Friday 12pm UK** time. Please always watch out **event-announcement** for upcoming pvm and skill events!");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("Feel free to message **SGnathy** with any questions!");

            eb.Footer = new EmbedFooterBuilder().WithText("Corrupt OS | Bot");
            eb.Description = sb.ToString();
            eb.ThumbnailUrl = guild.IconUrl;

            await ((SocketGuildUser)user).SendMessageAsync(embed: eb.Build());
        }

        public static bool TryGetEntireCommand(string commandString, string[] commandKeys, char splitter, out Dictionary<string, string> result)
        {
            result = new Dictionary<string, string>();
            var splittedString = commandString.Split(splitter);
            var index = 0;
            foreach (var item in splittedString)
            {
                if (commandKeys.Length >= index)
                {
                    var key = commandKeys[index];
                    var value = item;
                    result.Add(key, value);
                }
                index++;
            }
            return true;
        }
    }
}
