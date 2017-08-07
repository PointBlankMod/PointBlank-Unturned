using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Updater.Internals;

namespace Updater
{
    class Program
    {
        #region Info
        public static readonly string URL_Info = "https://pastebin.com/raw/ZVcNXEVw";
        public static readonly string Stored_Info = Directory.GetCurrentDirectory() + "/UpdaterInfo.json";

        public static readonly string Modules_Path = Directory.GetCurrentDirectory() + "/Modules";
        public static readonly string Module_Path = Modules_Path + "/PointBlank";
        public static readonly string Temporary_Path = Directory.GetCurrentDirectory() + "/Temp.dat";
        #endregion

        #region Variables
        private static WeebClient Client = null;
        #endregion

        #region Properties
        public static Program Instance { get; private set; }

        public static string Path_Unturned { get; private set; }

        public static JObject Info { get; private set; }
        public static JObject Stored { get; private set; }
        #endregion

        static void Main(string[] args)
        {
            Instance = new Program();

            Console.Title = "PointBlank Unturned Updater";
            Instance.Run();
        }

        public void Run()
        {
            Console.WriteLine("Welcome to the PointBlank Updater for Unturned.");
            Console.WriteLine("The updater allows you to automatically update PointBlank on Unturned.");
            Console.WriteLine("The updater is temporary! You will be notified once the launcher can be downloaded!");
            Console.WriteLine();

            if (IsUnturnedFolder(Directory.GetCurrentDirectory()))
            {
                DownloadInfo();
                LoadStoredInfo();
                CheckLauncher();
                CheckAPIUpdates();
                CheckFrameworkUpdates();
                File.WriteAllText(Stored_Info, Stored.ToString(Formatting.None));
                Console.WriteLine("Update checks complete!");
                Console.WriteLine("Thank you for using PointBlank!");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit the program!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Please make sure you have placed the executable into the unturned folder");
                Console.WriteLine("Press any key to exit!");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        #region Static Functions
        public static bool IsUnturnedFolder(string path) => Directory.Exists(path) && File.Exists(path + "/Unturned.exe") && File.Exists(path + "/Unturned_BE.exe");
        #endregion

        #region Functions
        private void DownloadInfo()
        {
            Console.WriteLine("Downloading info....");
            try
            {
                Client = new WeebClient();
                Info = JObject.Parse(Client.DownloadString(URL_Info));

                Console.WriteLine("Info downloaded!");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while downloading the information!");
                Console.WriteLine("Please check if your network connection is working properly!");
                Console.WriteLine("If it does please manually update the updater!");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            
        }
        private void LoadStoredInfo()
        {
            Console.WriteLine("Loading stored information....");
            if (File.Exists(Stored_Info))
            {
                try
                {
                    Stored = JObject.Parse(File.ReadAllText(Stored_Info));
                    Console.WriteLine("Stored information loaded!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while trying to load stored information!");
                    Console.WriteLine("Setting default values...");
                    Stored = new JObject();
                    Stored.Add("PointBlank_Version", "0");
                    Stored.Add("API_Version", "0");
                    Console.WriteLine("Removing corrupted file...");
                    File.Delete(Stored_Info);
                    Console.WriteLine("Reset complete!");
                }
            }
            else
            {
                Stored = new JObject();
                Stored.Add("PointBlank_Version", "0");
                Stored.Add("API_Version", "0");
                Console.WriteLine("No stored information found! Assuming default values!");
            }
            Console.WriteLine();
        }

        private void CheckLauncher()
        {
            Console.WriteLine("Checking for launcher...");
            if((string)Info["Games"]["Unturned"]["Launcher_Version"] != "0")
            {
                Console.WriteLine("The official launcher for PointBlank has been released!");
                Console.WriteLine("You can download the launcher from the official github page!");
                Console.WriteLine("You can also follow the link below and download it like that!");
                Console.WriteLine((string)Info["Games"]["Unturned"]["Launcher_Latest"]);
            }
            else
            {
                Console.WriteLine("No launcher has been found!");
            }
            Console.WriteLine();
        }
        private void CheckFrameworkUpdates()
        {
            Console.WriteLine("Checking for PointBlank updates...");
            if((string)Info["PointBlank_Version"] == "0")
            {
                Console.WriteLine("PointBlank updates canceled by developer!");
                Console.WriteLine();
                return;
            }
            if (Stored == null || (string)Info["PointBlank_Version"] != (string)Stored["PointBlank_Version"])
            {
                Console.WriteLine("Downloading latest version of PointBlank...");
                try
                {
                    Client.DownloadFile((string)Info["PointBlank_Latest"], Temporary_Path);
                    using (Unzip unzip = new Unzip(Temporary_Path))
                        unzip.ExtractToDirectory(Module_Path);
                    Stored["PointBlank_Version"] = (string)Info["PointBlank_Version"];
                    File.Delete(Temporary_Path);
                    Console.WriteLine("PointBlank successfully downloaded!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to download the latest version of PointBlank!");
                    Console.WriteLine("Please make sure that the server is NOT running!");
                }
            }
            else
            {
                Console.WriteLine("PointBlank up to date!");
            }
            Console.WriteLine();
        }
        private void CheckAPIUpdates()
        {
            Console.WriteLine("Checking for Unturned API updates...");
            if ((string)Info["Games"]["Unturned"]["API_Version"] == "0")
            {
                Console.WriteLine("Unturned API updates canceled by developer!");
                Console.WriteLine();
                return;
            }
            if(Stored == null || (string)Info["Games"]["Unturned"]["API_Version"] != (string)Stored["API_Version"])
            {
                Console.WriteLine("Downloading latest version of the Unturned API...");
                try
                {
                    Client.DownloadFile((string)Info["Games"]["Unturned"]["API_Latest"], Temporary_Path);
                    Console.WriteLine("Extracting the Unturned API...");
                    using (Unzip unzip = new Unzip(Temporary_Path))
                        unzip.ExtractToDirectory(Modules_Path);
                    Stored["API_Version"] = (string)Info["Games"]["Unturned"]["API_Version"];
                    File.Delete(Temporary_Path);
                    Console.WriteLine("Unturned API successfully downloaded!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to download the latest version of the Unturned API!");
                    Console.WriteLine("Please make sure that the server is NOT running!");
                }
            }
            else
            {
                Console.WriteLine("Unturned API is up to date!");
            }
            Console.WriteLine();
        }
        #endregion
    }
}
