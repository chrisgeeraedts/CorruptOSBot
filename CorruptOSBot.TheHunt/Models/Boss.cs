using System.Collections.Generic;

namespace CorruptOSBot.TheHunt
{
    public class Boss
    {
        public Boss(BossEnum boss, int allUniquesValue)
        {
            _boss = boss;
            _allUniquesValue = allUniquesValue;
        }
        public BossEnum BossEnum { get { return _boss; } }
        private BossEnum _boss { get; set; }
        public Dictionary<string, int> PointValues { get; set; }
        public int AllUniquesValue { get { return _allUniquesValue; }  }
        private int _allUniquesValue { get; set; }
    }
}
