using System;

namespace CorruptOSBot.Extensions
{
    public class Competition
    {
        public int id { get; set; }
        public string title { get; set; }
        public string metric { get; set; }
        public string type { get; set; }
        public int score { get; set; }
        public DateTime startsAt { get; set; }
        public DateTime endsAt { get; set; }
        public object groupId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int participantCount { get; set; }
    }
}
