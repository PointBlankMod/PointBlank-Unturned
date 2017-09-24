using System;
using PointBlank.API.Server;
using PointBlank.API.Services;
using PointBlank.API.Collections;
using PointBlank.API.DataManagment;
using PointBlank.API.Extension;
using PointBlank.Framework.Configurations;

namespace PointBlank.Services.WebPermissions
{
    internal class WebPremissions : PointBlankService
    {
        #region Variables
        private DateTime _lastUpdate;
        private ConfigurationList _configurations = PointBlankUnturnedEnvironment.ApiConfigurations[typeof(ApiConfigurations)].Configurations;
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
            if (!(bool)_configurations["WebPermissions"])
                return;

            // Set the events
            ExtensionEvents.OnFrameworkTick += DownloadPermissions;
        }

        public override void Unload()
        {
            if (!(bool)_configurations["WebPermissions"])
                return;

            // Set the events
            ExtensionEvents.OnFrameworkTick -= DownloadPermissions;
        }

        #region Thread Functions
        private void DownloadPermissions()
        {
            if(_lastUpdate == null || (DateTime.Now - _lastUpdate).TotalSeconds > (int)_configurations["WebPermissionsInterval"])
            {
                WebsiteData.DownloadFile(string.Format((string)_configurations["WebPermissionsSite"], "SteamGroups.dat"), SteamGroupPath);
                WebsiteData.DownloadFile(string.Format((string)_configurations["WebPermissionsSite"], "Players.dat"), PlayerPath);
                WebsiteData.DownloadFile(string.Format((string)_configurations["WebPermissionsSite"], "Groups.dat"), GroupPath);
                _lastUpdate = DateTime.Now;
            }
        }
        #endregion
    }
}
