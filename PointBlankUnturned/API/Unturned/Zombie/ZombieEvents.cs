using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Unturned.Player;

namespace PointBlank.API.Unturned.Zombie
{
    /// <summary>
    /// Class containing all of the zombie events
    /// </summary>
    public static class ZombieEvents
    {
        #region Handlers
        /// <summary>
        /// Handles zombie alerts
        /// </summary>
        /// <param name="zombie">The affected zombie</param>
        /// <param name="player">The affected player</param>
        /// <param name="cancel">Should the alert be canceled</param>
        public delegate void ZombieAlertHandler(UnturnedZombie zombie, ref UnturnedPlayer player, ref bool cancel);

        /// <summary>
        /// Handles zombie stuns
        /// </summary>
        /// <param name="zombie">The affected zombie</param>
        /// <param name="stun">The stun length</param>
        public delegate void ZombieStunHandler(UnturnedZombie zombie, ref byte stun);
        #endregion

        #region Events
        /// <summary>
        /// Called when a zombie is alerted
        /// </summary>
        public static event ZombieAlertHandler OnZombieAlert;

        /// <summary>
        /// Called when a zombie is stunned
        /// </summary>
        public static event ZombieStunHandler OnZombieStun;
        #endregion

        #region Functions
        internal static void RunZombieAlert(UnturnedZombie zombie, ref UnturnedPlayer player, ref bool cancel) => OnZombieAlert?.Invoke(zombie, ref player, ref cancel);

        internal static void RunZombieStun(UnturnedZombie zombie, ref byte stun) => OnZombieStun?.Invoke(zombie, ref stun);
        #endregion
    }
}
