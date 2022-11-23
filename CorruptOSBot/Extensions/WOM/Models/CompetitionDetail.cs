using System;
using System.Collections.Generic;

namespace CorruptOSBot.Extensions
{
    public class CompetitionDetail
    {
        public int id { get; set; }
        public string title { get; set; }
        public string metric { get; set; }
        public string type { get; set; }
        public double score { get; set; }
        public DateTime? startsAt { get; set; }
        public DateTime? endsAt { get; set; }
        public object groupId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public object group { get; set; }
        public string duration { get; set; }
        public List<Participant> participations { get; set; }
    }
}
