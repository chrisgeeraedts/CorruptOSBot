using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class KcModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("kc")]
        [Summary("!kc {player name}(optional) - Generates KC's for the specified player or, if left empty, your own. (Only allowed in **pvm-general**)")]
        public async Task SayKcAsync([Remainder]string playerName)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "pvm-general", Context.User))
            {
                if (ToggleStateManager.GetToggleState("kc", Context.User) && RootAdminManager.HasAnyRole(Context.User))
                {
                    await ReplyAsync(embed: await CreateEmbedForMessage(playerName));
                }
            }
            else
            {
                await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!kc", "pvm-general");
            }
            
            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("kc")]
        [Summary("kc - Generates your own KC. (Only allowed in **pvm-general**)")]
        public async Task SayKcAsync()
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "pvm-general", Context.User))
            {
                if (ToggleStateManager.GetToggleState("kc", Context.User) && RootAdminManager.HasAnyRole(Context.User))
                {
                    // get current user
                    var rsn = DiscordHelper.GetAccountNameOrNickname(Context.User);
                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // reply with username
                        await ReplyAsync(embed: await CreateEmbedForMessage(rsn));
                    }
                    else
                    {
                        // reply with username
                        await ReplyAsync(embed: await CreateEmbedForMessage(Context.User.Username));
                    }
                }
            }
            else
            {
                await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!kc", "pvm-general");
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }



        private void AddLine(EmbedBuilder builder, Dictionary<string, int> bossAndKc)
        {
            var fieldBuilder = new EmbedFieldBuilder();
            var stringFoo = string.Empty;
            foreach (var item in bossAndKc)
            {
                string kcValue = item.Value < 0 ? "---" : item.Value.ToString();
                stringFoo += string.Format("{0}\u200b **{1}**{2}{3}", item.Key, kcValue, Environment.NewLine, Environment.NewLine);
            }

            fieldBuilder.Name = "\u200b";
            fieldBuilder.IsInline = true;
            fieldBuilder.Value = stringFoo;

            builder.Fields.Add(fieldBuilder);
        }

        private async Task<Embed> CreateEmbedForMessage(string rsn)
        {
            // look up in WOM
            await WOMMemoryCache.UpdateClanMember(WOMMemoryCache.OneHourMS, rsn);
            var clanMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

            if (clanMember != null)
            {
                // create embed
                EmbedBuilder builder = new EmbedBuilder();
                builder.Title = string.Format("{0} | {1}", rsn, clanMember.id);
                builder.Color = Color.DarkRed;
                builder.WithFooter(string.Format("Data from: {0}", DateTime.Now.ToString("r")));

                AddLine(builder, new Dictionary<string, int>()
                {
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.sire.ToString()), clanMember.latestSnapshot.abyssal_sire.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.bryophyta.ToString()), clanMember.latestSnapshot.bryophyta.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.chamber.ToString()), clanMember.latestSnapshot.chambers_of_xeric.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.fanatic.ToString()), clanMember.latestSnapshot.chaos_fanatic.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.prime.ToString()), clanMember.latestSnapshot.dagannoth_prime.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.crazyarc.ToString()), clanMember.latestSnapshot.crazy_archaeologist.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.mole.ToString()), clanMember.latestSnapshot.giant_mole.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kq.ToString()), clanMember.latestSnapshot.kalphite_queen.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kree.ToString()), clanMember.latestSnapshot.kreearra.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.nightmare.ToString()), clanMember.latestSnapshot.nightmare.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.scorpia.ToString()), clanMember.latestSnapshot.scorpia.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.gaunt.ToString()), clanMember.latestSnapshot.the_gauntlet.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.thermy.ToString()), clanMember.latestSnapshot.thermonuclear_smoke_devil.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.venny.ToString()), clanMember.latestSnapshot.venenatis.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.todt.ToString()), clanMember.latestSnapshot.wintertodt.kills }
                });

                AddLine(builder, new Dictionary<string, int>()
                {
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.hydra.ToString()), clanMember.latestSnapshot.alchemical_hydra.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.callisto.ToString()), clanMember.latestSnapshot.callisto.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.chambercm.ToString()), clanMember.latestSnapshot.chambers_of_xeric_challenge_mode.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.sara.ToString()), clanMember.latestSnapshot.commander_zilyana.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.rex.ToString()), clanMember.latestSnapshot.dagannoth_rex.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.derangedarc.ToString()), clanMember.latestSnapshot.deranged_archaeologist.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.gargs.ToString()), clanMember.latestSnapshot.grotesque_guardians.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kbd.ToString()), clanMember.latestSnapshot.king_black_dragon.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kril.ToString()), clanMember.latestSnapshot.kril_tsutsaroth.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.obor.ToString()), clanMember.latestSnapshot.obor.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.skotizo.ToString()), clanMember.latestSnapshot.skotizo.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.corruptgaunt.ToString()), clanMember.latestSnapshot.the_corrupted_gauntlet.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.zuk.ToString()), clanMember.latestSnapshot.tzkal_zuk.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.vetion.ToString()), clanMember.latestSnapshot.vetion.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.zalcano.ToString()), clanMember.latestSnapshot.zalcano.kills }
                });

                AddLine(builder, new Dictionary<string, int>()
                {
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.barrows.ToString()), clanMember.latestSnapshot.barrows_chests.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.cerb.ToString()), clanMember.latestSnapshot.cerberus.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.chaosele.ToString()), clanMember.latestSnapshot.chaos_elemental.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.corp.ToString()), clanMember.latestSnapshot.corporeal_beast.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.supreme.ToString()), clanMember.latestSnapshot.dagannoth_supreme.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.bandos.ToString()), clanMember.latestSnapshot.general_graardor.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.hespori.ToString()), clanMember.latestSnapshot.hespori.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kraken.ToString()), clanMember.latestSnapshot.kraken.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.mimic.ToString()), clanMember.latestSnapshot.mimic.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.sarachnis.ToString()), clanMember.latestSnapshot.sarachnis.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.tempor.ToString()), clanMember.latestSnapshot.tempoross.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.tob.ToString()), clanMember.latestSnapshot.theatre_of_blood.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.jad.ToString()), clanMember.latestSnapshot.tztok_jad.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.vorkath.ToString()), clanMember.latestSnapshot.vorkath.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.zulrah.ToString()), clanMember.latestSnapshot.zulrah.kills }
                });

                var result = builder.Build();
                return result;
            }
            return null;
        }

    }
}
