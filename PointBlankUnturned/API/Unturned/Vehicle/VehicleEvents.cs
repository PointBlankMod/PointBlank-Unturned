using System;
using Steamworks;
using UnityEngine;

namespace PointBlank.API.Unturned.Vehicle
{
    /// <summary>
    /// Class containing all events for vehicles
    /// </summary>
    public class VehicleEvents
    {
        #region Handlers
        /// <summary>
        /// Handles entering of the vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle Player is entering</param>
        /// <param name="Seat">Seat in which the Player is entering</param>
        /// <param name="player">Player that is entering the vehicle</param>
        public delegate void VehicleEnterHandler(UnturnedVehicle vehicle, ref SDG.Unturned.Player player, ref bool cancel);
        
        /// <summary>
        /// Handles exiting of the vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle in which a player is exiting</param>
        /// <param name="Seat">Seat in which the player is exiting the vehicle from</param>
        /// <param name="Point">Point in which the player will be expelled</param>
        /// <param name="Angle">Angle in which the player will be expelled</param>
        /// <param name="ForceUpdate">Whether or not to call PlayerMovement.updateVehicle</param>
        public delegate void VehicleExitHandler(UnturnedVehicle vehicle, ref SDG.Unturned.Player player, ref bool cancel);

        /// <summary>
        /// Handles damage to tires of the vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle whose tires are being damaged</param>
        /// <param name="index">Index or tire in which is receiving damage</param>
        public delegate void VehicleTireDamageHandler(UnturnedVehicle vehicle, ref int index, ref bool cancel);

        /// <summary>
        /// Handles damage to vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle receiving damage</param>
        /// <param name="amount">Amount of damage given to vehicle</param>
        /// <param name="canRepair">Whether or not the damage can be repaired</param>
        public delegate void VehicleDamageHandler(UnturnedVehicle vehicle, ref ushort amount, ref bool canRepair, ref bool cancel);

        /// <summary>
        /// Handles repairs to vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle receiving repairs</param>
        /// <param name="amount">Amount of health given in repair</param>
        public delegate void VehicleRepairHandler(UnturnedVehicle vehicle, ref ushort amount, ref bool cancel);
        #endregion

        #region Events
        /// <summary>
        /// Called when a vehicle is entered
        /// </summary>
        public static event VehicleEnterHandler OnVehicleEnter;

        /// <summary>
        /// Called when a vehicle is exited
        /// </summary>
        public static event VehicleExitHandler OnVehicleExit;

        /// <summary>
        /// Called when a vehicles tire is damaged
        /// </summary>
        public static event VehicleTireDamageHandler OnVehicleTireDamage;

        /// <summary>
        /// Called when a vehicle is damaged
        /// </summary>
        public static event VehicleDamageHandler OnVehicleDamage;

        /// <summary>
        /// Called when a vehicle is repaired
        /// </summary>
        public static event VehicleRepairHandler OnVehicleRepair;
        #endregion
        
        #region Functions
        internal static void RunVehicleEnter(UnturnedVehicle vehicle, ref SDG.Unturned.Player player, ref bool cancel) =>
            OnVehicleEnter?.Invoke(vehicle, ref player, ref cancel);

        internal static void RunVehicleExit(UnturnedVehicle vehicle, ref SDG.Unturned.Player player, ref bool cancel) =>
            OnVehicleExit?.Invoke(vehicle, ref player, ref cancel);

        internal static void RunVehicleTireDamage(UnturnedVehicle vehicle, ref int index, ref bool cancel) => OnVehicleTireDamage?.Invoke(vehicle, ref index, ref cancel);

        internal static void RunVehicleDamage(UnturnedVehicle vehicle, ref ushort amount, ref bool canRepair, ref bool cancel) =>
            OnVehicleDamage?.Invoke(vehicle, ref amount, ref canRepair, ref cancel);

        internal static void RunVehicleRepair(UnturnedVehicle vehicle, ref ushort amount, ref bool cancel) => OnVehicleRepair?.Invoke(vehicle, ref amount, ref cancel);
        #endregion
    }
}
