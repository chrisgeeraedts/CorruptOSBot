using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class RSNModule : ModuleBase<SocketCommandContext>
    {
        [Command("rsn")]
        [Summary("(your new name) - changes your nickname in the server.")]
        public async Task SayRSNAsync(string username)
        {

            //user that actually started the command
            var currentUser = Context.User;

            // the prefered rsn name the user posted
            var preferedNickname = username;

            // check rank, if he has a smiley, its a namechange, otherwise, a new member
            var hasSmileyRole = ((SocketGuildUser)currentUser).Roles.Any(x => x.Name == "Smiley");

            if (!hasSmileyRole)
            {
                await CreateNewMember(currentUser, preferedNickname);
            }
            else
            {
                await NameChangeMember(currentUser, preferedNickname);
            }
            

        }

        private async Task NameChangeMember(SocketUser currentUser, string preferedNickname)
        {
            string previousName = ((SocketGuildUser)currentUser).Nickname;

            // change nickname
            await ((SocketGuildUser)currentUser).ModifyAsync(x =>
            {
                x.Nickname = preferedNickname;
            });

            // post to recruiting channel
            var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == 827967957371060264);
            await ((IMessageChannel)recruitingChannel).SendMessageAsync(embed: EmbedHelper.CreateDefaultEmbed("Member name change",
                string.Format("{0} has changed their name to <@{1}>", previousName, preferedNickname)));

            // update WOM
            new WiseOldManClient().PostNameChange(previousName, preferedNickname);

            // delete the command posted
            await Context.Message.DeleteAsync();           
        }

        private async Task CreateNewMember(SocketUser currentUser, string preferedNickname)
        {
            // check runewatch
            bool isSafeAccount = true;

            if (isSafeAccount)
            {
                // update the role
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Smiley");
                await ((SocketGuildUser)currentUser).AddRoleAsync(role);

                // change nickname
                await ((SocketGuildUser)currentUser).ModifyAsync(x =>
                {
                    x.Nickname = preferedNickname;
                });

                // post to general channel
                var generalChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == 827967957694283797);
                await ((IMessageChannel)generalChannel).SendMessageAsync(string.Format("<@{0}> Welcome to Corrupt OS", currentUser.Id));

                // post to general channel
                var recruitingChannel = Context.Guild.Channels.FirstOrDefault(x => x.Id == 827967957371060264);
                await ((IMessageChannel)recruitingChannel).SendMessageAsync(string.Format("{0} has joined!", preferedNickname));

                // add to WOM
                new WiseOldManClient().AddGroupMember(preferedNickname);

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
            else
            {
                // kick user

                // post that user got kicked due to runewatch
            }
        }
    }
}
