using System;
using System.Collections.Generic;

namespace CorruptOSBot.Extensions
{
    public class Participant
    {
        public int playerId { get; set; }
        public int competitionId { get; set; }
        public string teamName { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public Player player { get; set; }
        public Progress progress { get; set; }
        public List<History> history { get; set; }
    }
}
