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
        [Command("channelId")]
        [Summary("channelId")]
        public async Task SaychannelIdAsync()
        {
            var channel = Context.Channel.Id.ToString();

            await ReplyAsync(channel);

            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        [Command("guildid")]
        [Summary("guildid")]
        public async Task SayguildidAsync()
        {
            var channel = Context.Guild.Id.ToString();

            await ReplyAsync(channel);

            // delete the command posted
            await Context.Message.DeleteAsync();
        }



        [Command("test")]
        [Summary("test")]
        public async Task SayTestAsync(string testmessage)
        {
            // post to general channel
            var channelId = ChannelHelper.GetChannelId("general");
            var generalChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == channelId);
            await ((IMessageChannel)generalChannel).SendMessageAsync("test to recruitment with channel logic enabled");


            var channel2Id = ChannelHelper.GetChannelId("recruiting");
            var recruitmentChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == channel2Id);
            await ((IMessageChannel)recruitmentChannel).SendMessageAsync("test to general with channel logic enabled");



            //await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("title", testmessage));
        }

   
        public void foo()
        {
            ////await ReplyAsync(embed: EmbedHelper.CreateDefaultEmbed("testTitle", "http://wwww.google.com"));

            //Clan clan = new WiseOldManClient().GetClan(128);

            ////Competition comp =
            ////    new WiseOldManClient().GetCompetitions(2085);


            //await Context.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed(clan.name,
            //    "https://wiseoldman.net/groups/128",
            //    new System.Collections.Generic.Dictionary<string, string> {
            //    { "description (from WOM)",clan.description},
            //    { "homeworld (from WOM)" ,clan.homeworld.ToString()},
            //    { "members (from WOM)" ,clan.memberCount.ToString()},
            //    }));
        }
    }
}
