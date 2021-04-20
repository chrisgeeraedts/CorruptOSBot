using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
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
        [Command("kc")]
        [Summary("!kc {player name}(optional) - Generates KC's for the specified player or, if left empty, your own. (Only allowed in **pvm-general**)")]
        public async Task SayKcAsync([Remainder]string playerName)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "pvm-general", Context.User))
            {
                if (RootAdminManager.GetToggleState("kc", Context.User) && RootAdminManager.HasAnyRole(Context.User))
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

        [Command("kc")]
        [Summary("kc - Generates your own KC. (Only allowed in **pvm-general**)")]
        public async Task SayKcAsync()
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "pvm-general", Context.User))
            {
                if (RootAdminManager.GetToggleState("kc", Context.User) && RootAdminManager.HasAnyRole(Context.User))
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
                builder.WithFooter(string.Format("Powered by WiseOldMan API • {0}", DateTime.Now.ToString("dd/MM/yyyy")));

                AddLine(builder, new Dictionary<string, int>()
                {
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.sire), clanMember.latestSnapshot.abyssal_sire.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.bryophyta), clanMember.latestSnapshot.bryophyta.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.chamber), clanMember.latestSnapshot.chambers_of_xeric.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.fanatic), clanMember.latestSnapshot.chaos_fanatic.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.prime), clanMember.latestSnapshot.dagannoth_prime.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.crazyarc), clanMember.latestSnapshot.crazy_archaeologist.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.mole), clanMember.latestSnapshot.giant_mole.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kq), clanMember.latestSnapshot.kalphite_queen.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kree), clanMember.latestSnapshot.kreearra.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.nightmare), clanMember.latestSnapshot.nightmare.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.scorpia), clanMember.latestSnapshot.scorpia.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.gaunt), clanMember.latestSnapshot.the_gauntlet.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.thermy), clanMember.latestSnapshot.thermonuclear_smoke_devil.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.venny), clanMember.latestSnapshot.venenatis.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.todt), clanMember.latestSnapshot.wintertodt.kills }
                });

                AddLine(builder, new Dictionary<string, int>()
                {
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.hydra), clanMember.latestSnapshot.alchemical_hydra.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.callisto), clanMember.latestSnapshot.callisto.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.chambercm), clanMember.latestSnapshot.chambers_of_xeric_challenge_mode.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.sara), clanMember.latestSnapshot.commander_zilyana.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.rex), clanMember.latestSnapshot.dagannoth_rex.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.derangedarc), clanMember.latestSnapshot.deranged_archaeologist.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.gargs), clanMember.latestSnapshot.grotesque_guardians.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kbd), clanMember.latestSnapshot.king_black_dragon.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kril), clanMember.latestSnapshot.kril_tsutsaroth.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.obor), clanMember.latestSnapshot.obor.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.skotizo), clanMember.latestSnapshot.skotizo.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.corruptgaunt), clanMember.latestSnapshot.the_corrupted_gauntlet.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.zuk), clanMember.latestSnapshot.tzkal_zuk.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.vetion), clanMember.latestSnapshot.vetion.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.zalcano), clanMember.latestSnapshot.zalcano.kills }
                });

                AddLine(builder, new Dictionary<string, int>()
                {
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.barrows), clanMember.latestSnapshot.barrows_chests.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.cerb), clanMember.latestSnapshot.cerberus.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.chaosele), clanMember.latestSnapshot.chaos_elemental.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.corp), clanMember.latestSnapshot.corporeal_beast.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.supreme), clanMember.latestSnapshot.dagannoth_supreme.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.bandos), clanMember.latestSnapshot.general_graardor.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.hespori), clanMember.latestSnapshot.hespori.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.kraken), clanMember.latestSnapshot.kraken.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.mimic), clanMember.latestSnapshot.mimic.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.sarachnis), clanMember.latestSnapshot.sarachnis.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.tempor), clanMember.latestSnapshot.tempoross.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.tob), clanMember.latestSnapshot.theatre_of_blood.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.jad), clanMember.latestSnapshot.tztok_jad.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.vorkath), clanMember.latestSnapshot.vorkath.kills },
                    { EmojiHelper.GetFullEmojiString(EmojiEnum.zulrah), clanMember.latestSnapshot.zulrah.kills }
                });

                var result = builder.Build();
                return result;
            }
            return null;
        }

    }
}
