using System;
using PointBlank.API.Groups;
using PointBlank.API.Unturned.Item;
using SDG.Unturned;

namespace PointBlank.API.Unturned.Player
{
    /// <summary>
    /// Contains all unturned player based events
    /// </summary>
    public static class PlayerEvents
    {
        #region Handlers
        /// <summary>
        /// Handles permission changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="permission">The changed permission</param>
        public delegate void PermissionsChangedHandler(UnturnedPlayer player, string permission);
        /// <summary>
        /// Handles group changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="group">The changed group</param>
        public delegate void GroupsChangedHandler(UnturnedPlayer player, PointBlankGroup group);
        /// <summary>
        /// Handles the player list changes of a specific player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="target">The changed player</param>
        public delegate void PlayerListChangedHandler(UnturnedPlayer player, UnturnedPlayer target);

        /// <summary>
        /// Handles prefix changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="prefix">The changed prefix</param>
        public delegate void PrefixesChangedHandler(UnturnedPlayer player, string prefix);
        /// <summary>
        /// Handles suffix changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="suffix">The changed suffix</param>
        public delegate void SuffixesChangedHandler(UnturnedPlayer player, string suffix);

        /// <summary>
        /// Handles player deaths
        /// </summary>
        /// <param name="player">The player that got killed</param>
        /// <param name="cause">The cause of death</param>
        /// <param name="killer">The person who killed the player</param>
        public delegate void PlayerDeathHandler(UnturnedPlayer player, ref EDeathCause cause, ref UnturnedPlayer killer);
        /// <summary>
        /// Handles player hurt events
        /// </summary>
        /// <param name="player">The player that got hurt</param>
        /// <param name="damage">The amount of damage done to the player</param>
        /// <param name="cause">The cause of the damage</param>
        /// <param name="limb">The limb that got hurt</param>
        /// <param name="damager">The person that caused damage</param>
        /// <param name="cancel">Should the damage be canceled</param>
        public delegate void PlayerHurtHandler(UnturnedPlayer player, ref byte damage, ref EDeathCause cause, ref ELimb limb, ref UnturnedPlayer damager, ref bool cancel);
        /// <summary>
        /// Handles player kills
        /// </summary>
        /// <param name="player">The player that killed another player</param>
        /// <param name="cause">The cause of the kill</param>
        /// <param name="victim">The player that got killed</param>
        public delegate void PlayerKillHandler(UnturnedPlayer player, ref EDeathCause cause, ref UnturnedPlayer victim);

        /// <summary>
        /// Handles player skill upgrades
        /// </summary>
        /// <param name="Player">Player who upgraded their skill</param>
        /// <param name="Skillset">Skillset in which skill that is being upgraded belongs to</param>
        /// <param name="Skill">Skill being upgraded</param>
        /// <param name="Level">Upgraded level of skill</param>
        public delegate void PlayerSkillUpgradeHandler(UnturnedPlayer Player, Byte Specialty, Byte Skill, Byte Level);

        /*/// <summary>
        /// Handles item equipping and deequipping
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="item">The targeted item</param>
        public delegate void PlayerItemEquipHandler(UnturnedPlayer player, UnturnedStoredItem item);

        /// <summary>
        /// Handles picking up and dropping of items
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="item">The targeted item</param>
        public delegate void PlayerItemUpdateHandler(UnturnedPlayer player, UnturnedStoredItem item);*/
        #endregion

        #region Events
        /// <summary>
        /// Called when a player is added to the player's player list
        /// </summary>
        public static event PlayerListChangedHandler OnListPlayerAdded;
        /// <summary>
        /// Called when a player is removed from the player's player list
        /// </summary>
        public static event PlayerListChangedHandler OnListPlayerRemoved;

        /// <summary>
        /// Called when a prefix is added
        /// </summary>
        public static event PrefixesChangedHandler OnPrefixAdded;
        /// <summary>
        /// Called when a prefix is removed
        /// </summary>
        public static event PrefixesChangedHandler OnPrefixRemoved;

