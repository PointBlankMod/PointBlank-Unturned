using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;

namespace PointBlank.API.Unturned.Zombie
{
    /// <summary>
    /// The unturned zombie class
    /// </summary>
    public class UnturnedZombie
    {
        #region Properties
        /// <summary>
        /// The zombie instance
        /// </summary>
        public SDG.Unturned.Zombie Zombie { get; private set; }

        // Zombie information
        /// <summary>
        /// The zombie's ID
        /// </summary>
        public ushort ID => Zombie.id;
        /// <summary>
        /// Is the zombie a boss zombie
        /// </summary>
        public bool IsBoss => Zombie.isBoss;
        /// <summary>
        /// Is the zombie currently hunting a player
        /// </summary>
        public bool IsHunting
        {
            get => Zombie.isHunting;
            set => Zombie.isHunting = value;
        }
        /// <summary>
        /// Is the zombie hyper
        /// </summary>
        public bool IsHyper => Zombie.isHyper;
        /// <summary>
        /// Is the zombie type a mega
        /// </summary>
        public bool IsMega => Zombie.isMega;
        /// <summary>
        /// Is the zombie radioactive
        /// </summary>
        public bool IsRadioactive => Zombie.isRadioactive;
        /// <summary>
        /// Is the zombie wandering around
        /// </summary>
        public bool IsWandering
        {
            get => !IsHunting;
            set => IsHunting = !value;
        }
        /// <summary>
        /// What type is the zombie
        /// </summary>
        public EZombieSpeciality Speciality => Zombie.speciality;
        /// <summary>
        /// The type of zombie it is
        /// </summary>
        public byte Type => Zombie.type;
        /// <summary>
        /// The zombie's shirt item ID
        /// </summary>
        public ushort ShirtID => LevelZombies.tables[Type].slots[0].table[Zombie.shirt].item;
        /// <summary>
        /// The zombie's pants item ID
        /// </summary>
        public ushort PantsID => LevelZombies.tables[Type].slots[1].table[Zombie.pants].item;
        /// <summary>
        /// The zombie's hat item ID
        /// </summary>
        public ushort HatID => LevelZombies.tables[Type].slots[2].table[Zombie.hat].item;
        /// <summary>
        /// The zombie's gear item ID
        /// </summary>
        public ushort GearID => LevelZombies.tables[Type].slots[3].table[Zombie.gear].item;
        /// <summary>
        /// Is the zombie dead
        /// </summary>
        public bool IsDead => Zombie.isDead;
        /// <summary>
        /// Is the zombie stunned
        /// </summary>
        public bool IsStunned
        {
            get => PointBlankReflect.GetField<SDG.Unturned.Zombie>("isStunned", PointBlankReflect.INSTANCE_FLAG).GetValue<bool>(Zombie);
            set => PointBlankReflect.GetField<SDG.Unturned.Zombie>("isStunned", PointBlankReflect.INSTANCE_FLAG).SetValue(Zombie, value);
        }
        /// <summary>
        /// The zombie's health
        /// </summary>
        public ushort Health
        {
            get => PointBlankReflect.GetField<SDG.Unturned.Zombie>("health", PointBlankReflect.INSTANCE_FLAG).GetValue<ushort>(Zombie);
            set => PointBlankReflect.GetField<SDG.Unturned.Zombie>("health", PointBlankReflect.INSTANCE_FLAG).SetValue(Zombie, value);
        }
        /// <summary>
        /// The max health of the zombie
        /// </summary>
        public ushort MaxHealth
        {
            get => PointBlankReflect.GetField<SDG.Unturned.Zombie>("maxHealth", PointBlankReflect.INSTANCE_FLAG).GetValue<ushort>(Zombie);
            set => PointBlankReflect.GetField<SDG.Unturned.Zombie>("maxHealth", PointBlankReflect.INSTANCE_FLAG).SetValue(Zombie, value);
        }
        #endregion

        private UnturnedZombie(SDG.Unturned.Zombie zombie)
        {
            // Set the variables
            Zombie = zombie;

            // Run the code
            UnturnedServer.AddZombie(this);
        }

        #region Static Functions
        internal static UnturnedZombie Create(SDG.Unturned.Zombie zombie)
        {
            UnturnedZombie z = UnturnedServer.Zombies.FirstOrDefault(a => a.Zombie == zombie);

            return z ?? new UnturnedZombie(zombie);
        }
        #endregion

        #region Functions
        public void Stun() => PointBlankReflect.GetMethod<SDG.Unturned.Zombie>("stun", PointBlankReflect.INSTANCE_FLAG).RunMethod(Zombie);
        #endregion
    }
}
