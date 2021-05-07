using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using CorruptOSBot.TheHunt;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot
{
    public class PostCopyInterceptor : IInterceptor
    {
        public async Task Trigger(SocketMessage arg, Discord.IDiscordClient client)
        {
            var deleteOriginal = false;
            try
            {
                if (ToggleStateManager.GetToggleState(nameof(PostCopyInterceptor), arg.Author))
                {
                    // check if we should delete the original
                    deleteOriginal = Convert.ToBoolean(ConfigHelper.GetSettingProperty("thehunt-delete-original"));

                    // grab the target channel
                    var guildId = ConfigHelper.GetGuildId();
                    var guild = client.GetGuildAsync(guildId).Result;
                    var targetChannel = (IMessageChannel)guild.GetChannelsAsync().Result.FirstOrDefault(x => x.Id == ChannelHelper.GetChannelId("thehunttargetchannel"));
                    if (targetChannel != null && arg.Attachments.Any())
                    {
                        // copy the post to a target channel
                        foreach (var attachment in arg.Attachments)
                        {
                            try
                            {
                                // try to score the team
                                Team team = HuntManager.GetTeam(arg.Author);
                                BossEnum boss = HuntManager.DetermineBoss(arg.Content);
                                string item = HuntManager.DetermineItem(arg.Content, boss);
                                byte[] image = HuntManager.GetImageData(attachment);

                                if (team != null
                                    && boss != BossEnum.Undefined
                                    && !string.IsNullOrWhiteSpace(item)
                                    && image.Length > 0)
                                {
                                    // post to the seperate channels
                                    await targetChannel.SendMessageAsync(embed: new EmbedBuilder()
                                        .WithImageUrl(attachment.Url)
                                        .WithDescription(arg.Content)
                                        .WithTitle(String.Format("{0} posted a new item!", arg.Channel.Name))
                                        .Build());

                                    HuntManager.AddDrop(team, boss, item, image);
                                }
                                else
                                {
                                    await PostFailureToSetPointMessage(arg, targetChannel, attachment);
                                }
                            }
                            catch (Exception)
                            {
                                await PostFailureToSetPointMessage(arg, targetChannel, attachment);
                            }
                            

                        }
                    }
                }

            }
            catch (System.Exception e)
            {
            }


            if (deleteOriginal)
            {
                // delete the command posted
                await arg.DeleteAsync();
            }
        }

        private static async Task PostFailureToSetPointMessage(SocketMessage arg, IMessageChannel targetChannel, Attachment attachment)
        {
            await targetChannel.SendMessageAsync(embed: new EmbedBuilder()
           .WithImageUrl(attachment.Url)
           .WithDescription("Unable to add this item, please add it manually with !addhuntitem {teamname} {bossname} {itemname}")
           .WithTitle(String.Format("Unable to automatically add item for team {0}!", arg.Channel.Name))
           .Build());
        }
    }
}
