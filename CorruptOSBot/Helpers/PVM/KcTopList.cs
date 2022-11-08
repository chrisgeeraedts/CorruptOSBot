using System.Collections.Generic;

namespace CorruptOSBot.Helpers.PVM
{
    public class KcTopList
    {
        public BossEnum Boss { get; set; }
        public string postId { get; set; }
        public List<KcPlayer> KcPlayers { get; set; }
    }
}
