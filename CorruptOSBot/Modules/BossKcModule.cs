using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Helpers.PVM;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class BossKcModule : ModuleBase<SocketCommandContext>
    {
        [Command("bosskc")]
        [Summary("{boss name} - Generates boss KC's for the specified player or, if left empty, your own. (Only allowed in **pvm-general**)")]
        public async Task SayBossKcAsync([Remainder]string bossname)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "pvm-general", Context.User))
            {
                if (RootAdminManager.GetToggleState("bosskc", Context.User) && RootAdminManager.HasAnyRole(Context.User))
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


        private async Task<Embed> CreateEmbedForMessage(string bossname)
        {
            var result = await BossKCHelper.GetTopBossKC(WOMMemoryCache.OneDayMS);

            try
            {
                EmojiEnum bossEnum = (EmojiEnum)Enum.Parse(typeof(EmojiEnum), bossname);
                var bossResult = result.FirstOrDefault(x => x.Boss == bossEnum);
                var emb = new EmbedBuilder();
                var sb = new StringBuilder();
                for (int i = 0; i < 5; i++)
                {
                    emb.AddField("\u200b", GetKCLine(i, i == 0, bossResult), true);
                }
                
                return emb
                    .WithTitle(string.Format("{0} {1}",
                        EmojiHelper.GetFullEmojiString(bossResult.Boss),
                        bossResult.Boss))
                    .Build();
            }
            catch (Exception e)
            {
                return null;
            }
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
