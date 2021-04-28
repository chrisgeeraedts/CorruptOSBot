using CorruptOSBot.Shared;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CorruptOSBot.TheHunt
{
    public static class HuntManager
    {
        public static List<Team> Teams { get; set; }
        public static List<Boss> Bosses { get; set; }
        public static void Init()
        {
            Teams = new List<Team>();

            Bosses = new List<Boss>();
            Bosses.Add(new Boss(BossEnum.Armadyl, 295)
            {
                PointValues = new Dictionary<string, int>()
                {
                    { "Armadyl Chestplate", 90 },
                    { "Armadyl Chainskirt", 90 },
                    { "Armadyl Helmet", 90 },
                    { "Armadyl Hilt", 120 },
                    { "Any Shard", 60 },
                }
            });

            Bosses.Add(new Boss(BossEnum.Bandos, 245)
            {
                PointValues = new Dictionary<string, int>()
                {
                    { "Bandos Chestplate", 75 },
                    { "Bandos Tassets", 75 },
                    { "Bandos Boots", 75 },
                    { "Bandos Hilt", 100 },
                    { "Any Shard", 50 },
                }
            });

            Bosses.Add(new Boss(BossEnum.Vorkath, 590)
            {
                PointValues = new Dictionary<string, int>()
                {
                    { "Vorkath Head", 25 },
                    { "Dragonbone Necklace", 220 },
                    { "Skeletal Visage", 470 },
                    { "Dragonic Visage", 470 }
                }
            });

            LoadData();
        }

        public static void AddTeam(string teamName, string teamRole)
        {
            Teams.Add(new Team() { TeamName = teamName, TeamRole = teamRole });
        }

        public static int CalculatePointsForItem(Team team, BossEnum boss, string item)
        {
            var result = 0;
            var realBoss = Bosses.FirstOrDefault(x => x.BossEnum == boss);
            var realItem = realBoss.PointValues.FirstOrDefault(x => x.Key == item);

            var teamBoss = team.Bosses.FirstOrDefault(x => x.Boss == boss);
            int itemDuplicates = 0;

            if (teamBoss != null)
            {
                itemDuplicates = teamBoss.TeamDrops.Count(x => x.Item == item);
            }

            if (itemDuplicates == 0)
            {
                result = realItem.Value;
            }
            else if (itemDuplicates > 0)
            {
                result = realItem.Value / 2;
            }

            return result;
        }

        internal static byte[] GetImageData(Attachment attachment)
        {
            if (attachment != null)
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(attachment.Url);
                return imageBytes;
            }
            return new byte[0];
            
        }

        internal static string DetermineItem(string content, BossEnum boss)
        {
            var bossFound = Bosses.FirstOrDefault(x => x.BossEnum == boss);
            if (bossFound != null && bossFound.PointValues.Any(x => x.Key.ToLower() == content.ToLower()))
            {
                return content;
            }

            return string.Empty;
        }

        internal static BossEnum DetermineBoss(string content)
        {
            // find boss from item? Iterate through all bosses and find the item or force some string format like {boss}-{item}

            return Bosses.FirstOrDefault(x => x.BossEnum.ToString().ToLower() == content.ToLower()).BossEnum;
        }

        internal static Team GetTeam(SocketUser author)
        {
            // grab team by role or something
            var roles = ((SocketGuildUser)author).Roles;
            foreach (var role in roles)
            {
                var team = Teams.FirstOrDefault(x => x.TeamRole == role.Name);
                if (team != null)
                {
                    return team;
                }
                
            }
            return null;
        }

        public static void AddDrop(Team team, BossEnum boss, string item, byte[] image)
        {
            // check if we have that boss already
            var bossValue = team.Bosses.FirstOrDefault(x => x.Boss == boss);
            if (bossValue == null)
            {
                team.Bosses.Add(new TeamBoss() { Boss = boss });
            }

            // retrieve from list
            bossValue = team.Bosses.FirstOrDefault(x => x.Boss == boss);
            bossValue.TeamDrops.Add(new TeamDrop() {
                DropDate = DateTime.Now,
                Item = item,
                Image = image,
                DropValue = CalculatePointsForItem(team, boss, item),
                Id = Guid.NewGuid()
            });

            // Update the datastore
        }


        public static void LoadData()
        {
            AddTeam("team1-post", "Team 1");
        }

        public static void SaveData()
        {

        }
    }
}
