using System.Reflection;
using System.Linq;
using SDG.Unturned;
using UnityEngine;
using Steamworks;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Vehicle;

namespace PointBlank.Framework.Overrides
{
    internal class _VehicleManager
    {
        [SteamCall]
        [Detour(typeof(VehicleManager), "tellVehicleDestroy", BindingFlags.Public | BindingFlags.Instance)]
        public void tellVehicleDestroy(CSteamID steamID, uint instanceID)
        {
            if (VehicleManager.instance.channel.checkServer(steamID))
            {
                InteractableVehicle vehicle = VehicleManager.vehicles.FirstOrDefault(a => a.instanceID == instanceID);

                if (vehicle == null)
                    return;

                ServerEvents.RunVehicleRemoved(vehicle);
            }

            DetourManager.CallOriginal(typeof(VehicleManager).GetMethod("tellVehicleDestroy", BindingFlags.Instance | BindingFlags.Public), VehicleManager.instance, steamID, instanceID);
        }
        
        [SteamCall]
        [Detour(typeof(VehicleManager), "tellEnterVehicle", BindingFlags.Public | BindingFlags.Instance)]
        public void tellEnterVehicle(CSteamID steamID, uint instanceID, byte seat, CSteamID player)
        {
            if (!VehicleManager.instance.channel.checkServer(steamID)) return;
            
            for (int i = 0; i < VehicleManager.vehicles.Count; i++)
            {
                if (VehicleManager.vehicles[i].instanceID != instanceID) continue;
                
                UnturnedVehicle Vehicle = UnturnedServer.Vehicles.FirstOrDefault(vehicle => vehicle.InstanceID == instanceID);
                
                VehicleEvents.RunVehicleEnter(Vehicle, seat, player);
                
                break;
            }
            
            DetourManager.CallOriginal(typeof(VehicleManager).GetMethod("tellEnterVehicle", BindingFlags.Public | BindingFlags.Instance),
                VehicleManager.instance, steamID, instanceID, seat, player);
        }
        
        [SteamCall]
        [Detour(typeof(VehicleManager), "tellExitVehicle", BindingFlags.Public | BindingFlags.Instance)]
        public void tellExitVehicle(CSteamID steamID, uint instanceID, byte seat, Vector3 point, byte angle, bool forceUpdate)
        {
            if (!VehicleManager.instance.channel.checkServer(steamID)) return;
            
            for (int i = 0; i < VehicleManager.vehicles.Count; i++)
            {
                if (VehicleManager.vehicles[i].instanceID != instanceID) continue;
                
                UnturnedVehicle Vehicle = UnturnedServer.Vehicles.FirstOrDefault(vehicle => vehicle.InstanceID == instanceID);
                
                VehicleEvents.RunVehicleExit(Vehicle, seat, point, angle, forceUpdate);

                break;
            }
            
            DetourManager.CallOriginal(typeof(VehicleManager).GetMethod("tellExitVehicle", BindingFlags.Public | BindingFlags.Instance),
                VehicleManager.instance, steamID, instanceID, seat, point, angle, forceUpdate);
        }
    }
}