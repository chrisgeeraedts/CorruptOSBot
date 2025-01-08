using CorruptOSBot.Data;
using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Helpers.PVM;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MoreLinq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class DevModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Admin)]
        [Command("dev1", false)]
        [Summary("!dev - Dev command")]
        public async Task Dev()
        {
            //var embedMessageList = await EmbedHelper.CreateFullLeaderboardEmbed(30);


            //foreach (var embedMessage in embedMessageList)
            //{
            //    await Context.Channel.SendMessageAsync(embed: embedMessage);
            //}

            //var looper = true;

            //while (looper)
            //{
            //    var livedrops = Context.Guild.GetChannel(869521931379019796) as SocketTextChannel;
            //    var messages = await livedrops.GetMessagesAsync(100).FlattenAsync();

            //    var messagesList = messages.ToList();
            //    foreach (var message in messages.ToList())
            //    {
            //        var messagePostion = messagesList.IndexOf(message);
            //        if (messagePostion == 0 || messagePostion == 99)
            //        {

            //        }
            //        else
            //        {
            //            var previousMessage = messagesList[messagePostion - 1];

            //            if (string.IsNullOrEmpty(message.Content) && message.Embeds.Count == 0 &&
            //                string.IsNullOrEmpty(previousMessage.Content) && previousMessage.Embeds.Count == 0)
            //            {
            //                await livedrops.DeleteMessageAsync(message);
            //            }
            //        }
            //    }

            //    var groupedMessages = messages.Where(item => item.Embeds.Count > 0 &&
            //    ((item.Embeds.FirstOrDefault().Title != null && (item.Embeds.FirstOrDefault().Title.Contains("Seasonal")) ||
            //    item.Embeds.Any(x => x.Author.HasValue && x.Author.Value.Name.Contains("ltrn")) ||
            //    item.Embeds.Any(x => x.Author.HasValue && x.Author.Value.Name.Contains("GPH00KZ")) ||
            //    item.Embeds.Any(x => x.Author.HasValue && x.Author.Value.Name.Contains("o RICK o")) ||
            //    item.Embeds.Any(x => x.Author.HasValue && x.Author.Value.Name.Contains("srdn")) ||
            //    item.Embeds.Any(x => x.Author.HasValue && x.Author.Value.Name.Contains("Alien45678")) ||
            //    item.Embeds.Any(x => x.Author.HasValue && x.Author.Value.Name.Contains("HawaiianSuns")) ||
            //    item.Embeds.Any(x => x.Author.HasValue && x.Author.Value.Name.Contains("IronAustinz")) ||
            //    item.Embeds.Any(x => x.Author.HasValue && x.Author.Value.Name.Contains("Fake Charter")) ||
            //    item.Embeds.Any(x => x.Author.HasValue && x.Author.Value.Name.Contains("kingwalker12")))));

            //    await livedrops.DeleteMessagesAsync(groupedMessages);

            //    var latestMessage = await livedrops.GetMessagesAsync(1).FlattenAsync();
            //    looper = latestMessage.FirstOrDefault().CreatedAt > new DateTimeOffset(2024, 11, 27, 13, 0, 0, new TimeSpan());
            //}

            //await Context.Message.DeleteAsync();
        }


        [Helpgroup(HelpGroup.Admin)]
        [Command("NewPhrase", false)]
        [Summary("!NewPhrase - Generates a new phrase for someone to use")]
        public async Task RandomEventPhrase()
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "clan-bot", Context.User) && Context.User.Id == SettingsConstants.GMKirbyDiscordId)
            {
                var text = "This is a test";
                var font = new Font("Arial", 12);

                ////Image img = new Bitmap(1, 1);
                //Graphics drawing = Graphics.FromImage(img);

                //SizeF textSize = drawing.MeasureString(text, font);

                ////free up the dummy image and old graphics object
                //img.Dispose();
                //drawing.Dispose();

                ////create a new image of the right size
                //img = new Bitmap((int)textSize.Width, (int)textSize.Height);

                //drawing = Graphics.FromImage(img);

                ////paint the background
                //drawing.Clear(Color.Green);

                ////create a brush for the text
                //Brush textBrush = new SolidBrush(Color.Black);

                //drawing.DrawString(text, font, textBrush, 0, 0);

                //drawing.Save();

                //textBrush.Dispose();
                //drawing.Dispose();

                //img.Save("TempImage.png", ImageFormat.Png);
                //await Context.Channel.SendFileAsync("TempImage.png", "This is a test");
                //File.Delete("TempImage.png");
            }

            await Context.Message.DeleteAsync();
        }

        private async Task OnHold()
        {
            var bossKCs = await BossKCHelper.GetTopBossKC();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.ContinuousSize(PageSizes.A1.Width);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.Black);
                    page.DefaultTextStyle(x => x.FontSize(20));
                    //page.Background().Extend().Image("ABackground.png");

                    page.Header()
                        .AlignCenter()
                        .Text("Top Boss KC")
                        .SemiBold()
                        .FontFamily("RuneScape UF")
                        .FontSize(36)
                        .FontColor("#ffff00");

                    page.Content().Column(column =>
                    {
                        column.Spacing(20);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            foreach (var boss in bossKCs)
                            {
                                if (boss.KcPlayers.Count == 3)
                                {
                                    table.Cell().Text($"").FontSize(12).FontFamily("RuneScape UF");

                                    foreach (var player in boss.KcPlayers)
                                    {
                                        table.Cell().Text($"{player.Player} - {player.Kc}").FontSize(18).FontFamily("RuneScape UF").FontColor("#ffff00");
                                    }
                                }
                                else if (boss.KcPlayers.Count == 2)
                                {
                                    table.Cell().Text($"").FontSize(12).FontFamily("RuneScape UF");

                                    foreach (var player in boss.KcPlayers)
                                    {
                                        table.Cell().Text($"{player.Player} - {player.Kc}").FontSize(18).FontFamily("RuneScape UF").FontColor("#ffff00");
                                    }

                                    table.Cell().Text($"").FontSize(12).FontFamily("RuneScape UF");
                                }
                            }
                        });
                    });
                });
            });

            document.GeneratePdf("ADefaultPDF.pdf");
            document.GenerateImages(i => "ADefaultPng.png");

            await Context.Channel.SendFileAsync("ADefaultPng.png");
        }

    }
}