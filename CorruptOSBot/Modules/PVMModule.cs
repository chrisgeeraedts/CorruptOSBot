using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Helpers.PVM;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class PVMModule : ModuleBase<SocketCommandContext>
    {
        [Command("cox")]
        [Summary("!cox - Enables a player to earn the 'CoX learner' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayCoxAsync()
        {
            if (RootAdminManager.GetToggleState("cox", Context.User) && RootAdminManager.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
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
                else
                {
                    await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!cox", "set-pvm-roles");
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Command("tob")]
        [Summary("!tob - Enables a player to earn the 'ToB learner' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayTobAsync()
        {
            if (RootAdminManager.GetToggleState("tob", Context.User) && RootAdminManager.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
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
                else
                {
                    await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!tob", "set-pvm-roles");
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Command("nm")]
        [Summary("!nm - Enables a player to earn the 'nm learner' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayNmAsync()
        {
            if (RootAdminManager.GetToggleState("nm", Context.User) && RootAdminManager.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
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
                else
                {
                    await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!nm", "set-pvm-roles");
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }

        [Command("cm")]
        [Summary("!cm - Enables a player to earn the 'Challenge Mode' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayCMAsync()
        {
            if (RootAdminManager.GetToggleState("cm", Context.User) && RootAdminManager.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
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
                            var kills = clanMember.latestSnapshot.chambers_of_xeric_challenge_mode.kills;

                            // set the role appriate
                            await PvmSystemHelper.CheckAndUpdateAccountCMAsync(
                            currentUser,
                            Context.Guild,
                            kills,
                            new PvmSetCM()
                            {
                                role = Constants.CM,
                                imageUrl = Constants.CoxImage
                            },
                            false,
                            true);
                        }
                    }
                }
                else
                {
                    await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!cm", "set-pvm-roles");
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }


        [Command("gwd")]
        [Summary("!gwd - Enables a player to earn the 'GWD' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayGWDAsync()
        {
            if (RootAdminManager.GetToggleState("gwd", Context.User) && RootAdminManager.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
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
                            // set the role appriate
                            await PvmSystemHelper.CheckAndUpdateAccountNoKCAsync(
                            currentUser,
                            Context.Guild,
                            new PvmSetCM()
                            {
                                role = Constants.GWD,
                                imageUrl = Constants.GWDImage
                            },
                            false,
                            true);
                        }
                    }
                }
                else
                {
                    await DiscordHelper.NotAlloweddMessageToUser(Context.User, "!cm", "set-pvm-roles");
                }

                // delete the command posted
                await Context.Message.DeleteAsync();
            }
        }
    }
}