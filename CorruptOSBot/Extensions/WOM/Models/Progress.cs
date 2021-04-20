namespace CorruptOSBot.Extensions
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Progress
    {
        public int start { get; set; }
        public int end { get; set; }
        public int gained { get; set; }
    }
}
