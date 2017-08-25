using System;
using System.Linq;
using PointBlank.API.IPC;
using PointBlank.API.Services;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Player;

namespace PointBlank.Services.Launcher
{
    internal class Launcher : PointBlankService
    {
        #region Properties
        public override int LaunchIndex => 0;
        #endregion

        public override void Load()
        {
            if (!Environment.GetCommandLineArgs().Contains("-launcher"))
                return;

            // Set the configuration
            PointBlankIPCManager.IPCType = EIPCType.CONSOLE;

            // Setup the keys
            PointBlankIPCManager.AddKey("PlayerCount", UnturnedServer.Players.Length.ToString());
            PointBlankIPCManager.AddKey("Players", (UnturnedServer.Players.Length > 0 ? string.Join(",", UnturnedServer.Players.Select(a => a.PlayerName).ToArray()) : ""));

            // Setup the events
            ServerEvents.OnPlayerConnected += OnPlayerUpdate;
            ServerEvents.OnPlayerDisconnected += OnPlayerUpdate;
            ServerEvents.OnConsoleOutput += OnServerOutput;
        }

        public override void Unload()
        {
            if (!Environment.GetCommandLineArgs().Contains("-launcher"))
                return;

            // Unload the events
            ServerEvents.OnPlayerConnected -= OnPlayerUpdate;
            ServerEvents.OnPlayerDisconnected -= OnPlayerUpdate;
            ServerEvents.OnConsoleOutput -= OnServerOutput;
        }

        #region Event Functions
        private void OnPlayerUpdate(UnturnedPlayer player)
        {
            PointBlankIPCManager.SetValue("PlayerCount", UnturnedServer.Players.Length.ToString());
            PointBlankIPCManager.SetValue("Players", string.Join(",", UnturnedServer.Players.Select(a => a.PlayerName).ToArray()));
        }

        private void OnServerOutput(ref object text, ref ConsoleColor color, ref bool cancel) => PointBlankIPCManager.HookOutput(text.ToString());
        #endregion
    }
}
