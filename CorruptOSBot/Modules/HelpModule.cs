using CorruptOSBot.Helpers;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultFieldsEmbed(
                "Corrupt OS bot command list",
                "Global Commands:",
                CommandHelper.GetCommandsFromCode()));
        }

        

    }
}
