using System.Reflection;
using System.Linq;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Vehicle;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
	internal static class OvInteractableVehicle
	{
        #region Reflection
        private static MethodInfo _miInit = PointBlankReflect.GetMethod<SDG.Unturned.InteractableVehicle>("init", BindingFlags.Instance | BindingFlags.Public);
        private static MethodInfo _miAskDamageTire = PointBlankReflect.GetMethod<SDG.Unturned.InteractableVehicle>("askDamageTire", BindingFlags.Public | BindingFlags.Instance);
        private static MethodInfo _miAskDamage = PointBlankReflect.GetMethod<SDG.Unturned.InteractableVehicle>("askDamage", BindingFlags.Public | BindingFlags.Instance);
        private static MethodInfo _miAskRepair = PointBlankReflect.GetMethod<SDG.Unturned.InteractableVehicle>("askRepair", BindingFlags.Public | BindingFlags.Instance);
        #endregion

        [Detour(typeof(SDG.Unturned.InteractableVehicle), "init", BindingFlags.Public | BindingFlags.Instance)]
        public static void Init(this SDG.Unturned.InteractableVehicle vehicle)
        {
            ServerEvents.RunVehicleCreated(vehicle);

            PointBlankDetourManager.CallOriginal(_miInit, vehicle, new object[0]);
        }
        
        [Detour(typeof(SDG.Unturned.InteractableVehicle), "askDamageTire", BindingFlags.Public | BindingFlags.Instance)]
        public static void AskDamageTire(this SDG.Unturned.InteractableVehicle ivehicle, int index)
        {
            if (index < 0)
                return;
            if (ivehicle.asset != null && !ivehicle.asset.canTiresBeDamaged)
                return;
            UnturnedVehicle uvehicle = UnturnedServer.Vehicles.FirstOrDefault(vehicle => vehicle.InstanceId == ivehicle.instanceID);
            bool cancel = false;
            
            VehicleEvents.RunVehicleTireDamage(uvehicle, ref index, ref cancel);

            if (!cancel)
                PointBlankDetourManager.CallOriginal(_miAskDamageTire, uvehicle, index);
        }
        
        [Detour(typeof(SDG.Unturned.InteractableVehicle), "askDamage", BindingFlags.Public | BindingFlags.Instance)]
        public static void AskDamage(this SDG.Unturned.InteractableVehicle ivehicle, ushort amount, bool canRepair)
        {
            if (amount == 0) return;
            UnturnedVehicle vehicle = UnturnedServer.Vehicles.FirstOrDefault(v => v.InstanceId == ivehicle.instanceID);
            bool cancel = false;
            
            VehicleEvents.RunVehicleDamage(vehicle, ref amount, ref canRepair, ref cancel);
            
            if(!cancel)
                PointBlankDetourManager.CallOriginal(_miAskDamage, ivehicle, amount, canRepair);
        }
        
        [Detour(typeof(SDG.Unturned.InteractableVehicle), "askRepair", BindingFlags.Public | BindingFlags.Instance)]
        public static void AskRepair(this SDG.Unturned.InteractableVehicle ivehicle, ushort amount)
        {
            if (amount == 0 || ivehicle.isExploded)
                return;
            UnturnedVehicle vehicle = UnturnedServer.Vehicles.FirstOrDefault(v => v.InstanceId == ivehicle.instanceID);
            bool cancel = false;
            
            VehicleEvents.RunVehicleRepair(vehicle, ref amount, ref cancel);

            if (!cancel)
                PointBlankDetourManager.CallOriginal(_miAskRepair, vehicle, amount);
        }
    }
}
