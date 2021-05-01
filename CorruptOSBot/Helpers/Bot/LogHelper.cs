using CorruptOSBot.Data;
using Discord;
using System;
using System.Threading.Tasks;

namespace CorruptOSBot.Helpers.Bot
{

    public static class LogHelper
    {
        public static void ChatLog(LogMessage message)
        {
            try
            {
                using (CorruptModel data = new CorruptModel())
                {
                    var chatLog = new ChatLog() { Message = message.Message, Author = message.Source, Severity = message.Severity.ToString(), Datetime = DateTime.Now };
                    data.ChatLogs.Add(chatLog);
                    data.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log(new LogMessage(LogSeverity.Error, "ChatLog", "Failed to add chatlog - " + e.Message));
            }
        }

        public static Task Log(LogMessage message)
        {
            var messageString = string.Format($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");

            // check the settings if we should log outside
            try
            {
                if (message.Severity == LogSeverity.Error || message.Severity == LogSeverity.Critical)
                {
                    using (CorruptModel data = new CorruptModel())
                    {
                        var errorLog = new ErrorLog() { Message = message.Message, Severity = message.Severity.ToString() };
                        data.ErrorLogs.Add(errorLog);
                        data.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                // cant even log it - sad panda, but the show must go on.
            }

            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine(messageString);
            Console.ResetColor();

            // If you get an error saying 'CompletedTask' doesn't exist,
            // your project is targeting .NET 4.5.2 or lower. You'll need
            // to adjust your project's target framework to 4.6 or higher
            // (instructions for this are easily Googled).
            // If you *need* to run on .NET 4.5 for compat/other reasons,
            // the alternative is to 'return Task.Delay(0);' instead.
            return Task.CompletedTask;
        }
    }
}