        /// <summary>
        /// Called when a suffix is added
        /// </summary>
        public static event SuffixesChangedHandler OnSuffixAdded;
        /// <summary>
        /// Called when a suffix is removed
        /// </summary>
        public static event SuffixesChangedHandler OnSuffixRemoved;

        /// <summary>
        /// Called when a player dies
        /// </summary>
        public static event PlayerDeathHandler OnPlayerDied;
        /// <summary>
        /// Called when a player is hurt
        /// </summary>
        public static event PlayerHurtHandler OnPlayerHurt;
        /// <summary>
        /// Called when a player kills another player
        /// </summary>
        public static event PlayerKillHandler OnPlayerKill;

        /// <summary>
        /// Called when a player upgrades their skill
        /// </summary>
        public static event PlayerSkillUpgradeHandler OnPlayerSkillUpgrade;

        /*/// <summary>
        /// Called when an item is equipped
        /// </summary>
        public static event PlayerItemEquipHandler OnPlayerItemEquip;
        /// <summary>
        /// Called when an item is deequipped
        /// </summary>
        public static event PlayerItemEquipHandler OnPlayerItemDeequip;

        /// <summary>
        /// Called when a player drops an item
        /// </summary>
        public static event PlayerItemUpdateHandler OnPlayerItemDropped;
        /// <summary>
        /// Called when a player picks up an item
        /// </summary>
        public static event PlayerItemUpdateHandler OnPlayerItemPickup;*/
        #endregion

        #region Functions
        internal static void RunListPlayerAdd(UnturnedPlayer player, UnturnedPlayer target) => OnListPlayerAdded?.Invoke(player, target);
        internal static void RunListPlayerRemove(UnturnedPlayer player, UnturnedPlayer target) => OnListPlayerRemoved?.Invoke(player, target);

        internal static void RunPrefixAdd(UnturnedPlayer player, string prefix) => OnPrefixAdded?.Invoke(player, prefix);
        internal static void RunPrefixRemove(UnturnedPlayer player, string prefix) => OnPrefixRemoved?.Invoke(player, prefix);

        internal static void RunSuffixAdd(UnturnedPlayer player, string suffix) => OnSuffixAdded?.Invoke(player, suffix);
        internal static void RunSuffixRemove(UnturnedPlayer player, string suffix) => OnSuffixRemoved?.Invoke(player, suffix);

        internal static void RunPlayerHurt(SteamPlayer player, ref byte damage, ref EDeathCause cause, ref ELimb limb, ref UnturnedPlayer damager, ref bool cancel)
        {
            if (OnPlayerHurt == null)
                return;
            UnturnedPlayer ply = UnturnedPlayer.Get(player);

            OnPlayerHurt(ply, ref damage, ref cause, ref limb, ref damager, ref cancel);
            if (cancel)
                return;
            if ((ply.Health - damage) >= 0) return;
            OnPlayerDied(ply, ref cause, ref damager);
            OnPlayerKill(damager, ref cause, ref ply);
        }

        internal static void RunPlayerSkillUpgrade(UnturnedPlayer Player, Byte Specialty, Byte Skill, Byte Level) =>
            OnPlayerSkillUpgrade?.Invoke(Player, Specialty, Skill, Level);

        /*internal static void RunItemEquip(UnturnedPlayer player, UnturnedStoredItem item) => OnPlayerItemEquip?.Invoke(player, item);
        internal static void RunItemDeequip(UnturnedPlayer player, UnturnedStoredItem item) => OnPlayerItemDeequip?.Invoke(player, item);

        internal static void RunItemDrop(UnturnedPlayer player, UnturnedStoredItem item) => OnPlayerItemDropped?.Invoke(player, item);
        internal static void RunItemPickup(UnturnedPlayer player, UnturnedStoredItem item) => OnPlayerItemPickup?.Invoke(player, item);*/
        #endregion
    }
}
