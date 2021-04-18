using CorruptOSBot.Helpers;
using Discord.Commands;
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
            if (RootAdminManager.GetToggleState("help", Context.User) && RootAdminManager.HasAnyRole(Context.User))
            {
                await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultFieldsEmbed(
                "Corrupt OS bot command list",
                "Global Commands:",
                GetCommandsToShowInHelp()));

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        private Dictionary<string, string> GetCommandsToShowInHelp()
        {
            List<string> blackListedCommands = new List<string>();
            blackListedCommands.Add("!clear");
            blackListedCommands.Add("!toggle");
            blackListedCommands.Add("!togglestates");
            blackListedCommands.Add("!channelid");

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var command in CommandHelper.GetEnabledCommandsFromCode())
            {
                if (!blackListedCommands.Contains(command.Key))
                {
                    result.Add(command.Key, command.Value);
                }               
            }

            return result;
        }
    }
}
