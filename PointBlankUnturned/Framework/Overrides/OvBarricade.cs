using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Barricade;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
	internal static class OvBarricade
	{
        #region Reflection
        private static MethodInfo _miAskDamage = PointBlankReflect.GetMethod<SDG.Unturned.Barricade>("askDamage", BindingFlags.Instance | BindingFlags.Public);
        private static MethodInfo _miAskRepair = PointBlankReflect.GetMethod<SDG.Unturned.Barricade>("askRepair", BindingFlags.Instance | BindingFlags.Public);
        #endregion

        [Detour(typeof(SDG.Unturned.Barricade), "askDamage", BindingFlags.Instance | BindingFlags.Public)]
        public static void AskDamage(this SDG.Unturned.Barricade barricade, ushort amount)
        {
            // Set the variables
            bool cancel = false;

            // Run the events
            BarricadeEvents.RunBarricadeDamage(UnturnedBarricade.FindBarricade(barricade), ref amount, ref cancel);

            // Run the original function
            if (!cancel)
                PointBlankDetourManager.CallOriginal(_miAskDamage, barricade, amount);
        }

        [Detour(typeof(SDG.Unturned.Barricade), "askRepair", BindingFlags.Instance | BindingFlags.Public)]
        public static void AskRepair(this SDG.Unturned.Barricade barricade, ushort amount)
        {
            // Set the variables
            bool cancel = false;

            // Run the events
            BarricadeEvents.RunBarricadeRepair(UnturnedBarricade.FindBarricade(barricade), ref amount, ref cancel);

            // Run the original function
            if (!cancel)
                PointBlankDetourManager.CallOriginal(_miAskRepair, barricade, amount);
        }
    }
}
