using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.IPC;
using PointBlank.API.Services;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Player;
using IPCM = PointBlank.API.IPC.IPCManager;

namespace PointBlank.Services.Launcher
{
    [Service("Launcher", true)]
    internal class Launcher : Service
    {
        #region Override Functions
        public override void Load()
        {
            // Setup the keys
            IPCM.AddKey("PlayerCount", UnturnedServer.Players.Length.ToString());
            IPCM.AddKey("Players", (UnturnedServer.Players.Length > 0 ? string.Join(",", UnturnedServer.Players.Select(a => a.PlayerName).ToArray()) : ""));

            // Setup the events
            ServerEvents.OnPlayerConnected += new ServerEvents.PlayerConnectionHandler(OnPlayerUpdate);
            ServerEvents.OnPlayerDisconnected += new ServerEvents.PlayerConnectionHandler(OnPlayerUpdate);
        }

        public override void Unload()
        {
            
        }
        #endregion

        #region Event Functions
        private void OnPlayerUpdate(UnturnedPlayer player)
        {
            IPCM.SetValue("PlayerCount", UnturnedServer.Players.Length.ToString());
            IPCM.SetValue("Players", string.Join(",", UnturnedServer.Players.Select(a => a.PlayerName).ToArray()));
        }
        #endregion
    }
}
