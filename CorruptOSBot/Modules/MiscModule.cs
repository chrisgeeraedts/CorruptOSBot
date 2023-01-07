using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class MiscModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("roll")]
        [Summary("!roll - Rolls a random number")]
        public async Task RollNumberAsync(int maxNumber)
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var random = new Random();

                int randomInt = random.Next(0, maxNumber);

                await Context.Channel.SendMessageAsync($"{Context.User.Username} rolled a {randomInt}! (1 - {maxNumber})");
            }
        }
    }
}


