using System.Collections.Generic;

namespace CorruptOSBot.TheHunt
{
    public class Team
    {
        public Team()
        {
            Bosses = new List<TeamBoss>();
        }

        public string TeamRole { get; set; }
        public string TeamName { get; set; }
        public List<TeamBoss> Bosses { get; set; }

        public int CalculatePointsForTeam()
        {
            var result = 0;
            foreach (var boss in Bosses)
            {
                result += boss.CalculatePointsForBoss();
            }
            return result;
        }
    }
}
