using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class KcModule : ModuleBase<SocketCommandContext>
    {
        [Command("kc")]
        [Summary("{player name} (optional) - Generates KC's for the specified player or, if left empty, your own.")]
        public async Task SayKcAsync([Remainder]string playerName)
        {
            if (RootAdminManager.GetToggleState("kc") && RootAdminManager.HasAnyRole(Context.User))
            {
                await ReplyAsync(embed: CreateEmbedForMessage(playerName));
            }
        }

        [Command("kc")]
        [Summary("Generates your own KC.")]
        public async Task SayKcAsync()
        {
            if (RootAdminManager.GetToggleState("kc") && RootAdminManager.HasAnyRole(Context.User))
            {
                // get current user
                var rsn = ((SocketGuildUser)Context.User).Nickname;
                if (!string.IsNullOrEmpty(rsn))
                {
                    // reply with username
                    await ReplyAsync(embed: CreateEmbedForMessage(rsn));
                }
                else
                {
                    // reply with username
                    await ReplyAsync(embed: CreateEmbedForMessage(Context.User.Username));
                }
            }
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

        private Embed CreateEmbedForMessage(string rsn)
        {
            // look up in WOM
            var client = new WiseOldManClient();
            var clanMember = client.GetClanMembers(128).FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

            if (clanMember != null)
            {
                var user = client.GetPlayerDetails(clanMember.id);

                // create embed
                EmbedBuilder builder = new EmbedBuilder();
                builder.Title = string.Format("{0} | {1}", rsn, user.id);
                builder.Color = Color.DarkRed;
                builder.WithFooter(string.Format("Powered by WiseOldMan API • {0}", DateTime.Now.ToString("dd/MM/yyyy")));

                AddLine(builder, new Dictionary<string, int>()
                {
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.sire), user.latestSnapshot.abyssal_sire.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.bryophyta), user.latestSnapshot.bryophyta.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.chamber), user.latestSnapshot.chambers_of_xeric.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.fanatic), user.latestSnapshot.chaos_fanatic.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.prime), user.latestSnapshot.dagannoth_prime.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.crazyarc), user.latestSnapshot.crazy_archaeologist.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.mole), user.latestSnapshot.giant_mole.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kq), user.latestSnapshot.kalphite_queen.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kree), user.latestSnapshot.kreearra.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.nightmare), user.latestSnapshot.nightmare.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.scorpia), user.latestSnapshot.scorpia.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.gaunt), user.latestSnapshot.the_gauntlet.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.thermy), user.latestSnapshot.thermonuclear_smoke_devil.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.venny), user.latestSnapshot.venenatis.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.todt), user.latestSnapshot.wintertodt.kills }
                });

                AddLine(builder, new Dictionary<string, int>()
                {
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.hydra), user.latestSnapshot.alchemical_hydra.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.callisto), user.latestSnapshot.callisto.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.chambercm), user.latestSnapshot.chambers_of_xeric_challenge_mode.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.sara), user.latestSnapshot.commander_zilyana.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.rex), user.latestSnapshot.dagannoth_rex.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.derangedarc), user.latestSnapshot.deranged_archaeologist.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.gargs), user.latestSnapshot.grotesque_guardians.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kbd), user.latestSnapshot.king_black_dragon.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kril), user.latestSnapshot.kril_tsutsaroth.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.obor), user.latestSnapshot.obor.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.skotizo), user.latestSnapshot.skotizo.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.corruptgaunt), user.latestSnapshot.the_corrupted_gauntlet.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.zuk), user.latestSnapshot.tzkal_zuk.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.vetion), user.latestSnapshot.vetion.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.zalcano), user.latestSnapshot.zalcano.kills }
                });

                AddLine(builder, new Dictionary<string, int>()
                {
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.barrows), user.latestSnapshot.barrows_chests.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.cerb), user.latestSnapshot.cerberus.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.chaosele), user.latestSnapshot.chaos_elemental.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.corp), user.latestSnapshot.corporeal_beast.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.supreme), user.latestSnapshot.dagannoth_supreme.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.bandos), user.latestSnapshot.general_graardor.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.hespori), user.latestSnapshot.hespori.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kraken), user.latestSnapshot.kraken.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.mimic), user.latestSnapshot.mimic.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.sarachnis), user.latestSnapshot.sarachnis.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.tempor), user.latestSnapshot.tempoross.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.tob), user.latestSnapshot.theatre_of_blood.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.jad), user.latestSnapshot.tztok_jad.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.vorkath), user.latestSnapshot.vorkath.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.zulrah), user.latestSnapshot.zulrah.kills }
                });

                var result = builder.Build();
                return result;
            }
            return null;
        }

    }
}
