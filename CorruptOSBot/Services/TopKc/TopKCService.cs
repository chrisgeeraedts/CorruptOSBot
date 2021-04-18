//using CorruptOSBot.Extensions.WOM;
//using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
//using CorruptOSBot.Helpers;
//using Discord;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CorruptOSBot.Services
//{

//    public class TopKCService : IService
//    {
//        public int TriggerTimeInMS { get => 1000 * 60 * 60 * 24; } // every 24 hours
//        //private ulong GuildId;


//        public TopKCService(Discord.IDiscordClient client)
//        {
//            //Program.Log(new LogMessage(LogSeverity.Info, "TopKCService", "Created, triggering every " + TriggerTimeInMS + "MS"));
//            //GuildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));
//        }

//        public async Task Trigger(Discord.IDiscordClient client)
//        {
//        //    if (RootAdminManager.GetToggleState(nameof(TopKCService)))
//        //    {
//        //        await Program.Log(new LogMessage(LogSeverity.Info, "TopKCService", "Triggered"));

//        //        try
//        //        {
//        //            // Refresh data if needed
//        //            await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneDayMS);
//        //            var clanMembers = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails;
//        //            // connect the kcs per boss
//        //            var result = new List<KcTopList>();
//        //            FillListWithBosses(result);
//        //            UpdateListWithKc(result, clanMembers);
//        //            // find the message or create a new one
//        //            var channel = await client.GetChannelAsync(ChannelHelper.GetChannelId("top-boss-kc"));
//        //            foreach (var item in result)
//        //            {
//        //                var embed = new EmbedBuilder();
//        //                int rank = 1;
//        //                var sb = new StringBuilder();
//        //                foreach (var itemPlayer in item.KcPlayers.Take(3))
//        //                {
//        //                    embed.AddField("\u200b" , string.Format("{2} **{0}: {1}**", itemPlayer.Player, itemPlayer.Kc, GetRankString(rank)), false);
//        //                    rank++;
//        //                }
//        //                embed.Title = item.Boss;
//        //                await ((IMessageChannel)channel).SendMessageAsync(embed: embed.Build());
//        //            }
//        //            // update the message
//        //            await Program.Log(new LogMessage(LogSeverity.Info, "TopKCService", "Completed"));
//        //        }
//        //        catch (Exception e)
//        //        {
//        //            await Program.Log(new LogMessage(LogSeverity.Error, "TopKCService", "Failed: " + e.Message));
//        //        }
//        //    }
//        }

//        //private string GetRankString(int rank)
//        //{
//        //    if (rank == 1)
//        //    {
//        //        return "\U0001f947";
//        //    }
//        //    if (rank == 2)
//        //    {
//        //        return "\U0001f948";
//        //    }
//        //    if (rank == 3)
//        //    {
//        //        return "\U0001f949";
//        //    }
//        //    return string.Empty;
//        //}

