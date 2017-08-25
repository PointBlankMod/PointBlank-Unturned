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

        /// <summary>
        /// Handles zombie damage
        /// </summary>
        /// <param name="zombie">The affected zombie</param>
        /// <param name="player">The damager</param>
        /// <param name="amount">The amount of damage done</param>
        /// <param name="cause">The cause of the damage</param>
        /// <param name="xp">The experience gotten from the attack</param>
        /// <param name="cancel">Should the damage be canceled</param>
        public delegate void ZombieDamageHandler(UnturnedZombie zombie, UnturnedPlayer player, ref ushort amount, ref bool cancel);
        /// <summary>
        /// Handles zombie deaths
        /// </summary>
        /// <param name="zombie">The affected zombie</param>
        /// <param name="cancel">Should the kill be canceled</param>
        public delegate void ZombieDieHandler(UnturnedZombie zombie, ref bool cancel);
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

        /// <summary>
        /// Called when a zombie is damaged
        /// </summary>
        public static event ZombieDamageHandler OnZombieDamage;
        /// <summary>
        /// Called when a zombie dies
        /// </summary>
        public static event ZombieDieHandler OnZombieDie;
        #endregion

        #region Functions
        internal static void RunZombieAlert(UnturnedZombie zombie, ref UnturnedPlayer player, ref bool cancel) => OnZombieAlert?.Invoke(zombie, ref player, ref cancel);

        internal static void RunZombieStun(UnturnedZombie zombie, ref byte stun) => OnZombieStun?.Invoke(zombie, ref stun);

        internal static void RunZombieDamage(UnturnedZombie zombie, UnturnedPlayer player, ref ushort amount, ref bool cancel) =>
            OnZombieDamage?.Invoke(zombie, player, ref amount, ref cancel);
        internal static void RunZombieKill(UnturnedZombie zombie, ref bool cancel) => OnZombieDie?.Invoke(zombie, ref cancel);
        #endregion
    }
}
