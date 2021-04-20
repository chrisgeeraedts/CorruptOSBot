using System.Collections.Generic;

namespace CorruptOSBot.Extensions
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class RemoveMemberRoot
    {
        public string verificationCode { get; set; }
        public List<string> members { get; set; }
    }
}
