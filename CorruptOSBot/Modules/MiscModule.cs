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

        #region Maze One

        [Helpgroup(HelpGroup.Member)]
        [Command("lumbridge")]
        [Summary("!lumbridge")]
        public async Task MazeEventLumbridge()
        {
            await SendMazeRunnerImage("Lumbridge Maze", "https://imgur.com/a/lumbridge-egeiVrJ");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("draynor")]
        [Summary("!draynor")]
        public async Task MazeEventDraynor()
        {
            await SendMazeRunnerImage("Draynor Maze", "https://imgur.com/a/draynor-UwM3W6Z");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("swamp")]
        [Summary("!swamp")]
        public async Task MazeEventSwamp()
        {
            await SendMazeRunnerImage("Swamp Maze", "https://imgur.com/a/swamp-MyPGXom");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("alkharid")]
        [Summary("!alkharid")]
        public async Task MazeEventAlKharid()
        {
            await SendMazeRunnerImage("Al Kharid Maze", "https://imgur.com/a/alkharid-U58FwSP");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("gnome")]
        [Summary("!gnome")]
        public async Task MazeEventGnome()
        {
            await SendMazeRunnerImage("Gnome Maze", "https://imgur.com/a/gnome-xEcdJzn");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("glider")]
        [Summary("!glider")]
        public async Task MazeEventGlider()
        {
            await SendMazeRunnerImage("Glider Maze", "https://imgur.com/a/glider-RLFq2Wt");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("mountain")]
        [Summary("!mountain")]
        public async Task MazeEventMountain()
        {
            await SendMazeRunnerImage("Mountain Maze", "https://imgur.com/a/mountain-bEMpppa");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("stronghold")]
        [Summary("!stronghold")]
        public async Task MazeEventStronghold()
        {
            await SendMazeRunnerImage("Stronghold Maze", "https://imgur.com/a/stronghold-KSZz2vP");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("tree")]
        [Summary("!tree")]
        public async Task MazeEventTree()
        {
            await SendMazeRunnerImage("Tree Maze", "https://imgur.com/a/tree-oMcrw9w");
        }

        #endregion Maze One

        #region Maze Two

        [Helpgroup(HelpGroup.Member)]
        [Command("varrock")]
        [Summary("!varrock")]
        public async Task MazeEventVarrock()
        {
            await SendMazeRunnerImage("Varrock Maze", "https://imgur.com/a/varrock-fmh8ArC");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("eastbank")]
        [Summary("!eastbank")]
        public async Task MazeEventEastBank()
        {
            await SendMazeRunnerImage("East Bank Maze", "https://imgur.com/a/eastbank-sOJl4yx");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("westbank")]
        [Summary("!westbank")]
        public async Task MazeEventWestBank()
        {
            await SendMazeRunnerImage("West Bank Maze", "https://imgur.com/a/westbank-H0AqILI");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("grandexchange")]
        [Summary("!grandexchange")]
        public async Task MazeEventGrandExchange()
        {
            await SendMazeRunnerImage("Grand Exchange Maze", "https://imgur.com/a/grandexchange-fggfxk0");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("wildy")]
        [Summary("!wildy")]
        public async Task MazeEventWildy()
        {
            await SendMazeRunnerImage("Wildy Maze", "https://imgur.com/a/wildy-UCYPMZj");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("palace")]
        [Summary("!palace")]
        public async Task MazeEventPalace()
        {
            await SendMazeRunnerImage("Palace Maze", "https://imgur.com/a/palace-Vtd8Hok");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("ditch")]
        [Summary("!ditch")]
        public async Task MazeEventDitch()
        {
            await SendMazeRunnerImage("Ditch Maze", "https://imgur.com/a/ditch-OZk3pWK");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("pub")]
        [Summary("!pub")]
        public async Task MazeEventPub()
        {
            await SendMazeRunnerImage("Pub Maze", " https://imgur.com/a/pub-Duv2z7C");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("quetzal")]
        [Summary("!quetzal")]
        public async Task MazeEventQuetzal()
        {
            await SendMazeRunnerImage("Quetzal Maze", "https://imgur.com/a/quetzal-5N8C4pf");
        }

        #endregion Maze Two

        #region Maze Three

        [Helpgroup(HelpGroup.Member)]
        [Command("lovakengj")]
        [Summary("!lovakengj")]
        public async Task MazeEventLovakengj()
        {
            await SendMazeRunnerImage("Lovakengj Maze", "https://imgur.com/a/lovakengj-qisQriB");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("shazien")]
        [Summary("!shazien")]
        public async Task MazeEventShazien()
        {
            await SendMazeRunnerImage("Shazien Maze", "https://imgur.com/a/PRLKMw6");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("arceuus")]
        [Summary("!arceuus")]
        public async Task MazeEventArcuus()
        {
            await SendMazeRunnerImage("Arceuus Maze", " https://imgur.com/a/arceuus-JoCGG3l");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("piscarilius")]
        [Summary("!piscarilius")]
        public async Task MazeEventPiscarilius()
        {
            await SendMazeRunnerImage("Piscarilius Maze", "https://imgur.com/a/piscarilius-gAgUKqV");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("hosidius")]
        [Summary("!hosidius")]
        public async Task MazeEventHosidius()
        {
            await SendMazeRunnerImage("Hosidius Maze", "https://imgur.com/a/hosidius-rpqoAYT");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("fairyring")]
        [Summary("!fairyring")]
        public async Task MazeEventFairyRing()
        {
            await SendMazeRunnerImage("Fairy Ring Maze", "https://imgur.com/a/fairyring-pQZy1A2");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("karambwan")]
        [Summary("!karambwan")]
        public async Task MazeEventKarambwan()
        {
            await SendMazeRunnerImage("Karambwan Maze", "https://imgur.com/a/karambwan-3GiKJAb");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("zoo")]
        [Summary("!zoo")]
        public async Task MazeEventZoo()
        {
            await SendMazeRunnerImage("Zoo Maze", "https://imgur.com/a/zoo-2tDr2d8");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("westardougne")]
        [Summary("!westardougne")]
        public async Task MazeEventWestArdougne()
        {
            await SendMazeRunnerImage("West Ardougne Maze", "https://imgur.com/a/westardougne-bjOXCcu");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("undergroundpass")]
        [Summary("!undergroundpass")]
        public async Task MazeEventUndergroundPass()
        {
            await SendMazeRunnerImage("Underground Pass Maze", "https://imgur.com/a/undergroundpass-Gqhe7Rn");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("priff")]
        [Summary("!priff")]
        public async Task MazeEventPriff()
        {
            await SendMazeRunnerImage("Priff Maze", "https://imgur.com/a/priff-3CafpN9");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("tyras")]
        [Summary("!tyras")]
        public async Task MazeEventTyras()
        {
            await SendMazeRunnerImage("Tyras Maze", "https://imgur.com/a/tyras-EwiDr6L");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("isafdar")]
        [Summary("!isafdar")]
        public async Task MazeEventIsafdar()
        {
            await SendMazeRunnerImage("Isafdar Maze", "https://imgur.com/a/isafdar-FPFSObq");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("lleyta")]
        [Summary("!lleyta")]
        public async Task MazeEventLleyta()
        {
            await SendMazeRunnerImage("Lleyta Maze", "https://imgur.com/a/lleyta-eZ9xeJT");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("ship")]
        [Summary("!ship")]
        public async Task MazeEventShip()
        {
            await SendMazeRunnerImage("Ship Maze", "https://imgur.com/a/KsKmu0j");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("catherby")]
        [Summary("!catherby")]
        public async Task MazeEvent()
        {
            await SendMazeRunnerImage("Catherby Maze", "https://imgur.com/a/catherby-UcjqpkS");
        }

        #endregion Maze Three

        #region Maze Challenges

        [Helpgroup(HelpGroup.Member)]
        [Command("clue")]
        [Summary("!clue")]
        public async Task MazeEventClue()
        {
            await SendMazeRunnerImage("Clue Challenge", "https://imgur.com/a/clue-dX2IzSM");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("clue002")]
        [Summary("!clue002")]
        public async Task MazeEventClue002()
        {
            await SendMazeRunnerImage("Clue002 Challenge", " https://imgur.com/a/clue002-f0n6ueX");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("cluetres")]
        [Summary("!cluetres")]
        public async Task MazeEventClueTres()
        {
            await SendMazeRunnerImage("ClueTres Challenge - There are 3 images", "https://imgur.com/a/cluetres-H1PcRQ6");
        }

        [Helpgroup(HelpGroup.Member)]
        [Command("boss")]
        [Summary("!boss")]
        public async Task MazeEventBoss()
        {
            await SendMazeRunnerImage("Boss Challenge", " https://imgur.com/a/boss-YTlqdEC");
        }

        #endregion

        private async Task SendMazeRunnerImage(string message, string imageUrl)
        {
            if (Context.Message.Author.Id == 108710294049542144 || Context.Message.Author.Id == 412813330458083356 ||
                Context.Message.Channel.Id == 1278431728121417788 || 
                Context.Message.Channel.Id == 1278431878189158451 ||
                Context.Message.Channel.Id == 1278431900196929597)
            {
                await Context.Channel.SendMessageAsync(message);
                await Context.Channel.SendMessageAsync(imageUrl);
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