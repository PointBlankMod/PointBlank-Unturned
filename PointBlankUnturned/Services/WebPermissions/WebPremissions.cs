using System;
using PointBlank.API.Server;
using PointBlank.API.Services;
using PointBlank.API.Collections;
using PointBlank.API.DataManagment;
using PointBlank.API.Extension;
using Config = PointBlank.Framework.Configurations.APIConfigurations;

namespace PointBlank.Services.WebPermissions
{
    internal class WebPremissions : PointBlankService
    {
        #region Variables
        private DateTime LastUpdate;
        private ConfigurationList Configurations = UnturnedEnvironment.APIConfigurations[typeof(Config)].Configurations;
        #endregion

        #region Properties
        public static string SteamGroupPath => PointBlankServer.ConfigurationsPath + "/SteamGroups.dat";
        public static string PlayerPath => PointBlankServer.ConfigurationsPath + "/Players.dat";
        public static string GroupPath => PointBlankServer.ConfigurationsPath + "/Groups.dat";

        public override int LaunchIndex => 0;

        public override bool AutoStart { get; set; } = false;
        #endregion

        public override void Load()
        {
            if (!(bool)Configurations["WebPermissions"])
                return;

            // Set the events
            ExtensionEvents.OnFrameworkTick += DownloadPermissions;
        }

        public override void Unload()
        {
            if (!(bool)Configurations["WebPermissions"])
                return;

            // Set the events
            ExtensionEvents.OnFrameworkTick -= DownloadPermissions;
        }

        #region Thread Functions
        private void DownloadPermissions()
        {
            if(LastUpdate == null || (DateTime.Now - LastUpdate).TotalSeconds > (int)Configurations["WebPermissionsInterval"])
            {
                WebsiteData.DownloadFile(string.Format((string)Configurations["WebPermissionsSite"], "SteamGroups.dat"), SteamGroupPath);
                WebsiteData.DownloadFile(string.Format((string)Configurations["WebPermissionsSite"], "Players.dat"), PlayerPath);
                WebsiteData.DownloadFile(string.Format((string)Configurations["WebPermissionsSite"], "Groups.dat"), GroupPath);
                LastUpdate = DateTime.Now;
            }
        }
        #endregion
    }
}
