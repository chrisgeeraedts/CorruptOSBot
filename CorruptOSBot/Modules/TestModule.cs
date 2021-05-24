using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord.Commands;
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
            var guildId = ConfigHelper.GetGuildId();
            var guild = ((Discord.IDiscordClient)Context.Client).GetGuildAsync(guildId).Result;

            foreach (var item in guild.Roles)
            {
                await ReplyAsync(string.Format("{0} ({1})", item.Name, item.Id));
            }


            //await CommandHelper.ActWithLoadingIndicator(Method2, "1232", Context.Channel);

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
