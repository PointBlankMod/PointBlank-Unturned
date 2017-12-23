using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using SDG.Framework.Modules;

namespace PointBlank_Unturned
{
    public class Loader : IModuleNexus
    {
        #region Nexus Interface
        public void initialize()
        {
            if ((!Provider.isServer && !Dedicator.isDedicated))
                return;

            // Setup stack
            PointBlankEnvironment.Loader_Instance = this;

            // Setup events
            DedicatedUGC.installed += DedicatedUGC_installed;
        }

        public void shutdown()
        {
            if ((!Provider.isServer && !Dedicator.isDedicated))
                return;

            // Clear events
            DedicatedUGC.installed -= DedicatedUGC_installed;

            // Clear stack
            PointBlankEnvironment.Loader_Instance = null;

            // Execute code
            PointBlank.PointBlank.UnloadPointBlank();
        }
        #endregion

        #region Event Functions
        private void DedicatedUGC_installed()
        {
            PointBlank.PointBlank.LoadPointBlank();

            Dedicator.commandWindow.title = PointBlank.PointBlankInfo.Name + " v" + PointBlank.PointBlankInfo.Version;
        }
        #endregion
    }
}
