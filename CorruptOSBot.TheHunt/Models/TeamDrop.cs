using System;

namespace CorruptOSBot.TheHunt
{
    public class TeamDrop
    {
        public Guid Id { get; set; }
        public string Item { get; set; }
        public byte[] Image { get; set; }
        public DateTime DropDate { get; set; }
        public int DropValue { get; set; }
    }
}
