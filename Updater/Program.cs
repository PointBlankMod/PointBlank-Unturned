using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater
{
    class Program
    {
        #region Properties
        public static Program Instance { get; private set; }

        public static string Path_Steam { get; private set; }
        public static string Path_Unturned { get; private set; }
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
            Console.WriteLine("It also offers automatic updating of unturned!");
            Console.WriteLine("This updater is can be used stand-alone or as an updater for the Launcher!");
            Console.WriteLine();

            if (IsSteamFolder(Directory.GetCurrentDirectory()))
                Run_SteamFolder();
            else if (IsUnturnedFolder(Directory.GetCurrentDirectory()))
                Run_UnturnedFolder();
            else
            {
                Console.WriteLine("Please make sure you have placed the executable into the steam folder or the unturned folder!");
                Console.WriteLine("Press any key to exit!");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        #region Static Functions
        public static bool IsSteamFolder(string path) => Directory.Exists(path) && 
            (File.Exists(path + "/steamcmd.exe") || File.Exists(path + "/steamcmd.sh") || File.Exists(path + "/steamcmd")) && 
            (File.Exists(path + "/steam.dll") || File.Exists(path + "/steam.sh") || Directory.Exists(path + "/osx32"));
        public static bool IsUnturnedFolder(string path) => Directory.Exists(path) && File.Exists(path + "/Unturned.exe") && File.Exists(path + "/Unturned_BE.exe");
        #endregion

        #region Functions
        private void Run_SteamFolder()
        {
            Path_Steam = Directory.GetCurrentDirectory();
            Console.WriteLine("You have placed the executable into the steam folder! This activates automatic game updating!");
            Console.WriteLine("The game will be automatically updated 5 minutes after release!");
            Console.WriteLine("If you do not wish for automatic game updating then place the executable into the unturned folder!");
            Console.WriteLine();

            do
            {
                Console.Write("Path to unturned folder: ");
                Path_Unturned = Console.ReadLine();
            } while (string.IsNullOrEmpty(Path_Unturned) || !IsUnturnedFolder(Path_Unturned));

            
        }
        private void Run_UnturnedFolder()
        {

        }

        private void CheckUpdates()
        {
            
        }
        #endregion
    }
}
