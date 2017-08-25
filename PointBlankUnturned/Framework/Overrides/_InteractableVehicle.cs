using System.Reflection;
using System.Linq;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Vehicle;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
    internal static class _InteractableVehicle
    {
        #region Reflection
        private static MethodInfo mi_init = PointBlankReflect.GetMethod<InteractableVehicle>("init", BindingFlags.Instance | BindingFlags.Public);
        private static MethodInfo mi_askDamageTire = PointBlankReflect.GetMethod<InteractableVehicle>("askDamageTire", BindingFlags.Public | BindingFlags.Instance);
        private static MethodInfo mi_askDamage = PointBlankReflect.GetMethod<InteractableVehicle>("askDamage", BindingFlags.Public | BindingFlags.Instance);
        private static MethodInfo mi_askRepair = PointBlankReflect.GetMethod<InteractableVehicle>("askRepair", BindingFlags.Public | BindingFlags.Instance);
        #endregion

        [Detour(typeof(InteractableVehicle), "init", BindingFlags.Public | BindingFlags.Instance)]
        public static void init(this InteractableVehicle vehicle)
        {
            ServerEvents.RunVehicleCreated(vehicle);

            PointBlankDetourManager.CallOriginal(mi_init, vehicle, new object[0]);
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
                PointBlankDetourManager.CallOriginal(mi_askDamageTire, Vehicle, index);
        }
        
        [Detour(typeof(InteractableVehicle), "askDamage", BindingFlags.Public | BindingFlags.Instance)]
        public static void askDamage(this InteractableVehicle Vehicle, ushort amount, bool canRepair)
        {
            if (amount == 0) return;
            UnturnedVehicle vehicle = UnturnedServer.Vehicles.FirstOrDefault(v => v.InstanceID == Vehicle.instanceID);
            bool cancel = false;
            
            VehicleEvents.RunVehicleDamage(vehicle, ref amount, ref canRepair, ref cancel);
            
            if(!cancel)
                PointBlankDetourManager.CallOriginal(mi_askDamage, Vehicle, amount, canRepair);
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
                PointBlankDetourManager.CallOriginal(mi_askRepair, Vehicle, amount);
        }
    }
}
