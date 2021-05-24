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
            using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
            {
                var bosses = corruptosEntities.hunt_bosses.ToList();
                var items = corruptosEntities.hunt_bossdrops.ToList();
            }
        }
    }

    class HuntBoss
    {

    }
}
