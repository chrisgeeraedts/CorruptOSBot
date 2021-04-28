using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Services;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorruptOSBot.Shared
{
    public static class RootAdminManager
    {
        private static string BotVersion;
        public static void Init()
        {

            AddAvailableCommands();
            AddAvailableServices();
            AddAvailableEvents();
            AddAvailableInterceptors();

            ToggleStateManager.UpdateToggleStatesFromConfig();

            BotVersion = ConfigHelper.GetSettingProperty("bot-version");
        }

        public static bool HasAnyRole(SocketUser user)
        {
            return ((SocketGuildUser)user).Roles.Any();
        }



       

        public static string GetBotVersion()
        {
            return BotVersion;
        }

        private static void AddAvailableInterceptors()
        {
            ToggleStateManager.ToggleStates.Add(nameof(SuggestionInterceptor), true);
        }

        private static void AddAvailableServices()
        {
            ToggleStateManager.ToggleStates.Add(nameof(PVMRoleService), true);
            ToggleStateManager.ToggleStates.Add(nameof(AchievementService), true);
            ToggleStateManager.ToggleStates.Add(nameof(TopKCService), true);
            ToggleStateManager.ToggleStates.Add(nameof(HeartbeatService), true);
        }
        private static void AddAvailableEvents()
        {
            ToggleStateManager.ToggleStates.Add(Constants.EventUserJoined, true);
            ToggleStateManager.ToggleStates.Add(Constants.EventUserBanned, true);
            ToggleStateManager.ToggleStates.Add(Constants.EventUserLeft, true);
        }


        private static void AddAvailableCommands()
        {
            var availableCommands = CommandHelper.GetCommandsFromCode();
            foreach (var availableCommand in availableCommands)
            {
                ToggleStateManager.ToggleStates.Add(availableCommand.Key.Replace("!", string.Empty), true);
            }
        }

        

        public static bool GetCommandExist(string command)
        {
            return ToggleStateManager.ToggleStates.ContainsKey(command);
        }


        
    }
}
