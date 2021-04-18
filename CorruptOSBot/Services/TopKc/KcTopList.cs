using CorruptOSBot.Helpers;
using System.Collections.Generic;

namespace CorruptOSBot.Services
{
    public class KcTopList
    {
        public EmojiEnum Boss { get; set; }
        public string postId { get; set; }
        public List<KcPlayer> KcPlayers { get; set; }
    }
}