//        //private void UpdateListWithKc(List<KcTopList> result, List<ClanMemberDetail> clanMembers)
//        //{
//        //    result.FirstOrDefault(x => x.Boss == "sire").KcPlayers.AddRange(GetTopKc(typeof(AbyssalSire), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "hydra").KcPlayers.AddRange(GetTopKc(typeof(AlchemicalHydra), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "barrows").KcPlayers.AddRange(GetTopKc(typeof(BarrowsChests), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "bryophyta").KcPlayers.AddRange(GetTopKc(typeof(Bryophyta), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "callisto").KcPlayers.AddRange(GetTopKc(typeof(Callisto), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "cerb").KcPlayers.AddRange(GetTopKc(typeof(Cerberus), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "chamber").KcPlayers.AddRange(GetTopKc(typeof(ChambersOfXeric), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "chambercm").KcPlayers.AddRange(GetTopKc(typeof(ChambersOfXericChallengeMode), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "chaosele").KcPlayers.AddRange(GetTopKc(typeof(ChaosElemental), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "fanatic").KcPlayers.AddRange(GetTopKc(typeof(ChaosFanatic), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "sara").KcPlayers.AddRange(GetTopKc(typeof(CommanderZilyana), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "corp").KcPlayers.AddRange(GetTopKc(typeof(CorporealBeast), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "prime").KcPlayers.AddRange(GetTopKc(typeof(DagannothPrime), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "rex").KcPlayers.AddRange(GetTopKc(typeof(DagannothRex), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "supreme").KcPlayers.AddRange(GetTopKc(typeof(DagannothSupreme), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "crazyarc").KcPlayers.AddRange(GetTopKc(typeof(CrazyArchaeologist), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "derangedarc").KcPlayers.AddRange(GetTopKc(typeof(DerangedArchaeologist), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "bandos").KcPlayers.AddRange(GetTopKc(typeof(GeneralGraardor), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "mole").KcPlayers.AddRange(GetTopKc(typeof(GiantMole), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "gargs").KcPlayers.AddRange(GetTopKc(typeof(GrotesqueGuardians), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "hespori").KcPlayers.AddRange(GetTopKc(typeof(Hespori), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "kq").KcPlayers.AddRange(GetTopKc(typeof(KalphiteQueen), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "kbd").KcPlayers.AddRange(GetTopKc(typeof(KingBlackDragon), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "kraken").KcPlayers.AddRange(GetTopKc(typeof(Kraken), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "kree").KcPlayers.AddRange(GetTopKc(typeof(Kreearra), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "kril").KcPlayers.AddRange(GetTopKc(typeof(KrilTsutsaroth), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "mimic").KcPlayers.AddRange(GetTopKc(typeof(Mimic), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "nightmare").KcPlayers.AddRange(GetTopKc(typeof(Nightmare), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "obor").KcPlayers.AddRange(GetTopKc(typeof(Obor), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "sarachnis").KcPlayers.AddRange(GetTopKc(typeof(Sarachnis), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "scorpia").KcPlayers.AddRange(GetTopKc(typeof(Scorpia), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "skotizo").KcPlayers.AddRange(GetTopKc(typeof(Skotizo), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "tempor").KcPlayers.AddRange(GetTopKc(typeof(Tempoross), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "gaunt").KcPlayers.AddRange(GetTopKc(typeof(TheGauntlet), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "corruptgaunt").KcPlayers.AddRange(GetTopKc(typeof(TheCorruptedGauntlet), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "tob").KcPlayers.AddRange(GetTopKc(typeof(TheatreOfBlood), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "thermy").KcPlayers.AddRange(GetTopKc(typeof(ThermonuclearSmokeDevil), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "zuk").KcPlayers.AddRange(GetTopKc(typeof(TzkalZuk), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "jad").KcPlayers.AddRange(GetTopKc(typeof(TztokJad), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "venny").KcPlayers.AddRange(GetTopKc(typeof(Venenatis), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "vetion").KcPlayers.AddRange(GetTopKc(typeof(Vetion), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "vorkath").KcPlayers.AddRange(GetTopKc(typeof(Vorkath), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "todt").KcPlayers.AddRange(GetTopKc(typeof(Wintertodt), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "zalcano").KcPlayers.AddRange(GetTopKc(typeof(Zalcano), clanMembers));
//        //    result.FirstOrDefault(x => x.Boss == "zulrah").KcPlayers.AddRange(GetTopKc(typeof(Zulrah), clanMembers));
//        //}

//        //private List<KcPlayer> GetTopKc(Type type, List<ClanMemberDetail> clanMemberDetails)
//        //{
//        //    var bossKills = new List<KcPlayer>();
//        //    foreach (var clanMemberDetail in clanMemberDetails)
//        //    {
//        //        var props = clanMemberDetail.latestSnapshot.GetType().GetProperties();
//        //        foreach (var prop in props)
//        //        {
//        //            var typeOfProp = prop.PropertyType;
//        //            if (typeOfProp == type)
//        //            {
//        //                var kills = ((IBossKc)prop.GetValue(clanMemberDetail.latestSnapshot)).kills;
//        //                if (kills > 0)
//        //                {
//        //                    bossKills.Add(new KcPlayer() { Kc = kills, Player = clanMemberDetail.displayName });
//        //                }
//        //            }
//        //        }
//        //    }
//        //    return bossKills.OrderByDescending(x => x.Kc).ToList();
//        //}


//        //private void FillListWithBosses(List<KcTopList> result)
//        //{
//        //    result.Add(new KcTopList() { Boss = "sire", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "hydra", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "barrows", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "bryophyta", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "callisto", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "cerb", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "chamber", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "chambercm", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "chaosele", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "fanatic", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "sara", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "corp", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "prime", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "rex", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "supreme", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "crazyarc", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "derangedarc", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "bandos", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "mole", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "gargs", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "hespori", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "kq", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "kbd", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "kraken", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "kree", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "kril", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "mimic", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "nightmare", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "obor", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "sarachnis", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "scorpia", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "skotizo", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "tempor", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "gaunt", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "corruptgaunt", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "tob", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "thermy", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "zuk", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "jad", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "venny", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "vetion", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "vorkath", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "todt", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "zalcano", KcPlayers = new List<KcPlayer>() });
//        //    result.Add(new KcTopList() { Boss = "zulrah", KcPlayers = new List<KcPlayer>() });
//        //}
//    }
//}
