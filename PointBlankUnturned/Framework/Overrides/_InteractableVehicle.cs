using System.Reflection;
using System.Linq;
using SDG.Unturned;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Vehicle;

namespace PointBlank.Framework.Overrides
{
    internal static class _InteractableVehicle
    {
        [Detour(typeof(InteractableVehicle), "init", BindingFlags.Public | BindingFlags.Instance)]
        public static void init(this InteractableVehicle vehicle)
        {
            ServerEvents.RunVehicleCreated(vehicle);

            DetourManager.CallOriginal(typeof(InteractableVehicle).GetMethod("init"), vehicle, new object[0]);
        }
        
        [Detour(typeof(InteractableVehicle), "askDamageTire", BindingFlags.Public | BindingFlags.Instance)]
        public static void askDamageTire(this InteractableVehicle Vehicle, int index)
        {
            if (index < 0)
                return;
            if (Vehicle.asset != null && !Vehicle.asset.canTiresBeDamaged)
                return;
            UnturnedVehicle UVehicle = UnturnedServer.Vehicles.FirstOrDefault(vehicle => vehicle.InstanceID == Vehicle.instanceID);
            bool cancel = false;
            
            VehicleEvents.RunVehicleTireDamage(UVehicle, ref index, ref cancel);

            if (!cancel)
                DetourManager.CallOriginal(typeof(InteractableVehicle).GetMethod("askDamageTire", BindingFlags.Public | BindingFlags.Instance), Vehicle, index);
        }
        
        [Detour(typeof(InteractableVehicle), "askDamage", BindingFlags.Public | BindingFlags.Instance)]
        public static void askDamage(this InteractableVehicle Vehicle, ushort amount, bool canRepair)
        {
            if (amount == 0) return;
            UnturnedVehicle vehicle = UnturnedServer.Vehicles.FirstOrDefault(v => v.InstanceID == Vehicle.instanceID);
            bool cancel = false;
            
            VehicleEvents.RunVehicleDamage(vehicle, ref amount, ref canRepair, ref cancel);
            
            if(!cancel)
                DetourManager.CallOriginal(typeof(InteractableVehicle).GetMethod("askDamage", BindingFlags.Public | BindingFlags.Instance),
                    Vehicle, amount, canRepair);
        }
        
        [Detour(typeof(InteractableVehicle), "askRepair", BindingFlags.Public | BindingFlags.Instance)]
        public static void askRepair(this InteractableVehicle Vehicle, ushort amount)
        {
            if (amount == 0 || Vehicle.isExploded)
                return;
            UnturnedVehicle vehicle = UnturnedServer.Vehicles.FirstOrDefault(v => v.InstanceID == Vehicle.instanceID);
            bool cancel = false;
            
            VehicleEvents.RunVehicleRepair(vehicle, ref amount, ref cancel);

            if (!cancel)
                DetourManager.CallOriginal(typeof(InteractableVehicle).GetMethod("askRepair", BindingFlags.Public | BindingFlags.Instance),
                    Vehicle, amount);
        }
    }
}
