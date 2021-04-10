﻿using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class PVMModule : ModuleBase<SocketCommandContext>
    {
        [Command("cox")]
        [Summary("Enables a player to earn the 'CoX learner' role")]
        public async Task SayCoxAsync()
        {
            if (RootAdminManager.GetToggleState("cox") && RootAdminManager.HasAnyRole(Context.User))
            {
                if (Context.Channel.Id == ChannelHelper.GetChannelId("set-pvm-roles"))
                {
                    // check if user has learner/intermediate/advanced
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // Get KC in WOM
                        await WOMMemoryCache.UpdateClanMember(WOMMemoryCache.OneHourMS, rsn);
                        var clanMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

                        if (clanMember != null)
                        {
                            var kills = clanMember.latestSnapshot.chambers_of_xeric_challenge_mode.kills + clanMember.latestSnapshot.chambers_of_xeric.kills;

                            // set the role appriate
                            await PvmSystemHelper.CheckAndUpdateAccountAsync(
                            currentUser,
                            Context.Guild,
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

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Command("tob")]
        [Summary("Enables a player to earn the 'ToB learner' role")]
        public async Task SayTobAsync()
        {
            if (RootAdminManager.GetToggleState("tob") && RootAdminManager.HasAnyRole(Context.User))
            {
                if (Context.Channel.Id == ChannelHelper.GetChannelId("set-pvm-roles"))
                {
                    // check if user has learner/intermediate/advanced
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // Get KC in WOM
                        await WOMMemoryCache.UpdateClanMember(WOMMemoryCache.OneHourMS, rsn);
                        var clanMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

                        if (clanMember != null)
                        {

                            var kills = clanMember.latestSnapshot.theatre_of_blood.kills;

                            // set the role appriate
                            await PvmSystemHelper.CheckAndUpdateAccountAsync(
                            currentUser,
                            Context.Guild,
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

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Command("nm")]
        [Summary("Enables a player to earn the 'nm learner' role")]
        public async Task SayNmAsync()
        {
            if (RootAdminManager.GetToggleState("nm") && RootAdminManager.HasAnyRole(Context.User))
            {
                if (Context.Channel.Id == ChannelHelper.GetChannelId("set-pvm-roles"))
                {
                    // check if user has learner/intermediate/advanced
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // Get KC in WOM
                        await WOMMemoryCache.UpdateClanMember(WOMMemoryCache.OneHourMS, rsn);
                        var clanMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

                        if (clanMember != null)
                        {

                            var kills = clanMember.latestSnapshot.nightmare.kills;

                            // set the role appriate
                            await PvmSystemHelper.CheckAndUpdateAccountAsync(
                            currentUser,
                            Context.Guild,
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


                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }
    }
}