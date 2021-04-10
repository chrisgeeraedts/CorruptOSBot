namespace CorruptOSBot.Extensions
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class NameChangeRoot
    {
        public string oldName { get; set; }
        public string newName { get; set; }
    }
}
