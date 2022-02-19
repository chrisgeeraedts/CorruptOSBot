namespace CorruptOSBot.Extensions.WOM.ClanMemberDetails
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Overall
    {
        public int rank { get; set; }
        public int experience { get; set; }
        public double ehp { get; set; }
    }
}