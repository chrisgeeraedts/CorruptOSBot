using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using CorruptOSBot.Helpers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    [RequireOwner(Group = "Staff")]
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("channelid")]
        [Summary("(admin) Gets the current channel's Id")]
        public async Task SaychannelIdAsync()
        {
            if (RootAdminManager.GetToggleState("channelid"))
            {
                var channel = Context.Channel.Id.ToString();

                await ReplyAsync(channel);

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }


        [Command("guildid")]
        [Summary("(admin) Gets the current guild Id")]
        public async Task SayguildidAsync()
        {
            if (RootAdminManager.GetToggleState("guildid"))
            {
                var channel = Context.Guild.Id.ToString();

                await ReplyAsync(channel);

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }



        [Command("test")]
        [Summary("(admin) A test command hosting different functionality - only used during development")]
        public async Task SayTestAsync()
        {
            if (RootAdminManager.GetToggleState("test"))
            {
                //await ReplyAsync(embed: CreateEmbedForMessage("of the abbys"));

            }
        }

        
    }
}
