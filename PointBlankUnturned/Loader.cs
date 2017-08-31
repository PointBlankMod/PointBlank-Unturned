using SDG.Framework.Modules;
using SDG.Unturned;
using PointBlank.API.Server;
using PointBlank.API;
using PointBlank.API.Unturned.Server;

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

            Instance = this;
            PointBlankServer.ServerLocation = ServerInfo.ServerPath;

            DedicatedUGC.installed += Started;
        }

        public void shutdown()
        {
            if ((!Provider.isServer && !Dedicator.isDedicated))
                return;

            DedicatedUGC.installed -= Started;

            // Run code
            PointBlank.Shutdown();

            // Set the variables
            Instance = null;
            PointBlank = null;
        }
        #endregion

        #region Event Functions
        private void Started()
        {
            // Set the variables
            PointBlank = new PointBlank();

            // Run code
            PointBlank.Initialize();
            Dedicator.commandWindow.title = PointBlankInfo.Name + " v" + PointBlankInfo.Version;
            ServerEvents.RunServerInitialized();
        }
        #endregion
    }
}
