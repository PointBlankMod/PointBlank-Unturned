﻿using System;
using System.Linq;
using PointBlank.API.Services;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Player;
using PointBlank.API.IPC;
using IPCM = PointBlank.API.IPC.IPCManager;

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
            IPCM.IPCType = EIPCType.CONSOLE;

            // Setup the keys
            IPCM.AddKey("PlayerCount", UnturnedServer.Players.Length.ToString());
            IPCM.AddKey("Players", (UnturnedServer.Players.Length > 0 ? string.Join(",", UnturnedServer.Players.Select(a => a.PlayerName).ToArray()) : ""));

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
            IPCM.SetValue("PlayerCount", UnturnedServer.Players.Length.ToString());
            IPCM.SetValue("Players", string.Join(",", UnturnedServer.Players.Select(a => a.PlayerName).ToArray()));
        }

        private void OnServerOutput(ref object text, ref ConsoleColor color, ref bool cancel) => IPCM.HookOutput(text.ToString());
        #endregion
    }
}
