using CorruptOSBot.Helpers;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class PVMModule : ModuleBase<SocketCommandContext>
    {
        [Command("cox")]
        [Summary("Enables a player to earn the 'CoX learner' role")]
        public async Task SayCoxAsync()
        {
            var currentUser = Context.User;

            var hasCoxRole = ((SocketGuildUser)currentUser).Roles.Any(x => x.Name == Constants.CoxLearner);

            if (!hasCoxRole)
            {
                // update the role
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == Constants.CoxLearner);
                await ((SocketGuildUser)currentUser).AddRoleAsync(role);

                await ReplyAsync(string.Format("{0} role given to <@{1}>", Constants.CoxLearner, Context.User.Id));
            }
            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Command("tob")]
        [Summary("Enables a player to earn the 'ToB learner' role")]
        public async Task SayTobAsync()
        {
            var currentUser = Context.User;

            var hasToBRole = ((SocketGuildUser)currentUser).Roles.Any(x => x.Name == Constants.TobLearner);

            if (!hasToBRole)
            {
                // update the role
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == Constants.TobLearner);
                await ((SocketGuildUser)currentUser).AddRoleAsync(role);
                await ReplyAsync(string.Format("{0} role given to <@{1}>", Constants.CoxLearner, Context.User.Id));
            }
            // delete the command posted
            await Context.Message.DeleteAsync();
        }

        [Command("nm")]
        [Summary("Enables a player to earn the 'nm learner' role")]
        public async Task SayNmAsync()
        {
            var currentUser = Context.User;

            var hasNmRole = ((SocketGuildUser)currentUser).Roles.Any(x => x.Name == Constants.NmLearner);

            if (!hasNmRole)
            {
                // update the role
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == Constants.NmLearner);
                await ((SocketGuildUser)currentUser).AddRoleAsync(role);
                await ReplyAsync(string.Format("{0} role given to <@{1}>", Constants.CoxLearner, Context.User.Id));
            }
            // delete the command posted
            await Context.Message.DeleteAsync();
        }
    }
}


