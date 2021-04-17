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

    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [Summary("(admin) A test command hosting different functionality - only used during development")]
        public async Task SayTestAsync()
        {
            if (RootAdminManager.GetToggleState("test") && RootAdminManager.HasSpecificRole(Context.User, "Staff"))
            {
                
                //await DiscordHelper.SendWelcomeMessageToUser(Context.User, Context.Guild);




                //var builder = new EmbedBuilder();
                //builder.Color = Color.Blue;
                //builder.Title = string.Format("{0} Top 5 {1} kc", EmojiHelper.GetFullEmojiString(EmojiEnum.tob), "Scorpia");

                //builder.AddField("\u200b", string.Format("{0}\u200b **{1}**", "Player 1", "155"));
                //builder.AddField("\u200b", string.Format("{0}\u200b **{1}**", "Player 2", "145"));
                //builder.AddField("\u200b", string.Format("{0}\u200b **{1}**", "Player 3", "135"));
                //builder.AddField("\u200b", string.Format("{0}\u200b **{1}**", "Player 4", "125"));
                //builder.AddField("\u200b", string.Format("{0}\u200b **{1}**", "Player 5", "115"));

                //await ReplyAsync(embed: builder.Build());
            }
        }










        [Command("postid")]
        [Summary("(admin) Gets the current post's Id")]
        public async Task SaypostidAsync()
        {
            if (RootAdminManager.GetToggleState("postid"))
            {

                var messages = await Context.Channel
                   .GetMessagesAsync(Context.Message, Direction.Before, 1)
                   .FlattenAsync();

                var message = messages.FirstOrDefault();
                if (message != null)
                {
                    var messageId = ((IUserMessage)message).Id;

                    await ReplyAsync(messageId.ToString());
                }

                

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

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
    }
}
