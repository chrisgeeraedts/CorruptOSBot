using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Extensions.WOM.ClanMemberDetails;
using CorruptOSBot.Helpers;
using CorruptOSBot.Services;
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
        [Command("test2")]
        [Summary("(admin) A test command hosting different functionality - only used during development")]
        public async Task SayTest2Async()
        {
            if (RootAdminManager.GetToggleState("test2", Context.User) && RootAdminManager.HasSpecificRole(Context.User, "Staff"))
            {
                await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneDayMS);
                var clanMembers = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails;

                // connect the kcs per boss
                var result = new List<KcTopList>();
                FillListWithBosses(result);
                UpdateListWithKc(result, clanMembers);

                int charPerRow = 30;

                foreach (var item in result)
                {
                    var player1String = string.Format("{0}{1} **({2})**", "\U0001f947", item.KcPlayers.Skip(0).First().Player, item.KcPlayers.Skip(0).First().Kc);
                    var player1Append = GetRemainingSpaces(player1String, charPerRow);

                    var player2String = string.Format("{0}{1} ({2})", "\U0001f948", item.KcPlayers.Skip(1).First().Player, item.KcPlayers.Skip(1).First().Kc);
                    var player2Append = GetRemainingSpaces(player2String, charPerRow);

                    var player3String = string.Format("{0}{1} ({2})", "\U0001f949", item.KcPlayers.Skip(2).First().Player, item.KcPlayers.Skip(2).First().Kc);
                    var player3Append = GetRemainingSpaces(player3String, charPerRow);

                    await ReplyAsync(string.Format("{0} {1}{2}{3}{4}{5}{6}", 
                        EmojiHelper.GetFullEmojiString(item.Boss),
                        player1String,
                        player1Append,
                        player2String,
                        player2Append,
                        player3String,
                        player3Append));
                }
            }
        }

        private string GetRemainingSpaces(string name, int totalSpaces)
        {
            string result = string.Empty;
            var spacesRemaining = totalSpaces - name.Length;
            for (int i = 0; i < spacesRemaining; i++)
            {
                result += " ";
            }
            return result;
        }

        private void BuildSet(IEnumerable<KcTopList> result, EmbedBuilder builder)
        {
            // First column
            var sb = new StringBuilder();
            foreach (var item in result)
            {
                if (item.KcPlayers.Count() > 0)
                {
                    sb.AppendLine(string.Format("{0} {1} {2} **({3})**", EmojiHelper.GetFullEmojiString(item.Boss), "\U0001f947", item.KcPlayers.Skip(0).First().Player, item.KcPlayers.Skip(0).First().Kc));
                }
                else
                {
                    sb.AppendLine(string.Format("<:{0}:> {1} {2}", EmojiHelper.GetFullEmojiString(item.Boss), "\U0001f947", "---"));
                }
            }
            builder.AddField("\u200b", sb.ToString(), true);

            // Second column
            var sb2 = new StringBuilder();
            foreach (var item in result)
            {
                if (item.KcPlayers.Count() > 1)
                {
                    sb2.AppendLine(string.Format("{0} {1} ({2})", "\U0001f948", item.KcPlayers.Skip(1).First().Player, item.KcPlayers.Skip(1).First().Kc));
                }
                else
                {
                    sb2.AppendLine(string.Format("{0} {1}", "\U0001f948", "---"));
                }
            }
            builder.AddField("\u200b", sb2.ToString(), true);

            // Second column
            var sb3 = new StringBuilder();
            foreach (var item in result)
            {
                if (item.KcPlayers.Count() > 1)
                {
                    sb3.AppendLine(string.Format("{0} {1} ({2})", "\U0001f949", item.KcPlayers.Skip(2).First().Player, item.KcPlayers.Skip(2).First().Kc));
                }
                else
                {
                    sb3.AppendLine(string.Format("{0} {1}", "\U0001f949", "---"));
                }

            }
            builder.AddField("\u200b", sb3.ToString(), true);
        }

        private void FillListWithBosses(List<KcTopList> result)
        {
            result.Add(new KcTopList() { Boss = EmojiEnum.sire, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.hydra, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.barrows, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.bryophyta, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.callisto, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.cerb, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.chamber, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.chambercm, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.chaosele, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.fanatic, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.sara, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.corp, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.prime, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.rex, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.supreme, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.crazyarc, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.derangedarc, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.bandos, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.mole, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.gargs, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.hespori, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.kq, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.kbd, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.kraken, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.kree, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.kril, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.mimic, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.nightmare, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.obor, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.sarachnis, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.scorpia, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.skotizo, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.tempor, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.gaunt, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.corruptgaunt, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.tob, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.thermy, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.zuk, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.jad, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.venny, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.vetion, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.vorkath, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.todt, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.zalcano, KcPlayers = new List<KcPlayer>() });
            result.Add(new KcTopList() { Boss = EmojiEnum.zulrah, KcPlayers = new List<KcPlayer>() });
        }

        private void UpdateListWithKc(List<KcTopList> result, List<ClanMemberDetail> clanMembers)
        {
            result.FirstOrDefault(x => x.Boss == EmojiEnum.sire).KcPlayers.AddRange(GetTopKc(typeof(AbyssalSire), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.hydra).KcPlayers.AddRange(GetTopKc(typeof(AlchemicalHydra), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.barrows).KcPlayers.AddRange(GetTopKc(typeof(BarrowsChests), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.bryophyta).KcPlayers.AddRange(GetTopKc(typeof(Bryophyta), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.callisto).KcPlayers.AddRange(GetTopKc(typeof(Callisto), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.cerb).KcPlayers.AddRange(GetTopKc(typeof(Cerberus), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.chamber).KcPlayers.AddRange(GetTopKc(typeof(ChambersOfXeric), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.chambercm).KcPlayers.AddRange(GetTopKc(typeof(ChambersOfXericChallengeMode), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.chaosele).KcPlayers.AddRange(GetTopKc(typeof(ChaosElemental), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.fanatic).KcPlayers.AddRange(GetTopKc(typeof(ChaosFanatic), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.sara).KcPlayers.AddRange(GetTopKc(typeof(CommanderZilyana), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.corp).KcPlayers.AddRange(GetTopKc(typeof(CorporealBeast), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.prime).KcPlayers.AddRange(GetTopKc(typeof(DagannothPrime), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.rex).KcPlayers.AddRange(GetTopKc(typeof(DagannothRex), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.supreme).KcPlayers.AddRange(GetTopKc(typeof(DagannothSupreme), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.crazyarc).KcPlayers.AddRange(GetTopKc(typeof(CrazyArchaeologist), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.derangedarc).KcPlayers.AddRange(GetTopKc(typeof(DerangedArchaeologist), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.bandos).KcPlayers.AddRange(GetTopKc(typeof(GeneralGraardor), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.mole).KcPlayers.AddRange(GetTopKc(typeof(GiantMole), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.gargs).KcPlayers.AddRange(GetTopKc(typeof(GrotesqueGuardians), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.hespori).KcPlayers.AddRange(GetTopKc(typeof(Hespori), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.kq).KcPlayers.AddRange(GetTopKc(typeof(KalphiteQueen), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.kbd).KcPlayers.AddRange(GetTopKc(typeof(KingBlackDragon), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.kraken).KcPlayers.AddRange(GetTopKc(typeof(Kraken), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.kree).KcPlayers.AddRange(GetTopKc(typeof(Kreearra), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.kril).KcPlayers.AddRange(GetTopKc(typeof(KrilTsutsaroth), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.mimic).KcPlayers.AddRange(GetTopKc(typeof(Mimic), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.nightmare).KcPlayers.AddRange(GetTopKc(typeof(Nightmare), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.obor).KcPlayers.AddRange(GetTopKc(typeof(Obor), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.sarachnis).KcPlayers.AddRange(GetTopKc(typeof(Sarachnis), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.scorpia).KcPlayers.AddRange(GetTopKc(typeof(Scorpia), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.skotizo).KcPlayers.AddRange(GetTopKc(typeof(Skotizo), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.tempor).KcPlayers.AddRange(GetTopKc(typeof(Tempoross), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.gaunt).KcPlayers.AddRange(GetTopKc(typeof(TheGauntlet), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.corruptgaunt).KcPlayers.AddRange(GetTopKc(typeof(TheCorruptedGauntlet), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.tob).KcPlayers.AddRange(GetTopKc(typeof(TheatreOfBlood), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.thermy).KcPlayers.AddRange(GetTopKc(typeof(ThermonuclearSmokeDevil), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.zuk).KcPlayers.AddRange(GetTopKc(typeof(TzkalZuk), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.jad).KcPlayers.AddRange(GetTopKc(typeof(TztokJad), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.venny).KcPlayers.AddRange(GetTopKc(typeof(Venenatis), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.vetion).KcPlayers.AddRange(GetTopKc(typeof(Vetion), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.vorkath).KcPlayers.AddRange(GetTopKc(typeof(Vorkath), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.todt).KcPlayers.AddRange(GetTopKc(typeof(Wintertodt), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.zalcano).KcPlayers.AddRange(GetTopKc(typeof(Zalcano), clanMembers));
            result.FirstOrDefault(x => x.Boss == EmojiEnum.zulrah).KcPlayers.AddRange(GetTopKc(typeof(Zulrah), clanMembers));
        }

        private List<KcPlayer> GetTopKc(Type type, List<ClanMemberDetail> clanMemberDetails)
        {
            var bossKills = new List<KcPlayer>();
            foreach (var clanMemberDetail in clanMemberDetails.Where(x => x.latestSnapshot != null))
            {
                var props = clanMemberDetail.latestSnapshot.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var typeOfProp = prop.PropertyType;
                    if (typeOfProp == type)
                    {
                        var kills = ((IBossKc)prop.GetValue(clanMemberDetail.latestSnapshot)).kills;
                        if (kills > 0)
                        {
                            bossKills.Add(new KcPlayer() { Kc = kills, Player = clanMemberDetail.displayName });
                        }
                    }
                }
            }
            return bossKills.OrderByDescending(x => x.Kc).ToList();
        }


        [Command("postid")]
        [Summary("(admin) Gets the current post's Id")]
        public async Task SaypostidAsync()
        {
            if (RootAdminManager.GetToggleState("postid", Context.User))
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
            if (RootAdminManager.GetToggleState("channelid", Context.User))
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
            if (RootAdminManager.GetToggleState("guildid", Context.User))
            {
                var channel = Context.Guild.Id.ToString();

                await ReplyAsync(channel);

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }
    }
}
