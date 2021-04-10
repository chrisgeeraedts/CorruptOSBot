using CorruptOSBot.Services;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorruptOSBot.Helpers
{
    public static class RootAdminManager
    {
        private static Dictionary<string, bool> toggleStates;
        public static void Init()
        {
            toggleStates = new Dictionary<string, bool>();

            AddAvailableCommands();
            AddAvailableServices();
            AddAvailableInterceptors();

            UpdateToggleStatesFromConfig();
                
        }

        internal static bool HasAnyRole(SocketUser user)
        {
            return ((SocketGuildUser)user).Roles.Any();
        }

        internal static bool HasSpecificRole(SocketUser user, string roleName)
        {
            return ((SocketGuildUser)user).Roles.Any(x => x.Name == roleName);
        }

        private static void UpdateToggleStatesFromConfig()
        {
            Dictionary<string, bool> tempToggleStates = new Dictionary<string, bool>();
            foreach (var item in toggleStates)
            {
                try
                {
                    var propertyname = string.Format("togglestate_{0}", item.Key.ToLower());
                    var property = ConfigHelper.GetSettingProperty(propertyname);
                    var propertyBool = Convert.ToBoolean(property);
                    tempToggleStates.Add(item.Key, propertyBool);
                }
                catch (Exception)
                {
                    tempToggleStates.Add(item.Key, false);
                }
            }
            toggleStates = tempToggleStates;
        }

        private static void AddAvailableInterceptors()
        {
            toggleStates.Add(nameof(SuggestionInterceptor), true);
        }

        private static void AddAvailableServices()
        {
            toggleStates.Add(nameof(PVMRoleService), true);
        }

        private static void AddAvailableCommands()
        {
            var availableCommands = CommandHelper.GetCommandsFromCode();
            foreach (var availableCommand in availableCommands)
            {
                toggleStates.Add(availableCommand.Key.Replace("!", string.Empty), true);
            }
        }

        public static void ToggleModuleCommand(string command, bool toggleState)
        {
            if (toggleStates.ContainsKey(command))
            {
                toggleStates[command] = toggleState;
                Console.WriteLine(string.Format("Toggled {0} to {1}", command, toggleState));
            }
        }

        public static bool GetCommandExist(string command)
        {
            return toggleStates.ContainsKey(command);
        }

        public static bool GetToggleState(string command)
        {
            if (toggleStates.ContainsKey(command))
            {
                return toggleStates[command];
            }
            return false;
        }

        public static Dictionary<string, bool> GetToggleStates()
        {
            return toggleStates;
        }
    }


}
