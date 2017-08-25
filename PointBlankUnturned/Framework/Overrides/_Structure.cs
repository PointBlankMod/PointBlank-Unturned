using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Structure;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
    internal static class _Structure
    {
        #region Reflection
        private static MethodInfo mi_askDamage = PointBlankReflect.GetMethod<Structure>("askDamage", BindingFlags.Instance | BindingFlags.Public);
        private static MethodInfo mi_askRepair = PointBlankReflect.GetMethod<Structure>("askRepair", BindingFlags.Instance | BindingFlags.Public);
        #endregion

        [Detour(typeof(Structure), "askDamage", BindingFlags.Public | BindingFlags.Instance)]
        public static void askDamage(this Structure stru, ushort amount)
        {
            // Set the variables
            bool cancel = false;

            // Run the events
            StructureEvents.RunDamageStructure(UnturnedStructure.FindStructure(stru), ref amount, ref cancel);

            // Run the original function
            if (!cancel)
                PointBlankDetourManager.CallOriginal(mi_askDamage, stru, amount);
        }

        [Detour(typeof(Structure), "askRepair", BindingFlags.Public | BindingFlags.Instance)]
        public static void askRepair(this Structure stru, ushort amount)
        {
            // Set the events
            bool cancel = false;

            // Run the events
            StructureEvents.RunRepairStructure(UnturnedStructure.FindStructure(stru), ref amount, ref cancel);

            // Run the original function
            if (!cancel)
                PointBlankDetourManager.CallOriginal(mi_askRepair, stru, amount);
        }
    }
}
