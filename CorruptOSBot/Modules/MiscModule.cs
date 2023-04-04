using CorruptOSBot.Extensions;
using CorruptOSBot.Helpers.Bot;
using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared;
using CorruptOSBot.Shared.Helpers.Bot;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static QuestPDF.Helpers.Colors;

namespace CorruptOSBot.Modules
{
    public class MiscModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("validate-roll")]
        [Summary("!roll - Rolls a random number")]
        public async Task ValidateRollAsync(int sides, int total)
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var amountOfOnes = 0;
                var amountOfTwos = 0;
                var amountOfThrees = 0;
                var random = new Random();


                for (var i = 0; i < total; i++)
                {
                    int randomInt = random.Next(1, 4);

                    if(randomInt == 1)
                    {
                        amountOfOnes++;
                    }
                    else if (randomInt == 2)
                    {
                        amountOfTwos++;
                    }
                    else
                    {
                        amountOfThrees++;
                    }
                }

                var percentOfOnes = (int)(0.5f + ((100f * amountOfOnes) / total));
                var percentOfTwos = (int)(0.5f + ((100f * amountOfTwos) / total));
                var percentOfThrees = (int)(0.5f + ((100f * amountOfThrees) / total));

                var results = $"Out of {total} rolls. There were {amountOfOnes} Ones Rolled ({percentOfOnes}). {amountOfTwos} Twos Rolled ({percentOfTwos}). {amountOfThrees} Threes Rolled ({percentOfThrees})";

                await Context.Channel.SendMessageAsync($"{results}");
            }
        }


        [Helpgroup(HelpGroup.Member)]
        [Command("roll")]
        [Summary("!roll - Rolls a random number")]
        public async Task RollNumberAsync()
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var random = new Random();

                int randomInt = random.Next(1, 5);

                await Context.Channel.SendMessageAsync($"{DiscordNameHelper.GetAccountNameOrNickname(Context.User)} rolled a {randomInt}! (1 - 4)");
            }
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("roll6")]
        [Summary("!roll6 - Rolls a random number between 1-6")]
        public async Task RollNumber6Async()
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var random = new Random();

                int randomInt = random.Next(1, 7);

                await Context.Channel.SendMessageAsync($"{DiscordNameHelper.GetAccountNameOrNickname(Context.User)} rolled a {randomInt}! (1 - 6)");
            }
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("roll-odd")]
        [Summary("!roll-odd - Rolls a random odd number")]
        public async Task RollOddNumberAsync()
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var random = new Random();
                int randomInt = 0;
                int ans = random.Next(1, 4);

                if (ans % 2 == 1)
                {
                    randomInt = ans;
                }
                else
                {
                    if (ans + 1 <= 4)
                        randomInt = ans + 1;
                    else if (ans - 1 >= 1)
                        randomInt = ans - 1;
                }

                await Context.Channel.SendMessageAsync($"{DiscordNameHelper.GetAccountNameOrNickname(Context.User)} rolled a {randomInt}! (1 - 4)");
            }
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("roll-even")]
        [Summary("!roll-even - Rolls a random odd number")]
        public async Task RollEvenNumberAsync()
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var random = new Random();
                int randomInt = random.Next(1, 4 / 2) * 2 + 1;
                int ans = random.Next(1, 4);

                if (ans % 2 == 0)
                {
                    randomInt = ans;
                }
                else
                {
                    if (ans + 1 <= 4)
                        randomInt = ans + 1;
                    else if (ans - 1 >= 1)
                        randomInt = ans - 1;
                }

                await Context.Channel.SendMessageAsync($"{DiscordNameHelper.GetAccountNameOrNickname(Context.User)} rolled a {randomInt}! (1 - 4)");
            }
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("purple-spin")]
        [Summary("!purple-spin - Randomly picks an option from a predefined list")]
        public async Task PurpleSpinAsync()
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var wheelOptionsList = new List<string>()
                {
                    "Select a team who will only be able to roll an even number on their next turn",
                    "Additional Reroll for your team",
                    "Select a team who will only be able to roll an odd number on their next turn",
                    "Next turn you can use a 6 sided dice",
                    "Pick a team who will be forced to reroll their name turn",
                };

                var random = new Random();

                int randomInt = random.Next(0, wheelOptionsList.Count-1);

                await Context.Channel.SendMessageAsync($"Congratulations! \n{wheelOptionsList[randomInt]}");
            }
        }
    }
}


