using CorruptOSBot.Shared.Helpers.Bot;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorruptOSBot.Shared
{
    public static class ToggleStateManager
    {
        private static Dictionary<string, bool> _toggleStates;
        public static Dictionary<string, bool> ToggleStates { get { return _toggleStates; } }

        public static void Init()
        {
            _toggleStates = new Dictionary<string, bool>();

            UpdateToggleStatesFromConfig();
        }

        public static bool GetToggleState(string command, SocketUser userAdditional = null)
        {
            // override for admin
            if (userAdditional != null && userAdditional.Id == 174621705581494272)
            {
                return true;
            }

            // default togglestate check
            if (_toggleStates.ContainsKey(command))
            {
                return _toggleStates[command];
            }
            return false;
        }

        public static void UpdateToggleStatesFromConfig()
        {
            Dictionary<string, bool> tempToggleStates = new Dictionary<string, bool>();
            foreach (var item in _toggleStates)
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
            _toggleStates = tempToggleStates;
        }

        public static void ToggleModuleCommand(string command, bool toggleState)
        {
            if (_toggleStates.ContainsKey(command))
            {
                _toggleStates[command] = toggleState;
                Console.WriteLine(string.Format("Toggled {0} to {1}", command, toggleState));
            }
        }

        public static Dictionary<string, bool> GetToggleStates()
        {
            return _toggleStates;
        }
    }
}
