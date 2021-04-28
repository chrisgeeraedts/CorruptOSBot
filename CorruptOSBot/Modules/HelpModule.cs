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
        [Command("help")]
        [Summary("Gives information about the bot.")]
        public async Task SayAsync()
        {
            if (ToggleStateManager.GetToggleState("help", Context.User) && RootAdminManager.HasAnyRole(Context.User))
            {
                await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultFieldsEmbed(
                "Corrupt OS bot command list",
                GetCommandsToShowInHelp(Context.User, Context.Guild)));

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        private Dictionary<string, string> GetCommandsToShowInHelp(SocketUser user, SocketGuild guild)
        {
            var isStaffOrDev = DiscordHelper.HasRole(user, guild, "Staff") || DiscordHelper.HasRole(user, guild, "Developer");


            List<string> blackListedCommands = new List<string>();
            blackListedCommands.Add("!clear");
            blackListedCommands.Add("!toggle");
            blackListedCommands.Add("!togglestates");
            blackListedCommands.Add("!channelid");
            blackListedCommands.Add("!overthrownathan");
            blackListedCommands.Add("!postid");

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var command in CommandHelper.GetEnabledCommandsFromCode())
            {
                if (!blackListedCommands.Contains(command.Key))
                {
                    if ((command.Value.Contains("(Staff)") || command.Value.Contains("(Dev)")) && isStaffOrDev)
                    {
                        result.Add(command.Key, command.Value);
                    }
                    else if(!command.Value.Contains("(Staff)") && !command.Value.Contains("(Dev)"))
                    {
                        result.Add(command.Key, command.Value);
                    }
                }               
            }

            return result;
        }
    }
}
