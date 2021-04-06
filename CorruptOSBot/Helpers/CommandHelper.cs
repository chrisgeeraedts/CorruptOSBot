using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Helpers
{
    public static class CommandHelper
    {
        private static Dictionary<string, string> _commandsFromCode;
        public static Dictionary<string, string> GetCommandsFromCode()
        {
            if (_commandsFromCode == null)
            {
                _commandsFromCode = LoadCommandsFromCode();
            }
            return _commandsFromCode;
        }


        private static Dictionary<string, string> LoadCommandsFromCode()
        {
            var result = new Dictionary<string, string>();

            var commandMethods = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(assembly => assembly.GetTypes())
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                      .ToArray();




            foreach (var method in commandMethods)
            {
                SummaryAttribute attrSummary = (SummaryAttribute)method.GetCustomAttributes(typeof(SummaryAttribute), true)[0];
                string valueSummary = attrSummary.Text;

                CommandAttribute attrCommand = (CommandAttribute)method.GetCustomAttributes(typeof(CommandAttribute), true)[0];
                string valueCommand = attrCommand.Text;

                string commandLine = string.Format("!{0}", valueCommand);
                if (!result.ContainsKey(commandLine))
                {
                    result.Add(commandLine, valueSummary);
                }
            }

            return result;
        }

        internal static Dictionary<string, string> GetEnabledCommandsFromCode()
        {
            var result = new Dictionary<string, string>();

            foreach (var item in GetCommandsFromCode())
            {
                if (RootAdminManager.GetToggleState(item.Key.Replace("!", string.Empty)))
                {
                    result.Add(item.Key, item.Value);
                }
            }

            return result;
        }
    }
}
