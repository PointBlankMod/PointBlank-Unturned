using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Server;
using PointBlank.API.Services;
using PointBlank.API.Collections;
using PointBlank.API.DataManagment;
using Config = PointBlank.Framework.Configurations.APIConfigurations;

namespace PointBlank.Services.WebPermissions
{
    internal class WebPremissions : Service
    {
        #region Info
        public static readonly string SteamGroupPath = Server.ConfigurationsPath + "/SteamGroups.dat";
        public static readonly string PlayerPath = Server.ConfigurationsPath + "/Players.dat";
        public static readonly string GroupPath = Server.ConfigurationsPath + "/Groups.dat";
        #endregion

        #region Variables
        private Thread tDownloader;

        private bool Running = true;
        private DateTime LastUpdate;
        private ConfigurationList Configurations = Enviroment.APIConfigurations[typeof(Config)].Configurations;
        #endregion

        #region Properties
        public override int LaunchIndex => 0;

        public override bool AutoStart { get; set; } = false;
        #endregion

        public override void Load()
        {
            if (!(bool)Configurations["WebPermissions"])
                return;
            // Set the variables
            tDownloader = new Thread(new ThreadStart(DownloadPermissions));

            // Run the code
            tDownloader.Start();
        }

        public override void Unload()
        {
            if (!(bool)Configurations["WebPermissions"])
                return;
            // Set the variables
            Running = false;

            // Run the code
            tDownloader.Abort();
        }

        #region Thread Functions
        private void DownloadPermissions()
        {
            while (Running)
            {
                if(LastUpdate == null || (DateTime.Now - LastUpdate).TotalSeconds > (int)Configurations["WebPermissionsInterval"])
                {
                    WebsiteData.DownloadFile(string.Format((string)Configurations["WebPermissionsSite"], "SteamGroups.dat"), SteamGroupPath);
                    WebsiteData.DownloadFile(string.Format((string)Configurations["WebPermissionsSite"], "Players.dat"), PlayerPath);
                    WebsiteData.DownloadFile(string.Format((string)Configurations["WebPermissionsSite"], "Groups.dat"), GroupPath);
                    LastUpdate = DateTime.Now;
                }
            }
        }
        #endregion
    }
}
