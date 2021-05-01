using CorruptOSBot.Extensions;
using CorruptOSBot.Shared;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class AccountModule : ModuleBase<SocketCommandContext>
    {

        [Command("add-alt")]
        [Summary("!add-alt {rsn} - Adds an runescape alt account to your discord account")]
        public async Task SayInitDBAsync([Remainder] string rsn)
        {
            if (ToggleStateManager.GetToggleState("add-alt", Context.User))
            {
                await AddAlt(Context, rsn);
            }
        }

        private async Task AddAlt(SocketCommandContext context, string rsn)
        {
            using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
            {
                long discordId = Convert.ToInt64(context.User.Id);

                // Find discord dataset
                var discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == discordId);
                var isRsnAlreadyLinked = corruptosEntities.RunescapeAccounts.Any(x => x.rsn == rsn && x.DiscordUser != null && x.DiscordUserId != discordId);
                // check if account is linked to rsn
                if (discordUser != null)
                {
                    if (isRsnAlreadyLinked)
                    {
                        await context.User.SendMessageAsync(String.Format("This rsn {0} is already linked to an an account!", rsn));
                    }
                    else if (discordUser.RunescapeAccounts.Any(x => x.rsn == rsn))
                    {
                        // message if it does

                        await context.User.SendMessageAsync(String.Format("You already have linked the rsn {0} to your account!", rsn));
                    }
                    else
                    {
                        // add it to WOM                
                        var client = new WiseOldManClient();
                        client.AddGroupMember(rsn);

                        var users = client.SearchUsersByName(rsn);
                        int? womId = null;
                        if (users.Count == 1)
                        {
                            womId = users.First().id;
                        }

                        // add it to db
                        corruptosEntities.RunescapeAccounts.Add(new Data.RunescapeAccount()
                        {
                            DiscordUser = discordUser,
                            rsn = rsn,
                            wom_id = womId
                        });

                    }
                }
                // add and message if it doesnt
                await context.User.SendMessageAsync(String.Format("Rsn {0} was linked to your account!", rsn));

                // Save
                await corruptosEntities.SaveChangesAsync();
            }
        }
    }
}
