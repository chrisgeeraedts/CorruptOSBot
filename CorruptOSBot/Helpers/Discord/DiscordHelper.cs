using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Helpers.Discord
{
    public static class DiscordHelper
    {
        public static List<string> DiscordUsers { get; set; }


        public static string GetAccountNameOrNickname(SocketUser user)
        {
            var currentUser = ((SocketGuildUser)user);
            var name = currentUser.Nickname ?? user.Username;
            return name;
        }

        public static string GetAccountNameOrNickname(SocketGuildUser user)
        {
            var currentUser = user;
            var name = currentUser.Nickname ?? user.Username;
            return name;
        }

        internal static bool IsInChannel(ulong channelId, string channelName, SocketUser userAdditional = null)
        {
            // override for admin
            if (userAdditional != null && userAdditional.Id == 174621705581494272)
            {
                return true;
            }

            return ChannelHelper.GetChannelId(channelName) == channelId;
        }

        public static string GetAccountNameOrNickname(IGuildUser user)
        {
            var currentUser = user;
            var name = currentUser.Nickname ?? user.Username;
            return name;
        }

        internal static async Task PostHeartbeat(IMessageChannel channel, TimeSpan timeOnline)
        {
            await (channel).SendMessageAsync(string.Format("Bot (v{2}) online for **{0}** minutes | **Heartbeat** : [{1}] ", Convert.ToInt64(timeOnline.TotalMinutes), DateTime.Now, RootAdminManager.GetBotVersion()));
        }

        internal static async Task PostComeOnline(IMessageChannel channel)
        {
            await (channel).SendMessageAsync(string.Format("Bot (v{1}) **Online** : [{0}] ", DateTime.Now, RootAdminManager.GetBotVersion()));
        }

        public static bool HasRole(IGuildUser user, IGuild guild, string roleName)
        {
            var roleId_Inactive = guild.Roles.FirstOrDefault(x => x.Name.ToLower() == roleName.ToLower());
            return user.RoleIds.ToList().Contains(roleId_Inactive.Id);
        }

        public static bool HasRole(SocketUser user, IGuild guild, string roleName)
        {
            return HasRole(((IGuildUser)user), guild, roleName);
        }

        internal static async Task NotAlloweddMessageToUser(SocketUser user, string command, string allowedChannel)
        {
            await ((SocketGuildUser)user).SendMessageAsync(string.Format("That command (**{0}**) is **not allowed** in this channel but only in the following channel(s): **{1}**!", command, allowedChannel));
        }

        internal static async Task SendWelcomeMessageToUser(SocketUser user, SocketGuild guild)
        {
            var eb = new EmbedBuilder();
            eb.Title = "Welcome to Corrupt OS";

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Hi **{0}**, and welcome to Corrupt OS!", GetAccountNameOrNickname(user)));
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("**Important channels:**");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(string.Format("**<#{0}>**", ChannelHelper.GetChannelId("Announcements")));
            sb.AppendLine("This will tell you what is happening and what changes we are making.");
            sb.AppendLine(string.Format("**<#{0}>**", ChannelHelper.GetChannelId("Suggestions")));
            sb.AppendLine("Help shape the clan and give us new ways we can make events more entertaining!");
            sb.AppendLine(string.Format("**<#{0}>**", ChannelHelper.GetChannelId("Support")));
            sb.AppendLine("Any issues you can open a ticket and we will be with you ASAP");
            sb.AppendLine(string.Format("**<#{0}>**", ChannelHelper.GetChannelId("Clan-News")));
            sb.AppendLine("Daily updates on what we are doing in the clan that some might not see");
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
    }
}
