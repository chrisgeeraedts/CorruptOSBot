using CorruptOSBot.TheHunt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            HuntManager.Init();

            HuntManager.AddTeam("Corrupt Heroes", "Team 1");
            var team = HuntManager.Teams[0];

            HuntManager.AddDrop(team, TheHunt.BossEnum.Armadyl, "Armadyl Chestplate", new byte[8]);
            HuntManager.AddDrop(team, TheHunt.BossEnum.Armadyl, "Armadyl Chestplate", new byte[8]);
            HuntManager.AddDrop(team, TheHunt.BossEnum.Armadyl, "Armadyl Chestplate", new byte[8]);

            var points = team.CalculatePointsForTeam();
        }
    }
}
