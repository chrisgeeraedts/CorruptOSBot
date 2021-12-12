using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using CorruptOSBot.Shared.Helpers.Discord;
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
            if (ToggleStateManager.GetToggleState("help", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                var embeds = EmbedHelper.CreateDefaultFieldsEmbed(
                "Affliction bot command list",
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
            var isStaff = RoleHelper.IsStaff(user, guild);
            var isMember = RoleHelper.IsMember(user, guild);

            List<string> blackListedCommands = new List<string>();
            blackListedCommands.Add("!overthrownathan");

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var command in CommandHelper.GetEnabledCommandsFromCode())
            {
                if (!blackListedCommands.Contains(command.Key))
                {
                    if (command.Value.HelpGroup == HelpGroup.Admin)
                    {
                        result.Add(command.Key, command.Value.Name);
                    }
                    if (command.Value.HelpGroup == HelpGroup.Staff && isStaff)
                    {
                        result.Add(command.Key, command.Value.Name);
                    }
                    if (command.Value.HelpGroup == HelpGroup.Member && (isMember || isStaff))
                    {
                        result.Add(command.Key, command.Value.Name);
                    }
                }               
            }

            return result;
        }
    }
}
