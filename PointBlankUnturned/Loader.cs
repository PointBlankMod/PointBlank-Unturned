using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Framework.Modules;
using SDG.Unturned;
using PointBlank.API;

namespace PointBlank
{
    internal class Loader : IModuleNexus
    {
        #region Properties
        public static Loader Instance;
        
        #endregion

        #region Nexus Interface
        public void initialize()
        {
            if ((!Provider.isServer && !Dedicator.isDedicated))
                return;

            // Set the variables
            Instance = this;

            // Run code
            Dedicator.commandWindow.title = PointBlankInfo.Name + " v" + PointBlankInfo.Version;
        }

        public void shutdown()
        {
            if ((!Provider.isServer && !Dedicator.isDedicated))
                return;

            // Set the variables
            Instance = null;
        }
        #endregion
    }
}
