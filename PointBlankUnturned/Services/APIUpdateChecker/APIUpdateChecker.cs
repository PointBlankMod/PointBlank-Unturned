using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Services;
using PointBlank.API.DataManagment;
using Newtonsoft.Json.Linq;

namespace PointBlank.Services.APIUpdateChecker
{
    internal class APIUpdateChecker : PointBlankService
    {
        #region Info
        public static readonly string URL = "http://pastebin.com/raw/ZVcNXEVw";
        #endregion

        #region Variables
        private Thread tChecker;

        private bool Running = true;
        #endregion

        #region Properties
        public override int LaunchIndex => 0;
        #endregion

        public override void Load()
        {
            // Setup the variables
            tChecker = new Thread(new ThreadStart(CheckUpdates));

            // Run the code
            tChecker.Start();
        }

        public override void Unload()
        {
            Running = false;
            tChecker.Abort();
        }

        #region Thread Functions
        private void CheckUpdates()
        {
            while (Running)
            {
                Thread.Sleep(300000); // Check every 5 minutes
                if (WebsiteData.GetData(URL, out string data))
                {
                    JObject info = JObject.Parse(data);

                    if ((string)info["Games"]["Unturned"]["API_Version"] == "0")
                        continue;
                    if((string)info["Games"]["Unturned"]["API_Version"] != APIInfo.Version)
                        PointBlankLogging.LogImportant("A new update is available for the API!");
                }
            }
        }
        #endregion
    }
}
