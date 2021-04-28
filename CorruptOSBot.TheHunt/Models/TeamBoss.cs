using System.Collections.Generic;

namespace CorruptOSBot.TheHunt
{
    public class TeamBoss
    {
        public TeamBoss()
        {
            TeamDrops = new List<TeamDrop>();
        }
        public BossEnum Boss { get; set; }
        public List<TeamDrop> TeamDrops { get; set; }

        public int CalculatePointsForBoss()
        {
            var result = 0;
            foreach (var drop in TeamDrops)
            {
                result += drop.DropValue;
            }
            return result;
        }

    }
}
