namespace CorruptOSBot.Extensions
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Progress
    {
        public double start { get; set; }
        public double end { get; set; }
        public double gained { get; set; }
    }
}
