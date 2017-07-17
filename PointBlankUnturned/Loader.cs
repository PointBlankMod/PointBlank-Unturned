using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Framework.Modules;
using SDG.Unturned;
using PointBlank.API.Server;
using PointBlank.API;

namespace PointBlank
{
    internal class Loader : IModuleNexus
    {
        #region Properties
        public static Loader Instance;
        public static PointBlank PointBlank;
        #endregion

        #region Nexus Interface
        public void initialize()
        {
            if ((!Provider.isServer && !Dedicator.isDedicated))
                return;

            // Set the variables
            Instance = this;
            Server.ServerLocation = ServerInfo.ServerPath;
            PointBlank = new PointBlank();

            // Run code
            PointBlank.Initialize();
            Dedicator.commandWindow.title = PointBlankInfo.Name + " v" + PointBlankInfo.Version;
        }

        public void shutdown()
        {
            if ((!Provider.isServer && !Dedicator.isDedicated))
                return;

            // Run code
            PointBlank.Shutdown();

            // Set the variables
            Instance = null;
            PointBlank = null;
        }
        #endregion
    }
}
