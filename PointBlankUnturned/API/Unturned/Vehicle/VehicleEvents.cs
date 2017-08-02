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
        /// <param name="Vehicle">Vehicle Player is entering</param>
        /// <param name="Seat">Seat in which the Player is entering</param>
        /// <param name="player">Player that is entering the vehicle</param>
        public delegate void VehicleEnterHandler(UnturnedVehicle Vehicle, ref byte Seat, ref CSteamID Player, ref bool cancel);
        
        /// <summary>
        /// Handles exiting of the vehicle
        /// </summary>
        /// <param name="Vehicle">Vehicle in which a player is exiting</param>
        /// <param name="Seat">Seat in which the player is exiting the vehicle from</param>
        /// <param name="Point">Point in which the player will be expelled</param>
        /// <param name="Angle">Angle in which the player will be expelled</param>
        /// <param name="ForceUpdate">Whether or not to call PlayerMovement.updateVehicle</param>
        public delegate void VehicleExitHandler(UnturnedVehicle Vehicle, ref byte Seat, ref Vector3 Point, ref byte Angle, ref bool ForceUpdate, ref bool cancel);

        /// <summary>
        /// Handles damage to tires of the vehicle
        /// </summary>
        /// <param name="Vehicle">Vehicle whose tires are being damaged</param>
        /// <param name="Index">Index or tire in which is receiving damage</param>
        public delegate void VehicleTireDamageHandler(UnturnedVehicle Vehicle, ref int Index, ref bool cancel);

        /// <summary>
        /// Handles damage to vehicle
        /// </summary>
        /// <param name="Vehicle">Vehicle receiving damage</param>
        /// <param name="Amount">Amount of damage given to vehicle</param>
        /// <param name="CanRepair">Whether or not the damage can be repaired</param>
        public delegate void VehicleDamageHandler(UnturnedVehicle Vehicle, ref ushort Amount, ref bool CanRepair, ref bool cancel);

        /// <summary>
        /// Handles repairs to vehicle
        /// </summary>
        /// <param name="Vehicle">Vehicle receiving repairs</param>
        /// <param name="Amount">Amount of health given in repair</param>
        public delegate void VehicleRepairHandler(UnturnedVehicle Vehicle, ref ushort Amount, ref bool cancel);
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
        internal static void RunVehicleEnter(UnturnedVehicle Vehicle, ref byte Seat, ref CSteamID Player, ref bool cancel) =>
            OnVehicleEnter?.Invoke(Vehicle, ref Seat, ref Player, ref cancel);

        internal static void RunVehicleExit(UnturnedVehicle Vehicle, ref byte Seat, ref Vector3 Point, ref byte Angle, ref bool ForceUpdate, ref bool cancel) =>
            OnVehicleExit?.Invoke(Vehicle, ref Seat, ref Point, ref Angle, ref ForceUpdate, ref cancel);

        internal static void RunVehicleTireDamage(UnturnedVehicle Vehicle, ref int Index, ref bool cancel) => OnVehicleTireDamage?.Invoke(Vehicle, ref Index, ref cancel);

        internal static void RunVehicleDamage(UnturnedVehicle Vehicle, ref ushort Amount, ref bool CanRepair, ref bool cancel) =>
            OnVehicleDamage?.Invoke(Vehicle, ref Amount, ref CanRepair, ref cancel);

        internal static void RunVehicleRepair(UnturnedVehicle Vehicle, ref ushort Amount, ref bool cancel) => OnVehicleRepair?.Invoke(Vehicle, ref Amount, ref cancel);
        #endregion
    }
}
