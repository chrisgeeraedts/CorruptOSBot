using System.Collections.Generic;

namespace CorruptOSBot.Extensions
{
    public class TeamCompParticipation
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Team
        {
            public string name { get; set; }
            public List<string> participants { get; set; }
        }

        public class Root
        {
            public string verificationCode { get; set; }
            public List<Team> teams { get; set; }
        }
    }
}
