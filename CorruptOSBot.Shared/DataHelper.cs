using CorruptOSBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorruptOSBot.Shared
{
    public partial class DataHelper
    {
        public List<BotConfiguration> GetConfiguration()
        {
            try
            {
                using (var corruptosEntities = new Data.CorruptModel())
                {
                    return corruptosEntities.BotConfigurations.ToList();
                }
            }
            catch (Exception)
            {
                return new List<BotConfiguration>();
            }

        }

        internal List<Role> GetRoles()
        {
            try
            {
                using (var corruptosEntities = new Data.CorruptModel())
                {
                    return corruptosEntities.Roles.ToList();
                }
            }
            catch (Exception)
            {
                return new List<Role>();
            }
        }

        public List<Channel> GetChannels()
        {
            try
            {
                using (var corruptosEntities = new Data.CorruptModel())
                {
                    return corruptosEntities.Channels.ToList();
                }
            }
            catch (Exception)
            {
                return new List<Channel>();
            }

        }

        public List<Boss> GetBosses()
        {
            try
            {
                using (var corruptosEntities = new Data.CorruptModel())
                {
                    return corruptosEntities.Bosses.ToList();
                }
            }
            catch (Exception)
            {
                return new List<Boss>();
            }
        }

        public List<Toggle> GetToggles()
        {
            try
            {
                using (var corruptosEntities = new Data.CorruptModel())
                {
                    return corruptosEntities.Toggles.ToList();
                }
            }
            catch (Exception)
            {
                return new List<Toggle>();
            }
        }

        public async Task UpdateToggle(string command, bool toggleState)
        {
            using (var corruptosEntities = new Data.CorruptModel())
            {
                var toggle = corruptosEntities.Toggles.FirstOrDefault(x => x.Functionality == command);
                if (toggle != null)
                {
                    toggle.Toggled = toggleState;
                    await corruptosEntities.SaveChangesAsync();
                }
            }
        }
    }
}
