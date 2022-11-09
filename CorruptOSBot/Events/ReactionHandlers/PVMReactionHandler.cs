using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Helpers.PVM;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Events.ReactionHandlers
{
    public class PVMReactionHandler
    { 

        public static async Task Execute(DiscordSocketClient client, ulong guildId, Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            // do something with this specific emoji. For example, if the post is the PVM thing, execute effectively the !tob command
            var emojiId = ((Emote)arg3.Emote).Id;
            if (emojiId == EmojiHelper.GetEmojiId(BossEnum.theatre_of_blood.ToString()))
            {
                await HandleToB(client, guildId, arg3);
            }
            if (emojiId == EmojiHelper.GetEmojiId(BossEnum.chambers_of_xeric.ToString()))
            {
                await HandleCoX(client, guildId, arg3);
            }
            if (emojiId == EmojiHelper.GetEmojiId(BossEnum.nightmare.ToString()))
            {
                await HandleNm(client, guildId, arg3);
            }
        }

        private static async Task HandleToB(DiscordSocketClient client, ulong guildId, SocketReaction arg3)
        {
            var guild = client.GetGuild(guildId);
            var currentUser = guild.Users.FirstOrDefault(x => x.Id == arg3.UserId);
            if (currentUser != null)
            {
                var rsn = DiscordNameHelper.GetAccountNameOrNickname(currentUser);

                if (!string.IsNullOrEmpty(rsn))
                {
                    // Get KC in WOM
                    await WOMMemoryCache.UpdateClanMember(WOMMemoryCache.OneHourMS, rsn);
                    var clanMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

                    if (clanMember != null)
                    {
                        var kills = clanMember.latestSnapshot.data.Bosses.theatre_of_blood.kills;

                        // set the role appriate
                        await PvmSystemHelper.CheckAndUpdateAccountAsync(
                        currentUser,
                        guild,
                        kills,
                        new PvmSet()
                        {
                            learner = Constants.TobLearner,
                            intermediate = Constants.ToBIntermediate,
                            advanced = Constants.ToBAdvanced,
                            imageUrl = Constants.TobImage
                        },
                        false,
                        true);
                    }
                }
            }
        }

        private static async Task HandleCoX(DiscordSocketClient client, ulong guildId, SocketReaction arg3)
        {
            var guild = client.GetGuild(guildId);
            var currentUser = guild.Users.FirstOrDefault(x => x.Id == arg3.UserId);
            if (currentUser != null)
            {
                var rsn = DiscordNameHelper.GetAccountNameOrNickname(currentUser);

                if (!string.IsNullOrEmpty(rsn))
                {
                    // Get KC in WOM
                    await WOMMemoryCache.UpdateClanMember(WOMMemoryCache.OneHourMS, rsn);
                    var clanMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

                    if (clanMember != null)
                    {
                        var kills = clanMember.latestSnapshot.data.Bosses.chambers_of_xeric.kills + clanMember.latestSnapshot.data.Bosses.chambers_of_xeric_challenge_mode.kills;

                        // set the role appriate
                        await PvmSystemHelper.CheckAndUpdateAccountAsync(
                        currentUser,
                        guild,
                        kills,
                        new PvmSet()
                        {
                            learner = Constants.CoxLearner,
                            intermediate = Constants.CoxIntermediate,
                            advanced = Constants.CoxAdvanced,
                            imageUrl = Constants.CoxImage
                        },
                        false,
                        true);
                    }
                }
            }
        }

        private static async Task HandleNm(DiscordSocketClient client, ulong guildId, SocketReaction arg3)
        {
            var guild = client.GetGuild(guildId);
            var currentUser = guild.Users.FirstOrDefault(x => x.Id == arg3.UserId);
            if (currentUser != null)
            {
                var rsn = DiscordNameHelper.GetAccountNameOrNickname(currentUser);

                if (!string.IsNullOrEmpty(rsn))
                {
                    // Get KC in WOM
                    await WOMMemoryCache.UpdateClanMember(WOMMemoryCache.OneHourMS, rsn);
                    var clanMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

                    if (clanMember != null)
                    {
                        var kills = clanMember.latestSnapshot.data.Bosses.nightmare.kills;

                        // set the role appriate
                        await PvmSystemHelper.CheckAndUpdateAccountAsync(
                        currentUser,
                        guild,
                        kills,
                        new PvmSet()
                        {
                            learner = Constants.NmLearner,
                            intermediate = Constants.NmIntermediate,
                            advanced = Constants.NmAdvanced,
                            imageUrl = Constants.nmImage
                        },
                        false,
                        true);
                    }
                }
            }
        }
    }
}
