using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
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
        public async Task RollNumberAsync()
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var random = new Random();

                int randomInt = random.Next(1, 6);

                await Context.Channel.SendMessageAsync($"{DiscordNameHelper.GetAccountNameOrNickname(Context.User)} rolled a {randomInt}! (1 - 6)");
            }
        }
    }
}


