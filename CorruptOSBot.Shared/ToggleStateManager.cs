﻿using CorruptOSBot.Data;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Shared
{
    public static class ToggleStateManager
    {
        public static bool GetToggleState(string command, SocketUser userAdditional = null)
        {
            // override for admin
            if (userAdditional != null && userAdditional.Id == 174621705581494272)
            {
                return true;
            }
            
            // default togglestate check
            using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
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
            using (Data.CorruptModel corruptosEntities = new Data.CorruptModel())
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
