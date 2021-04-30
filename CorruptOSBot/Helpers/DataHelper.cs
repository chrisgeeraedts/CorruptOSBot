﻿using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers.Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Helpers
{
    public static class DataHelper
    {
        public static async Task AddNewDiscordUserAndRSN(SocketUser currentUser, string preferedNickname, ClanMember groupMember)
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
                    wom_id = groupMember?.id
                });

                await corruptosEntities.SaveChangesAsync();
            }
        }

    }
}
