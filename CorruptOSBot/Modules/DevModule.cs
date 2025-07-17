using CorruptOSBot.Helpers;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared.Helpers.Bot;
using Discord;
using Discord.Commands;
using GenerativeAI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            var embedMessageList = await EmbedHelper.CreateFullLeaderboardEmbed(30);


            foreach (var embedMessage in embedMessageList)
            {
                await Context.Channel.SendMessageAsync(embed: embedMessage);
            }

            await Context.Message.DeleteAsync();
        }


        [Helpgroup(HelpGroup.Admin)]
        [Command("NewPhrase", false)]
        [Summary("!NewPhrase - Generates a new phrase for someone to use")]
        public async Task RandomEventPhrase()
        {
            var googleAI = new GoogleAi("AIzaSyA4JiOL5T2jVvoFoILBZDJZHzh7Of253zM");
            var model = googleAI.CreateGenerativeModel("gemini-2.0-flash");

            var newPhrase = (await model.GenerateContentAsync($"Give me two random five letter words. No other text in your message")).Text.ToUpper();
            var font = new Font("Arial", 12);

            System.Drawing.Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            SizeF textSize = drawing.MeasureString(newPhrase, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap(100, 100);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(System.Drawing.Color.White);

            //create a brush for the text
            Brush textBrush = new SolidBrush(System.Drawing.Color.Black);

            drawing.DrawString(newPhrase, font, textBrush, 25, 25);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            img.Save("TempImage.png", System.Drawing.Imaging.ImageFormat.Png);
            await Context.Channel.SendFileAsync("TempImage.png", $"New Phrase for <@{Context.User.Id}> {Environment.NewLine}{newPhrase}");
            File.Delete("TempImage.png");

            await Context.Message.DeleteAsync();
        }

        //private async Task OnHold()
        //{
        //    var bossKCs = await BossKCHelper.GetTopBossKC();

        //    var document = Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.ContinuousSize(PageSizes.A1.Width);
        //            page.Margin(1, Unit.Centimetre);
        //            page.PageColor(Colors.Black);
        //            page.DefaultTextStyle(x => x.FontSize(20));
        //            //page.Background().Extend().Image("ABackground.png");

        //            page.Header()
        //                .AlignCenter()
        //                .Text("Top Boss KC")
        //                .SemiBold()
        //                .FontFamily("RuneScape UF")
        //                .FontSize(36)
        //                .FontColor("#ffff00");

        //            page.Content().Column(column =>
        //            {
        //                column.Spacing(20);

        //                column.Item().Table(table =>
        //                {
        //                    table.ColumnsDefinition(columns =>
        //                    {
        //                        columns.RelativeColumn();
        //                        columns.RelativeColumn();
        //                        columns.RelativeColumn();
        //                        columns.RelativeColumn();
        //                    });

        //                    foreach (var boss in bossKCs)
        //                    {
        //                        if (boss.KcPlayers.Count == 3)
        //                        {
        //                            table.Cell().Text($"").FontSize(12).FontFamily("RuneScape UF");

        //                            foreach (var player in boss.KcPlayers)
        //                            {
        //                                table.Cell().Text($"{player.Player} - {player.Kc}").FontSize(18).FontFamily("RuneScape UF").FontColor("#ffff00");
        //                            }
        //                        }
        //                        else if (boss.KcPlayers.Count == 2)
        //                        {
        //                            table.Cell().Text($"").FontSize(12).FontFamily("RuneScape UF");

        //                            foreach (var player in boss.KcPlayers)
        //                            {
        //                                table.Cell().Text($"{player.Player} - {player.Kc}").FontSize(18).FontFamily("RuneScape UF").FontColor("#ffff00");
        //                            }

        //                            table.Cell().Text($"").FontSize(12).FontFamily("RuneScape UF");
        //                        }
        //                    }
        //                });
        //            });
        //        });
        //    });

        //    document.GeneratePdf("ADefaultPDF.pdf");
        //    document.GenerateImages(i => "ADefaultPng.png");

        //    await Context.Channel.SendFileAsync("ADefaultPng.png");
        //}

        [Helpgroup(HelpGroup.Admin)]
        [Command("Question", false)]
        [Summary("!Question - Ask the Bot a question about the event")]
        public async Task EventQuestion([Remainder] string question)
        {
            if (Context.Channel.Name == "staff-general")
            {
                var googleAI = new GoogleAi("AIzaSyA4JiOL5T2jVvoFoILBZDJZHzh7Of253zM");
                var model = googleAI.CreateGenerativeModel("gemini-2.0-flash");

                var eventDetails = File.ReadAllText("EventDetails.txt");

                try
                {
                    var answer = await model.GenerateContentAsync($"You are hosting a group game/event and need to answer user questions All following questions are related to this." +
                        $" DO NOT answer out of scope questions and tell the user you are unable to answer out of scope questions. if a user asks a question you do not have the answer to tell them to contact a staff member." +
                        $" Here is the question {question}." +
                        $" Here are the details: {eventDetails}." +
                        $"{Environment.NewLine}-# This may not be true I'm just a little AI");

                    await Context.Channel.SendMessageAsync(answer.Text);
                }
                catch (Exception ex)
                {
                    await Context.Channel.SendMessageAsync($"An error occurred while processing your request: {ex.Message}");
                    return;
                }
            }
        }
    }
}