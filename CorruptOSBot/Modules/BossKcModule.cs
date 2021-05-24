using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Helpers.PVM;
using CorruptOSBot.Services;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class BossKcModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("bosskc")]
        [Summary("!bosskc {boss name} - Generates Top Clan KC for a specific boss (Only allowed in **pvm-general**)")]
        public async Task SayBossKcAsync()
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "pvm-general", Context.User))
            {
                if (ToggleStateManager.GetToggleState("bosskc", Context.User) && 
                    RoleHelper.HasAnyRole(Context.User))
                {
                    var bosses = new Shared.DataHelper().GetBosses();
                    StringBuilder sb = new StringBuilder();
                    foreach (var boss in bosses)
                    {
                        sb.AppendLine(string.Format("- {0}", boss.BossCommand));
                    }


                    await ReplyAsync(embed:
                    new EmbedBuilder()
                        .WithTitle("Available bosses - use !bosskc {bossname}:")
                        .WithDescription(sb.ToString())
                        .Build());
                }
            }
            else
            {
                await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!bosskc", "pvm-general");
            }

            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("bosskc")]
        [Summary("!bosskc {boss name} - Generates Top Clan KC for a specific boss (Only allowed in **pvm-general**)")]
        public async Task SayBossKcAsync([Remainder]string bossname)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "pvm-general", Context.User))
            {
                if (ToggleStateManager.GetToggleState("bosskc", Context.User) && 
                    RoleHelper.HasAnyRole(Context.User))
                {
                    var embed = await CreateEmbedForMessage(bossname);
                    if (embed != null)
                    {
                        await ReplyAsync(embed: embed);
                    }
                }
            }
            else
            {
                await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!bosskc", "pvm-general");
            }
            
            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Admin)]
        [Command("refreshtopbosskc")]
        [Summary("!refreshtopbosskc - Regenerates the Top Clan KC list")]
        public async Task SayRefreshTopKcAsync()
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "top-boss-kc", Context.User) &&
                RoleHelper.HasRole(Context.User, Context.Guild, 3) && //bot staff
                ToggleStateManager.GetToggleState(nameof(TopKCService)))
            {
                // find current channel
                var channel = await ((IGuild)Context.Guild).GetChannelAsync(ChannelHelper.GetChannelId("top-boss-kc")) as SocketTextChannel;

                if (channel != null)
                {
                    // first update base data
                    await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneDayMS);

                    // clear messages in channel
                    var messages = await channel.GetMessagesAsync(3).FlattenAsync();
                    await channel.DeleteMessagesAsync(messages);

                    // Add new message
                    await (channel as SocketTextChannel).SendMessageAsync(embed: await EmbedHelper.CreateFullLeaderboardEmbed(1000 * 60 * 60 * 24 * 3));
                }
            }
        }

        private async Task<Embed> CreateEmbedForMessage(string bossname)
        {
            var result = await BossKCHelper.GetTopBossKC(WOMMemoryCache.OneDayMS);

            var bosses = new Shared.DataHelper().GetBosses();
            var selectedBoss = bosses.FirstOrDefault(x => x.BossCommand == bossname.ToLower());
            string bossEmoji = "";
            string bossUri = "";

            if (selectedBoss != null)
            {
                bossEmoji = selectedBoss.EmojiName;
                bossUri = selectedBoss.BossImage;

                try
                {
                    EmojiEnum bossEnum = (EmojiEnum)Enum.Parse(typeof(EmojiEnum), selectedBoss.Bossname.ToLower());
                    var bossResult = result.FirstOrDefault(x => x.Boss == bossEnum);
                    var emb = new EmbedBuilder();
                    var sb = new StringBuilder();
                    for (int i = 0; i < 3; i++)
                    {
                        emb.AddField("\u200b", GetKCLine(i, i == 0, bossResult), true);
                    }

                    return emb
                        .WithTitle(string.Format("{0} Corrupt OS Top 3 for: {1}",
                            bossEmoji,
                            bossResult.Boss.ToString().FirstCharToUpper()))
                        .WithImageUrl(bossUri)
                        .WithFooter(string.Format("Corrupt OS - type '!bosskc {1}' - Data from: {0}", DateTime.Now.ToString("r"), bossResult.Boss.ToString()))
                        .Build();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return null;
        }

        private string GetKCLine(int skip, bool bold, KcTopList bossResult)
        {
            string emoji = GetEmoji(skip);
            var player = bossResult.KcPlayers.Skip(skip).First();
            if (bossResult.KcPlayers.Count > skip)
            {
                if (bold)
                {
                    return string.Format("{0}  **{1} ({2})**", emoji, player.Player, player.Kc);
                }
                else
                {
                    return string.Format("{0}  {1} ({2})", emoji, player.Player, player.Kc);
                }
            }
            return string.Empty;
        }

        private string GetEmoji(int skip)
        {
            if (skip == 0)
            {
                return "\U0001f947";
            }
            else if (skip == 1)
            {
                return "\U0001f948";
            }
            else if (skip == 2)
            {
                return "\U0001f949";
            }
            else if (skip == 3)
            {
                return ":four:";
            }
            else if (skip == 4)
            {
                return ":five:";
            }
            return string.Empty;
        }
    }
}
