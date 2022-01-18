using CorruptOSBot.Events;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Modules;
using CorruptOSBot.Services;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CorruptOSBot
{
    internal class Program
    {
        private readonly DiscordSocketClient client;

        public static DateTime OnlineFrom { get; set; }

        private readonly CommandService commands;
        private readonly IServiceProvider services;
        private readonly Dictionary<ulong, Func<SocketMessage, IDiscordClient, Task>> channelInterceptors;

        private List<IService> activeServices;

        private static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private Program()
        {
            OnlineFrom = DateTime.Now;

            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                AlwaysDownloadUsers = true
            });

            commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false,
            });

            client.Log += Log;
            commands.Log += Log;
            services = ConfigureServices();

            ConfigHelper.Init();

            channelInterceptors = ConfigureChannelInterceptors();
            activeServices = ConfigureActiveServices();
        }

        private List<IService> ConfigureActiveServices()
        {
            var result = new List<IService>
            {
                new PVMRoleService(client),
                new AchievementService(client),
                new TopKCService(client),
                new HeartbeatService(client),
                new SotWService(client),
                new PromotionService(client)
            };

            return result;
        }

        private static IServiceProvider ConfigureServices()
        {
            var map = new ServiceCollection();
            
            return map.BuildServiceProvider();
        }

        public static Task Log(LogMessage message)
        {
            return LogHelper.Log(message);
        }

        private async Task MainAsync()
        {
            await InitCommands();

            RootAdminManager.Init();

            if (!ConfigHelper.IsDebugMode)
            {
                await client.LoginAsync(TokenType.Bot, ConfigHelper.GetSettingProperty("DiscordToken"));
            }
            else
            {
                await client.LoginAsync(TokenType.Bot, ConfigHelper.GetSettingProperty("TestDiscordToken"));
            }

            await client.StartAsync();

            await LoadMemoryCache();

            StartServiceThreads(client);

            foreach (var item in ToggleStateManager.GetToggleStates().OrderBy(X => X.Type).ThenBy(x => x.Functionality))
            {
                await Log(new LogMessage(LogSeverity.Info, "Toggle states:", string.Format("[{2}] {0}: {1}", item.Functionality, item.Toggled, item.Type)));
            }

            await Task.Delay(Timeout.Infinite);
        }

        private async Task LoadMemoryCache()
        {
            if (!ConfigHelper.IsDebugMode)
            {
                await WOMMemoryCache.UpdateClanMembers(WOMMemoryCache.OneHourMS);
                await WOMMemoryCache.UpdateClan(WOMMemoryCache.OneHourMS);
            }
        }

        private void StartServiceThreads(DiscordSocketClient client)
        {
            foreach (var _activeService in activeServices)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(_activeService.BeforeTriggerTimeInMS);

                        Thread.CurrentThread.IsBackground = true;

                        if (!ConfigHelper.IsDebugMode)
                        {
                            _activeService.Trigger(client);
                        }

                        Thread.Sleep(_activeService.TriggerTimeInMS);
                    }
                }).Start();
            }
        }

        private Dictionary<ulong, Func<SocketMessage, IDiscordClient, Task>> ConfigureChannelInterceptors()
        {
            var result = new Dictionary<ulong, Func<SocketMessage, IDiscordClient, Task>>();

            AddToChannelInterceptorDictionary("suggestions", new SuggestionInterceptor().Trigger, result);

            return result;
        }

        private void AddToChannelInterceptorDictionary(string channelName, Func<SocketMessage, IDiscordClient, Task> func, Dictionary<ulong, Func<SocketMessage, IDiscordClient, Task>> dictionaryToAddTo)
        {
            var channelId = ChannelHelper.GetChannelId(channelName);
            if (channelId != 0)
            {
                dictionaryToAddTo.Add(channelId, func);
            }
        }

        private async Task InitCommands()
        {
            await commands.AddModuleAsync<AdminModule>(services);
            await commands.AddModuleAsync<AccountModule>(services);
            await commands.AddModuleAsync<HelpModule>(services);
            await commands.AddModuleAsync<BossKcModule>(services);
            await commands.AddModuleAsync<KcModule>(services);
            await commands.AddModuleAsync<RSNModule>(services);
            await commands.AddModuleAsync<ScoreModule>(services);
            await commands.AddModuleAsync<WoMModule>(services);
            await commands.AddModuleAsync<PVMModule>(services);
            await commands.AddModuleAsync<RoleModule>(services);
            await commands.AddModuleAsync<CalendarModule>(services);

            client.MessageReceived += HandleCommandAsync;
            client.UserJoined += Client_UserJoined;
            client.UserLeft += Client_UserLeft;
            client.UserBanned += Client_UserBanned;
            client.ReactionAdded += Client_ReactionAdded;
            client.CurrentUserUpdated += Client_CurrentUserUpdated;
        }

        private async Task Client_CurrentUserUpdated(SocketSelfUser arg1, SocketSelfUser arg2)
        {
            await Log(new LogMessage(LogSeverity.Info, "Users", "_client_CurrentUserUpdated"));
        }

        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            await Log(new LogMessage(LogSeverity.Info, "Users", string.Format("Reaction: {0}: {1}", arg3.Emote.Name, arg3.Emote.ToString())));
        }

        private async Task Client_UserJoined(SocketGuildUser arg)
        {
            if (ToggleStateManager.GetToggleState(Constants.EventUserJoined))
            {
                await Log(new LogMessage(LogSeverity.Info, "Users", string.Format("User joined: {0}", arg.Username)));
                await EventManager.JoinedGuild(arg);
            }
        }

        private async Task Client_UserLeft(SocketGuildUser arg)
        {
            if (ToggleStateManager.GetToggleState(Constants.EventUserLeft))
            {
                if (arg.IsBot || arg.IsWebhook) return;
                await Log(new LogMessage(LogSeverity.Info, "Users", string.Format("User left: {0}", arg.Username)));
                await EventManager.LeavingGuild(arg);
            }
        }

        private async Task Client_UserBanned(SocketUser arg1, SocketGuild arg2)
        {
            if (ToggleStateManager.GetToggleState(Constants.EventUserBanned))
            {
                await Log(new LogMessage(LogSeverity.Info, "Users", string.Format("User banned: {0}", arg1.Username)));
                await EventManager.BannedFromGuild(arg1, arg2);
            }
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // Bail out if it's a System Message.
            var msg = arg as SocketUserMessage;
            if (msg == null) return;

            // We don't want the bot to respond to itself or other bots.
            if (msg.Author.Id == client.CurrentUser.Id || msg.Author.IsBot) return;

            //await BlockMessageIfDebugMode(msg);

            try
            {
                ChatLog(arg);
            }
            catch (Exception e)
            {
                await Log(new LogMessage(LogSeverity.Error, "chatlog", e.Message, e));
            }

            int posX = 0;
            if (channelInterceptors.ContainsKey(arg.Channel.Id) && !msg.HasCharPrefix('!', ref posX))
            {
                await channelInterceptors[arg.Channel.Id](arg, client);
            }
            else
            {
                int pos = 0;

                if (msg.HasCharPrefix('!', ref pos))
                {
                    var context = new SocketCommandContext(client, msg);
                    var result = await commands.ExecuteAsync(context, pos, services);

                    if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    {
                        await LogHelper.Log(new LogMessage(LogSeverity.Error, msg.Content, result.ErrorReason));
                    }
                }
            }
        }

        private async Task BlockMessageIfDebugMode(SocketUserMessage msg)
        {
            var isDeveloper = msg.Author.Id == 108710294049542144 || msg.Author.Id == 391595176314798090; //GMKirby | Carn3asada Discord Id

            if (ConfigHelper.IsDebugMode && !isDeveloper)
            {
                var context = new SocketCommandContext(client, msg);
                var message = await context.Channel.SendMessageAsync("The corrupt bot is currently in development mode.");

                await msg.DeleteAsync();
                await Task.Delay(5000).ContinueWith(t => message.DeleteAsync());
            }
        }

        private static void ChatLog(SocketMessage arg)
        {
            LogHelper.ChatLog(arg);
        }
    }
}