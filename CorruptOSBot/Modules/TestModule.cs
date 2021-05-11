using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Helpers.PVM;
using CorruptOSBot.Shared;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{

    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("test")]
        [Summary("!test - TEST")]
        public async Task SayTestAsync()
        {
            await CommandHelper.ActWithLoadingIndicator(Method2, "1232", Context.Channel);

            // delete the command posted
            await Context.Message.DeleteAsync();
        }
        public async Task<int> Method2(string input)
        {
            System.Threading.Thread.Sleep(5000);
            return 1;
        }
    }
}
