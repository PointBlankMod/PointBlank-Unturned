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
        public delegate void VehicleEnterHandler(UnturnedVehicle Vehicle, Byte Seat, CSteamID Player);
        
        /// <summary>
        /// Handles exiting of the vehicle
        /// </summary>
        /// <param name="Vehicle">Vehicle in which a player is exiting</param>
        /// <param name="Seat">Seat in which the player is exiting the vehicle from</param>
        /// <param name="Point">Point in which the player will be expelled</param>
        /// <param name="Angle">Angle in which the player will be expelled</param>
        /// <param name="ForceUpdate">Whether or not to call PlayerMovement.updateVehicle</param>
        public delegate void VehicleExitHandler(UnturnedVehicle Vehicle, Byte Seat, Vector3 Point, Byte Angle, bool ForceUpdate);

        /// <summary>
        /// Handles damage to tires of the vehicle
        /// </summary>
        /// <param name="Vehicle">Vehicle whose tires are being damaged</param>
        /// <param name="Index">Index or tire in which is receiving damage</param>
        public delegate void VehicleTireDamageHandler(UnturnedVehicle Vehicle, int Index);

        /// <summary>
        /// Handles damage to vehicle
        /// </summary>
        /// <param name="Vehicle">Vehicle receiving damage</param>
        /// <param name="Amount">Amount of damage given to vehicle</param>
        /// <param name="CanRepair">Whether or not the damage can be repaired</param>
        public delegate void VehicleDamageHandler(UnturnedVehicle Vehicle, ushort Amount, bool CanRepair);

        /// <summary>
        /// Handles repairs to vehicle
        /// </summary>
        /// <param name="Vehicle">Vehicle receiving repairs</param>
        /// <param name="Amount">Amount of health given in repair</param>
        public delegate void VehicleRepairHandler(UnturnedVehicle Vehicle, ushort Amount);
        
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

        internal static void RunVehicleEnter(UnturnedVehicle Vehicle, Byte Seat, CSteamID Player) =>
            OnVehicleEnter?.Invoke(Vehicle, Seat, Player);

        internal static void RunVehicleExit(UnturnedVehicle Vehicle, Byte Seat, Vector3 Point, Byte Angle, bool ForceUpdate) =>
            OnVehicleExit?.Invoke(Vehicle, Seat, Point, Angle, ForceUpdate);

        internal static void RunVehicleTireDamage(UnturnedVehicle Vehicle, int Index) => OnVehicleTireDamage?.Invoke(Vehicle, Index);

        internal static void RunVehicleDamage(UnturnedVehicle Vehicle, ushort Amount, bool CanRepair) =>
            OnVehicleDamage?.Invoke(Vehicle, Amount, CanRepair);

        internal static void RunVehicleRepair(UnturnedVehicle Vehicle, ushort Amount) => OnVehicleRepair?.Invoke(Vehicle, Amount);

        #endregion
    }
}
