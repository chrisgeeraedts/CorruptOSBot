using CorruptOSBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Helpers.PVM
{
    public static class BossKCHelper
    {
        public static List<KcTopList> cachedBosses = new List<KcTopList>();

        public static async Task<List<KcTopList>> GetTopBossKC()
        {
            var result = new List<KcTopList>();

            var womClient = new WiseOldManClient();

            if (!cachedBosses.Any())
            {
                await Task.Run(() =>
                {
                    var bosses = ((BossEnum[])Enum.GetValues(typeof(BossEnum))).ToList();

                    foreach (var boss in bosses)
                    {
                        var hiscores = womClient.GetClanHiscores(boss.ToString(), 3);
                        var kcPlayers = new List<KcPlayer>();

                        foreach (var hiscore in hiscores)
                        {
                            kcPlayers.Add(new KcPlayer()
                            {
                                Player = hiscore.player.displayName,
                                Kc = hiscore.data.kills
                            });
                        }

                        result.Add(new KcTopList()
                        {
                            Boss = boss,
                            KcPlayers = kcPlayers
                        });
                    }
                });

                cachedBosses = result;
            }

            return cachedBosses;
        }
    }
}
