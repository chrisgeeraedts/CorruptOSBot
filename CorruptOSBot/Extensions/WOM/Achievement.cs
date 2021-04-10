using System;

namespace CorruptOSBot.Extensions
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Achievement
    {
        public int threshold { get; set; }
        public int playerId { get; set; }
        public string name { get; set; }
        public string metric { get; set; }
        public DateTime createdAt { get; set; }
        public string measure { get; set; }
    }
}
