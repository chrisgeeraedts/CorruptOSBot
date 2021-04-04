using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [Summary("test")]
        public async Task SayTestAsync()
        {
            //await ReplyAsync(embed: EmbedHelper.CreateDefaultEmbed("testTitle", "http://wwww.google.com"));

            Clan clan = new WiseOldManClient().GetClan(128);

            //Competition comp =
            //    new WiseOldManClient().GetCompetitions(2085);


            await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(clan.name, 
                "https://wiseoldman.net/groups/128",
                new System.Collections.Generic.Dictionary<string, string> {
                { "description (from WOM)",clan.description},
                { "homeworld (from WOM)" ,clan.homeworld.ToString()},
                { "members (from WOM)" ,clan.memberCount.ToString()},
                }));
        }

        [Command("setrank")]
        [Summary("setrank")]
        public async Task SaySetRankAsync(Discord.IGuildUser user)
        {
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Smiley");
            if (role != null && user != null)
            {
                await user.AddRoleAsync(role);
                await ReplyAsync("Role " + role + " added to " + user.Username);
            }
        }

        [Command("testRSN")]
        [Summary("testRSN")]
        public async Task SaySetRank2Async(string username)
        {
            
           
        }


        [Command("getrank")]
        [Summary("getrank")]
        public async Task SayGetRankAsync(Discord.IGuildUser user)
        {
            //var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Smiley");



            // get roles of mentioned player
            string result = string.Empty;
            foreach (var item in ((SocketGuildUser)user).Roles)
            {
                if (!item.Name.StartsWith("@"))
                {
                    result += item.Name + ";";
                }
            }
            

            await Context.Channel.SendMessageAsync(result);

        }
    }
}
