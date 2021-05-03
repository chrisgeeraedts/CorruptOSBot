using CorruptOSBot.Data;
using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers.Discord;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Helpers
{
    public partial class DataHelper
    {
        public async Task AddNewDiscordUserAndRSN(SocketUser currentUser, string preferedNickname, ClanMember groupMember)
        {
            using (var corruptosEntities = new Data.CorruptModel())
            {
                var discordUser = new Data.DiscordUser()
                {
                    DiscordId = Convert.ToInt64(currentUser.Id),
                    Username = preferedNickname,
                    OriginallyJoinedAt = DateTime.Now
                };

                corruptosEntities.DiscordUsers.Add(discordUser);

                corruptosEntities.RunescapeAccounts.Add(new Data.RunescapeAccount()
                {
                    DiscordUser = discordUser,
                    rsn = DiscordHelper.GetAccountNameOrNickname(currentUser),
                    wom_id = groupMember?.id,
                    account_type = "main"
                });

                await corruptosEntities.SaveChangesAsync();
            }
        }

        public async Task SetDiscorduserLeaving(ulong userDiscordId)
        {

            try
            {
                using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
                {
                    long discordId = Convert.ToInt64(userDiscordId);

                    // Find discord dataset
                    var discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == discordId);

                    if (discordUser != null)
                    {
                        discordUser.LeavingDate = DateTime.Now;
                    }

                    await corruptosEntities.SaveChangesAsync();
                }

            }
            catch (System.Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "SetDiscorduserLeaving", "Failed add LeavingDate to database - " + e.Message));
            }


            
        }


        public Data.DiscordUser GetDiscordUserFromUserId(ulong? userId)
        {
            using (var corruptosEntities = new Data.CorruptModel())
            {
                long discordId = Convert.ToInt64(userId);

                // Find discord dataset
                var discordUser = corruptosEntities.DiscordUsers.FirstOrDefault(x => x.DiscordId == discordId);

                return discordUser;
            }
        }
    }
}
