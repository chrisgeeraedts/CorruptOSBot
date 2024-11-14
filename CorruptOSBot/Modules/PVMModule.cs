using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Helpers.PVM;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class PVMModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("cox")]
        [Summary("!cox - Enables a player to earn the 'CoX learner' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayCoxAsync()
        {
            if (ToggleStateManager.GetToggleState("cox", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    // check if user has learner/intermediate/advanced
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // Get KC in WOM
                        await WOMMemoryCache.UpdateClanMember(WOMMemoryCache.OneHourMS, rsn);
                        var clanMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

                        if (clanMember != null)
                        {
                            var kills = clanMember.latestSnapshot.data.Bosses.chambers_of_xeric_challenge_mode.kills + clanMember.latestSnapshot.data.Bosses.chambers_of_xeric.kills;

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
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!cox", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("tob")]
        [Summary("!tob - Enables a player to earn the 'ToB learner' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayTobAsync()
        {
            if (ToggleStateManager.GetToggleState("tob", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    // check if user has learner/intermediate/advanced
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

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
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!tob", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("nm")]
        [Summary("!nm - Enables a player to earn the 'nm learner' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayNmAsync()
        {
            if (ToggleStateManager.GetToggleState("nm", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    // check if user has learner/intermediate/advanced
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

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
                                role = Constants.Nightmare,
                                imageUrl = Constants.nmImage
                            },
                            false,
                            true);
                        }
                    }
                }
                else
                {
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!nm", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("cm")]
        [Summary("!cm - Enables a player to earn the 'Cox Challenge Mode' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayCMAsync()
        {
            if (ToggleStateManager.GetToggleState("cm", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    // check if user has learner/intermediate/advanced
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // Get KC in WOM
                        await WOMMemoryCache.UpdateClanMember(WOMMemoryCache.OneHourMS, rsn);
                        var clanMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

                        if (clanMember != null)
                        {
                            var kills = clanMember.latestSnapshot.data.Bosses.chambers_of_xeric_challenge_mode.kills;

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
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!cm", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("hmt")]
        [Summary("!hmt - Enables a player to earn the 'Tob Hard Mode' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayHMTAsync()
        {
            if (ToggleStateManager.GetToggleState("hmt", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    // check if user has learner/intermediate/advanced
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // Get KC in WOM
                        await WOMMemoryCache.UpdateClanMember(WOMMemoryCache.OneHourMS, rsn);
                        var clanMember = WOMMemoryCache.ClanMemberDetails.ClanMemberDetails.FirstOrDefault(x => x.displayName.ToLower() == rsn.ToLower());

                        if (clanMember != null)
                        {
                            var kills = clanMember.latestSnapshot.data.Bosses.theatre_of_blood_hard_mode.kills;

                            // set the role appriate
                            await PvmSystemHelper.CheckAndUpdateAccountCMAsync(
                            currentUser,
                            Context.Guild,
                            kills,
                            new PvmSetCM()
                            {
                                role = Constants.HMT,
                                imageUrl = Constants.TobImage
                            },
                            false,
                            true);
                        }
                    }
                }
                else
                {
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!hmt", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("gwd")]
        [Summary("!gwd - Enables a player to earn the 'GWD' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayGWDAsync()
        {
            if (ToggleStateManager.GetToggleState("gwd", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    // check if user has learner/intermediate/advanced
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
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
                else
                {
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!gwd", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("corp")]
        [Summary("!corp - Enables a player to earn the 'Corp' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayCorpAsync()
        {
            if (ToggleStateManager.GetToggleState("corp", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    // check if user has learner/intermediate/advanced
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // set the role appriate
                        await PvmSystemHelper.CheckAndUpdateAccountNoKCAsync(
                        currentUser,
                        Context.Guild,
                        new PvmSetCM()
                        {
                            role = Constants.Corp,
                            imageUrl = Constants.CorpImage
                        },
                        false,
                        true);
                    }
                }
                else
                {
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!corp", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("nex")]
        [Summary("!nex - Enables a player to earn the 'Nex' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayNexAsync()
        {
            if (ToggleStateManager.GetToggleState("nex", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // set the role appriate
                        await PvmSystemHelper.CheckAndUpdateAccountNoKCAsync(
                        currentUser,
                        Context.Guild,
                        new PvmSetCM()
                        {
                            role = Constants.Nex,
                            imageUrl = Constants.NexImage
                        },
                        false,
                        true);
                    }
                }
                else
                {
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!nex", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("toa")]
        [Summary("!toa - Enables a player to earn the 'Toa' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayToAAsync()
        {
            if (ToggleStateManager.GetToggleState("toa", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // set the role appriate
                        await PvmSystemHelper.CheckAndUpdateAccountNoKCAsync(
                        currentUser,
                        Context.Guild,
                        new PvmSetCM()
                        {
                            role = Constants.TOA,
                            imageUrl = Constants.ToAImage
                        },
                        false,
                        true);
                    }
                }
                else
                {
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!toa", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("wildybosses")]
        [Summary("!wildybosses - Enables a player to earn the 'WildyBosses' role (Only allowed in **set-pvm-roles**)")]
        public async Task SayWildyBossesAsync()
        {
            if (ToggleStateManager.GetToggleState("wildybosses", Context.User) && RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    var currentUser = ((SocketGuildUser)Context.User);
                    var rsn = DiscordNameHelper.GetAccountNameOrNickname(Context.User);

                    if (!string.IsNullOrEmpty(rsn))
                    {
                        // set the role appriate
                        await PvmSystemHelper.CheckAndUpdateAccountNoKCAsync(
                        currentUser,
                        Context.Guild,
                        new PvmSetCM()
                        {
                            role = Constants.WildyBosses,
                            imageUrl = Constants.WildyBossesImage
                        },
                        false,
                        true);
                    }
                }
                else
                {
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!wildybosses", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("removePVMRole")]
        [Summary("!removePVMRole - Enables a player to remove a pvm role(Only allowed in **set-pvm-roles**)")]
        public async Task SayRemovePVMRoleAsync([Remainder] string roleName)
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                if (DiscordHelper.IsInChannel(Context.Channel.Id, "set-pvm-roles", Context.User))
                {
                    var currentUser = ((SocketGuildUser)Context.User);

                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == roleName.ToLower());
                    var hasRole = currentUser.Roles.Any(item => item.Id == role.Id);
                    var pvmRoles = new List<string>()
                    {
                        "Cox Advanced",
                        "Cox Intermediate",
                        "Cox Learner",
                        "Cox Challenge Mode",
                        "Tob Advanced",
                        "Tob Intermediate",
                        "Tob Learner",
                        "Tob Hard Mode",
                        "Nex",
                        "Nightmare",
                        "GWD",
                        "Corp",
                        "TOA",
                        "Wildy Bosses"
                    };

                    if (hasRole && pvmRoles.Any(item => item.ToLower() == roleName))
                    {
                        await currentUser.RemoveRoleAsync(role);
                    }
                }
                else
                {
                    await DiscordHelper.NotAllowedMessageToUser(Context, "!removePVMRole", "set-pvm-roles");
                }
            }

            await Context.Message.DeleteAsync();
        }
    }
}