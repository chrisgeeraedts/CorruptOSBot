using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Events.ReactionHandlers
{
    public class BingoReactionHandler
    { 
        public static async Task Execute(DiscordSocketClient client, ulong guildId, Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            // do something with this specific emoji. For example, if the post is the PVM thing, execute effectively the !tob command
            var activatedEmojiId = ((Emote)arg3.Emote).Id;
            var yesEmojiId = 122131;
            var noEmojiId = 122131;

            Dictionary<Data.BingoCardSlot, long> SlotEmojiIds = new Dictionary<Data.BingoCardSlot, long>();




            if (emojiId == CorruptOSBot.Helpers.EmojiHelper.GetEmojiId(EmojiEnum.tob.ToString()))
            {
                // DO STUFF
            }

        }
    }
}
