using CorruptOSBot.Data;
using CorruptOSBot.Shared;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.Entity;

namespace CorruptOSBot.Bingo
{
    public static class BingoManager
    {
        public static BingoEvent ActiveBingoEvent { get; set; }
        public static List<BingoTeam> ActiveBingoTeams { get; set; }
        public static List<BingoCardSlot> ActiveBingoCardSlots { get; set; }
        public static BingoCard ActiveBingoCard { get; set; }
        public static void Init()
        {
            using (var database = new Data.CorruptModel())
            {
                ActiveBingoEvent = database.BingoEvents.FirstOrDefault(x => x.Active);
                if (ActiveBingoEvent != null)
                {
                    ActiveBingoTeams = database.BingoTeams.Include(customer => customer.BingoTeamMembers).Where(x =>
                        x.BingoTeamCards.FirstOrDefault() != null &&
                        x.BingoTeamCards.FirstOrDefault().BingoCard != null &&
                        x.BingoTeamCards.FirstOrDefault().BingoCard.BingoEvent.Id == ActiveBingoEvent.Id).ToList();
                    ActiveBingoCard = ActiveBingoEvent.BingoCards.FirstOrDefault();
                    ActiveBingoCardSlots = ActiveBingoCard.BingoCardSlots.ToList();
                }
            }
        }
    }
}
