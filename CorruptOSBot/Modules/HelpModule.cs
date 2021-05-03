using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("help")]
        [Summary("!help - Gives information about the bot commands for your roles.")]
        public async Task SayAsync()
        {
            if (ToggleStateManager.GetToggleState("help", Context.User) && RootAdminManager.HasAnyRole(Context.User))
            {
                var embeds = EmbedHelper.CreateDefaultFieldsEmbed(
                "Corrupt OS bot command list",
                GetCommandsToShowInHelp(Context.User, Context.Guild));

                foreach (var embed in embeds)
                {
                    await Context.Channel.SendMessageAsync(embed: embed);
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        private Dictionary<string, string> GetCommandsToShowInHelp(SocketUser user, SocketGuild guild)
        {
            var isDev = DiscordHelper.HasRole(user, guild, "Developer");
            var isStaff = DiscordHelper.HasRole(user, guild, "Staff") ||
                DiscordHelper.HasRole(user, guild, "Clan Owner");
            var isMod = DiscordHelper.HasRole(user, guild, "Moderator");
            var isMember = DiscordHelper.HasRole(user, guild, "Smiley") || 
                DiscordHelper.HasRole(user, guild, "Recruit") || 
                DiscordHelper.HasRole(user, guild, "Sergeant") || 
                DiscordHelper.HasRole(user, guild, "Corporal") || 
                DiscordHelper.HasRole(user, guild, "OG");

            List<string> blackListedCommands = new List<string>();
            //blackListedCommands.Add("!clear");
            //blackListedCommands.Add("!toggle");
            //blackListedCommands.Add("!togglestates");
            //blackListedCommands.Add("!channelid");
            //blackListedCommands.Add("!postid");
            //blackListedCommands.Add("!guildid");
            //blackListedCommands.Add("!serverip");
            blackListedCommands.Add("!overthrownathan");

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var command in CommandHelper.GetEnabledCommandsFromCode())
            {
                if (!blackListedCommands.Contains(command.Key))
                {
                    if (command.Value.HelpGroup == HelpGroup.Admin && (isDev))
                    {
                        result.Add(command.Key, command.Value.Name);
                    }
                    if (command.Value.HelpGroup == HelpGroup.Staff && (isStaff || isDev))
                    {
                        result.Add(command.Key, command.Value.Name);
                    }
                    if (command.Value.HelpGroup == HelpGroup.Moderator && (isMod || isStaff || isDev))
                    {
                        result.Add(command.Key, command.Value.Name);
                    }
                    if (command.Value.HelpGroup == HelpGroup.Member && (isMember || isMod || isStaff || isDev))
                    {
                        result.Add(command.Key, command.Value.Name);
                    }
                }               
            }

            return result;
        }
    }
}
