using CorruptOSBot.Data;
using CorruptOSBot.Helpers;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Shared
{
    public static class ToggleStateManager
    {
        public static bool GetToggleState(string command, SocketUser userAdditional = null)
        {
            if (userAdditional != null && userAdditional.Id == SettingsConstants.GMKirbyDiscordId)
            {
                return true;
            }

            // default togglestate check
            using (CorruptModel corruptosEntities = new CorruptModel())
            {
                var toggled = false;
                var toggleState = corruptosEntities.Toggles.FirstOrDefault(x => x.Functionality == command);
                if (toggleState != null)
                {
                    toggled = toggleState.Toggled;
                }
                corruptosEntities.SaveChanges();

                return toggled;
            }
        }

        public static async Task ToggleModuleCommand(string command, bool toggleState)
        {
            using (CorruptModel corruptosEntities = new CorruptModel())
            {
                var toggleFromDb = corruptosEntities.Toggles.FirstOrDefault(x => x.Functionality == command);
                if (toggleFromDb != null)
                {
                    toggleFromDb.Toggled = toggleState;
                }
                await corruptosEntities.SaveChangesAsync();
            }
        }

        public static List<Toggle> GetToggleStates()
        {
            return new DataHelper().GetToggles();
        }
    }
}