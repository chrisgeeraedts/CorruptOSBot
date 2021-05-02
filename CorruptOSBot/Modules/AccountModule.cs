using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
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
        public async Task SayAddAltBAsync([Remainder] string rsn)
        {
            if (ToggleStateManager.GetToggleState("add-alt", Context.User))
            {
                await AddAlt(Context, rsn, "alt");
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        [Command("add-iron")]
        [Summary("!add-iron {rsn} - Adds an runescape iron account to your discord account")]
        public async Task SayAddIronAsync([Remainder] string rsn)
        {
            if (ToggleStateManager.GetToggleState("add-iron", Context.User))
            {
                await AddAlt(Context, rsn, "iron");
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }


        private async Task AddAlt(SocketCommandContext context, string rsn, string type)
        {
            try
            {
                using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
                {
                    var discordUser = DataHelper.GetDiscordUserFromUserId(context.User.Id);
                    var isRsnAlreadyLinked = corruptosEntities.RunescapeAccounts.Any(x => x.rsn == rsn && x.DiscordUser != null && x.DiscordUserId != discordUser.DiscordId);
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
                                wom_id = womId,
                                account_type = type,
                            });

                        }
                    }
                    // add and message if it doesnt
                    await context.User.SendMessageAsync(String.Format("Rsn {0} was linked to your account!", rsn));

                    // Save
                    await corruptosEntities.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "AddAlt", "Failed adding alt to database - " + e.Message));
            }

        }
    }
}
