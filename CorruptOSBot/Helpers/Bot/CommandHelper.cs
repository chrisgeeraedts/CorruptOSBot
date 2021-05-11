using CorruptOSBot.Modules;
using CorruptOSBot.Shared;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CorruptOSBot.Helpers.Bot
{
    public static class CommandHelper
    {
        private static Dictionary<string, CommandHelp> _commandsFromCode;
        public static Dictionary<string, CommandHelp> GetCommandsFromCode()
        {
            if (_commandsFromCode == null)
            {
                _commandsFromCode = LoadCommandsFromCode();
            }
            return _commandsFromCode;
        }


        private static Dictionary<string, CommandHelp> LoadCommandsFromCode()
        {
            var result = new Dictionary<string, CommandHelp>();
            var blacklist = new List<string>() { "!overthrownathan" };

            var commandMethods = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(assembly => assembly.GetTypes())
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                      .ToArray();

            foreach (var method in commandMethods)
            {
                try
                {
                    string valueSummary = string.Empty;
                    var summaryAttributes = method.GetCustomAttributes(typeof(SummaryAttribute), true);
                    if (summaryAttributes.Any())
                    {
                        SummaryAttribute attrSummary = (SummaryAttribute)summaryAttributes[0];
                        valueSummary = attrSummary.Text;
                    }


                    string valueCommand = string.Empty;
                    var commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                    if (commandAttributes.Any())
                    {
                        CommandAttribute attrCommand = (CommandAttribute)commandAttributes[0];
                        valueCommand = attrCommand.Text;
                    }
                    

                    HelpGroup valueGroup = HelpGroup.Undefined;
                    var helpAttributes = method.GetCustomAttributes(typeof(HelpgroupAttribute), true);
                    if (helpAttributes.Any())
                    {
                        HelpgroupAttribute attrHelpGroup = (HelpgroupAttribute)helpAttributes[0];
                        valueGroup = attrHelpGroup.HelpGroup;
                    }
                    

                    string commandLine = string.Format("!{0}", valueCommand);
                    if (!result.ContainsKey(commandLine) && !blacklist.Contains(commandLine.ToLower()))
                    {
                        result.Add(commandLine, new CommandHelp() { Name = string.Format("({0}) {1}", valueGroup, valueSummary), HelpGroup = valueGroup });
                    }
                }
                catch (Exception e)
                {
                    Program.Log(new LogMessage(LogSeverity.Error, "ChatLog", "Failed to add chatlog - " + e.Message));
                }
                
            }
            return result;
        }


        internal static Dictionary<string, CommandHelp> GetEnabledCommandsFromCode()
        {
            var result = new Dictionary<string, CommandHelp>();

            foreach (var item in GetCommandsFromCode())
            {
                if (ToggleStateManager.GetToggleState(item.Key.Replace("!", string.Empty)))
                {
                    result.Add(item.Key, new CommandHelp() { Name = item.Value.Name, HelpGroup = item.Value.HelpGroup });
                }
            }
            return OrderCommandsByRoles(result);
        }

        internal static Dictionary<string, CommandHelp> OrderCommandsByRoles(Dictionary<string, CommandHelp> input)
        {
            var result = new Dictionary<string, CommandHelp>();

            // First add the dev roles
            foreach (var item in input.Where(x => x.Value.HelpGroup == HelpGroup.Admin))
            {
                if (!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, item.Value);
                }
            }

            // Second add the staff roles
            foreach (var item in input.Where(x => x.Value.HelpGroup == HelpGroup.Staff))
            {
                if (!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, item.Value);
                }
            }

            // thirdly add the mod roles
            foreach (var item in input.Where(x => x.Value.HelpGroup == HelpGroup.Moderator))
            {
                if (!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, item.Value);
                }
            }
            
            // Fourthly add the member roles
            foreach (var item in input.Where(x => x.Value.HelpGroup == HelpGroup.Member))
            {
                if (!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, item.Value);
                }
            }

            // then add everything else
            foreach (var item in input.Where(x => !result.ContainsKey(x.Key)))
            {
                if (!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, item.Value);
                }
            }
            return result;
        }
    }

    public class CommandHelp
    {
        public string Name { get; set; }
        public HelpGroup HelpGroup { get; set; }
    }
}
