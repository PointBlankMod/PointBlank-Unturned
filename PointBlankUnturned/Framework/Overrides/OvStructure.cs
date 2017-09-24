using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Structure;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
	internal static class OvStructure
	{
        #region Reflection
        private static MethodInfo _miAskDamage = PointBlankReflect.GetMethod<SDG.Unturned.Structure>("askDamage", BindingFlags.Instance | BindingFlags.Public);
        private static MethodInfo _miAskRepair = PointBlankReflect.GetMethod<SDG.Unturned.Structure>("askRepair", BindingFlags.Instance | BindingFlags.Public);
        #endregion

        [Detour(typeof(SDG.Unturned.Structure), "askDamage", BindingFlags.Public | BindingFlags.Instance)]
        public static void AskDamage(this SDG.Unturned.Structure stru, ushort amount)
        {
            // Set the variables
            bool cancel = false;

            // Run the events
            StructureEvents.RunDamageStructure(UnturnedStructure.FindStructure(stru), ref amount, ref cancel);

            // Run the original function
            if (!cancel)
                PointBlankDetourManager.CallOriginal(_miAskDamage, stru, amount);
        }

        [Detour(typeof(SDG.Unturned.Structure), "askRepair", BindingFlags.Public | BindingFlags.Instance)]
        public static void AskRepair(this SDG.Unturned.Structure stru, ushort amount)
        {
            // Set the events
            bool cancel = false;

            // Run the events
            StructureEvents.RunRepairStructure(UnturnedStructure.FindStructure(stru), ref amount, ref cancel);

            // Run the original function
            if (!cancel)
                PointBlankDetourManager.CallOriginal(_miAskRepair, stru, amount);
        }
    }
}
