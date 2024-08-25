using CorruptOSBot.Helpers.Discord;
using CorruptOSBot.Shared.Helpers.Bot;
using CorruptOSBot.Shared.Helpers.Discord;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class MiscModule : ModuleBase<SocketCommandContext>
    {
        [Helpgroup(HelpGroup.Member)]
        [Command("validate-roll-reward")]
        [Summary("!roll - Rolls a random number")]
        public async Task ValidateRollRewardAsync(int total)
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var random = new Random();
                
                var amountOfOnes = 0;
                var amountOfTwos = 0;
                var amountOfThrees = 0;
                var amountOfFours = 0;
                var amountOfFives = 0;
                var amountOfSixes = 0;
                var amountOfSevens = 0;
                var amountOfEights = 0;

                var reward = string.Empty;

                WeightedChanceExecutor weightedChanceExecutor = new WeightedChanceExecutor(
                    new WeightedChanceParam(() => { reward = "1"; }, 20),
                    new WeightedChanceParam(() => { reward = "2"; }, 20),
                    new WeightedChanceParam(() => { reward = "3"; }, 10),
                    new WeightedChanceParam(() => { reward = "4"; }, 10),
                    new WeightedChanceParam(() => { reward = "5"; }, 10),
                    new WeightedChanceParam(() => { reward = "6"; }, 10),
                    new WeightedChanceParam(() => { reward = "7"; }, 10),
                    new WeightedChanceParam(() => { reward = "8"; }, 10));

                for (var i = 0; i < total; i++)
                {
                    weightedChanceExecutor.Execute();

                    if (reward == "1")
                    {
                        amountOfOnes++;
                    }
                    else if (reward == "2")
                    {
                        amountOfTwos++;
                    }
                    else if (reward == "3")
                    {
                        amountOfThrees++;
                    }
                    else if (reward == "4")
                    {
                        amountOfFours++;
                    }
                    else if (reward == "5")
                    {
                        amountOfFives++;
                    }
                    else if (reward == "6")
                    {
                        amountOfSixes++;
                    }
                    else if (reward == "7")
                    {
                        amountOfSevens++;
                    }
                    else
                    {
                        amountOfEights++;
                    }
                }

                var percentOfOnes = (int)(0.5f + ((100f * amountOfOnes) / total));
                var percentOfTwos = (int)(0.5f + ((100f * amountOfTwos) / total));
                var percentOfThrees = (int)(0.5f + ((100f * amountOfThrees) / total));
                var percentOfFours = (int)(0.5f + ((100f * amountOfFours) / total));
                var percentOfFives = (int)(0.5f + ((100f * amountOfFives) / total));
                var percentOfSixes = (int)(0.5f + ((100f * amountOfSixes) / total));
                var percentOfSevens = (int)(0.5f + ((100f * amountOfSevens) / total));
                var percentOfEights = (int)(0.5f + ((100f * amountOfEights) / total));

                var results = $"Out of {total} rolls. " +
                    $"There were {amountOfOnes} Ones Rolled ({percentOfOnes}). " +
                    $"{amountOfTwos} Twos Rolled ({percentOfTwos}). " +
                    $"{amountOfThrees} Threes Rolled ({percentOfThrees}). " +
                    $"{amountOfFours} Fours Rolled ({percentOfFours}). " +
                    $"{amountOfFives} Fives Rolled ({percentOfFives}). " +
                    $"{amountOfSixes} Sixes Rolled ({percentOfSixes}). " +
                    $"{amountOfSevens} Sevens Rolled ({percentOfSevens}). " +
                    $"{amountOfEights} Eights Rolled ({percentOfEights})";

                await Context.Channel.SendMessageAsync($"{results}");
            }
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("validate-roll")]
        [Summary("!roll - Rolls a random number")]
        public async Task ValidateRollAsync(int total)
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var amountOfOnes = 0;
                var amountOfTwos = 0;
                var amountOfThrees = 0;
                var amountOfFours = 0;
                
                var random = new Random();

                for (var i = 0; i < total; i++)
                {
                    int randomInt = random.Next(1, 5);

                    if (randomInt == 1)
                    {
                        amountOfOnes++;
                    }
                    else if (randomInt == 2)
                    {
                        amountOfTwos++;
                    }
                    else if (randomInt == 3)
                    {
                        amountOfThrees++;
                    }
                    else
                    {
                        amountOfFours++;
                    }
                }

                var percentOfOnes = (int)(0.5f + ((100f * amountOfOnes) / total));
                var percentOfTwos = (int)(0.5f + ((100f * amountOfTwos) / total));
                var percentOfThrees = (int)(0.5f + ((100f * amountOfThrees) / total));
                var percentOfFours = (int)(0.5f + ((100f * amountOfFours) / total));

                var results = $"Out of {total} rolls. There were {amountOfOnes} Ones Rolled ({percentOfOnes}). {amountOfTwos} Twos Rolled ({percentOfTwos}). {amountOfThrees} Threes Rolled ({percentOfThrees}). {amountOfFours} Fours Rolled ({percentOfFours})";

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

                await Context.Channel.SendMessageAsync($"{DiscordNameHelper.GetAccountNameOrNickname(Context.User)} rolled a {randomInt}! (1 / 3)");
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

                await Context.Channel.SendMessageAsync($"{DiscordNameHelper.GetAccountNameOrNickname(Context.User)} rolled a {randomInt}! (2 / 4)");
            }
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("roll-reward")]
        [Summary("!roll-reward - Randomly picks an option from a predefined list")]
        public async Task PurpleSpinAsync()
        {
            if (RoleHelper.HasAnyRole(Context.User))
            {
                var reward = string.Empty;

                WeightedChanceExecutor weightedChanceExecutor = new WeightedChanceExecutor(
                    new WeightedChanceParam(() => { reward = "Odd Dice Role"; }, 20),
                    new WeightedChanceParam(() => { reward = "Even Dice Role"; }, 20),
                    new WeightedChanceParam(() => { reward = "6 Sided Dice Role"; }, 10),
                    new WeightedChanceParam(() => { reward = "Custom Dice Role"; }, 10),
                    new WeightedChanceParam(() => { reward = "Freeze"; }, 10),
                    new WeightedChanceParam(() => { reward = "Reroll"; }, 10),
                    new WeightedChanceParam(() => { reward = "Steal"; }, 10),
                    new WeightedChanceParam(() => { reward = "Block"; }, 10));

                weightedChanceExecutor.Execute();

                await Context.Channel.SendMessageAsync($"Congratulations! \n{reward}");
            }
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("create-vc")]
        [Summary("!create-vc - Creates a voice channel for with a limited amount of users allowed in the VC")]
        public async Task CreateVCAsync(int maxUsers)
        {
            if (DiscordHelper.IsInChannel(Context.Channel.Id, "spam-bot-commands", Context.User))
            {
                var category = Context.Guild.CategoryChannels.FirstOrDefault(item => item.Name == "Voice Channels");

                var userPerms = new List<Overwrite>
                {
                    new Overwrite(Context.User.Id, PermissionTarget.User, new OverwritePermissions(manageChannel: PermValue.Allow))
                };

                var privateVoiceChannel = await Context.Guild.CreateVoiceChannelAsync($"Private {Context.User.Username}", prop =>
                {
                    prop.CategoryId = category.Id;
                    prop.UserLimit = maxUsers;
                    prop.PermissionOverwrites = userPerms;
                });

                await Context.User.SendMessageAsync($"You have created {privateVoiceChannel.Name}. If you wish to change settings for this channel please right click the channel > Edit Channel.");
            }
            else
            {
                await DiscordHelper.NotAllowedMessageToUser(Context, "!create-vc", "spam-bot-commands");
            }

            await Context.Message.DeleteAsync();
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("lumbridge")]
        [Summary("!lumbridge")]
        public async Task MazeEventLumbridge()
        {
            if (Context.Message.Author.Id == 108710294049542144 || Context.Message.Id == 412813330458083356)
            {
                await Context.User.SendMessageAsync($"Lumbridge Maze Test");
                await Context.User.SendMessageAsync($"https://i.imgur.com/lEIRhSZ.png");
            }
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("varrock")]
        [Summary("!varrock")]
        public async Task MazeEventVarrock()
        {
            if (Context.Message.Author.Id == 108710294049542144 || Context.Message.Id == 412813330458083356)
            {
                await Context.User.SendMessageAsync($"Lumbridge Maze Test");
                await Context.User.SendMessageAsync($"https://i.imgur.com/lEIRhSZ.png");
            }
        }
    }

    public class WeightedChanceParam
    {
        public Action Func { get; }
        public double Ratio { get; }

        public WeightedChanceParam(Action func, double ratio)
        {
            Func = func;
            Ratio = ratio;
        }
    }

    public class WeightedChanceExecutor
    {
        public WeightedChanceParam[] Parameters { get; }
        private Random r;

        public double RatioSum
        {
            get { return Parameters.Sum(p => p.Ratio); }
        }

        public WeightedChanceExecutor(params WeightedChanceParam[] parameters)
        {
            Parameters = parameters;
            r = new Random();
        }

        public void Execute()
        {
            double numericValue = r.NextDouble() * RatioSum;

            foreach (var parameter in Parameters)
            {
                numericValue -= parameter.Ratio;

                if (!(numericValue <= 0))
                    continue;

                parameter.Func();
                return;
            }

        }
    }
}