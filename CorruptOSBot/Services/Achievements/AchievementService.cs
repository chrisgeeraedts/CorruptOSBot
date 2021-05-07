﻿using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Services;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace CorruptOSBot
{
    internal class AchievementService : IService
    {
        public int TriggerTimeInMS { get => 1000 * 60 * 60 * 24; } // every 3  days
        public int BeforeTriggerTimeInMS { get => 1000 * 60 * 3; } // 3 minute
        private ulong GuildId;


        public AchievementService(Discord.IDiscordClient client)
        {
            Program.Log(new LogMessage(LogSeverity.Info, "AchievementService", "Created, triggering every " + TriggerTimeInMS + "MS"));
            GuildId = ConfigHelper.GetGuildId();
        }

        public async Task Trigger(IDiscordClient client)
        {
            if (ToggleStateManager.GetToggleState(nameof(AchievementService)))
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "AchievementService", "Triggered"));

                // find current channel
                var guild = await client.GetGuildAsync(GuildId);
                var channel = await guild.GetChannelAsync(ChannelHelper.GetChannelId(SettingsConstants.ChannelAchievements)) as SocketTextChannel;

                // Add new message
                if (channel != null)
                {
                    var embed = EmbedHelper.CreateWOMEmbed();
                    if (embed != null)
                    {
                        await (channel as SocketTextChannel).SendMessageAsync(embed: embed);
                    }
                }
            }
        }
    }
}