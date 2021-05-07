using System.Threading.Tasks;

namespace CorruptOSBot.Services
{
    public interface IService
    {
        Task Trigger(Discord.IDiscordClient client);
        int TriggerTimeInMS { get; }
        int BeforeTriggerTimeInMS { get; }
    }
}
