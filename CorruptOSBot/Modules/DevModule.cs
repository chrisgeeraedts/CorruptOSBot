using CorruptOSBot.Data;
using CorruptOSBot.Extensions;
using CorruptOSBot.Extensions.WOM;
using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Helpers.PVM;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord.Commands;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
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
        [Command("dev", false)]
        [Summary("!dev - Dev command")]
        public async Task Dev()
        {
            await Context.Channel.SendMessageAsync(embed: await EmbedHelper.CreateFullLeaderboardEmbed(30));

            await Context.Message.DeleteAsync();
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

                Image img = new Bitmap(1, 1);
                Graphics drawing = Graphics.FromImage(img);

                SizeF textSize = drawing.MeasureString(text, font);

                //free up the dummy image and old graphics object
                img.Dispose();
                drawing.Dispose();

                //create a new image of the right size
                img = new Bitmap((int)textSize.Width, (int)textSize.Height);

                drawing = Graphics.FromImage(img);

                //paint the background
                drawing.Clear(Color.Green);

                //create a brush for the text
                Brush textBrush = new SolidBrush(Color.Black);

                drawing.DrawString(text, font, textBrush, 0, 0);

                drawing.Save();

                textBrush.Dispose();
                drawing.Dispose();

                img.Save("TempImage.png", ImageFormat.Png);
                await Context.Channel.SendFileAsync("TempImage.png", "This is a test");
                File.Delete("TempImage.png");
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