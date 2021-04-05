namespace CorruptOSBot.Services
{
    public interface IService
    {
        void Trigger(Discord.IDiscordClient client);
        int TriggerTimeInMS { get; }
    }
}
