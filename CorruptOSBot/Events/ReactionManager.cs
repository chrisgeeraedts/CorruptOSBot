//using CorruptOSBot.Helpers.Discord;
//using CorruptOSBot.Shared.Helpers.Bot;
//using Discord;
//using Discord.WebSocket;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace CorruptOSBot.Events
//{
//    public static class ReactionManager
//    {
//        private static Dictionary<ulong, Func<DiscordSocketClient, ulong, Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task>> eventHandlers;
//        private static ulong GuildId;
//        public static void Init()
//        {
//            eventHandlers = new Dictionary<ulong, Func<DiscordSocketClient, ulong, Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task>>();
//            GuildId = Convert.ToUInt64(ConfigHelper.GetSettingProperty("GuildId"));

//            eventHandlers.Add(PostHelper.GetPostId("post_pvmSelection"), ReactionHandlers.PVMReactionHandler.Execute);
//        }

//        public async static Task ReactionPosted(DiscordSocketClient client, Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
//        {   
//            if (eventHandlers.ContainsKey(arg3.MessageId))
//            {
//                await eventHandlers[arg3.MessageId](client, GuildId, arg1, arg2, arg3);
//            }
//        }
//    }
//}
