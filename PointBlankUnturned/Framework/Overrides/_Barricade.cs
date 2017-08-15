using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Barricade;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
    internal static class _Barricade
    {
        #region Reflection
        private static MethodInfo mi_askDamage = PointBlankReflect.GetMethod<Barricade>("askDamage", BindingFlags.Instance | BindingFlags.Public);
        private static MethodInfo mi_askRepair = PointBlankReflect.GetMethod<Barricade>("askRepair", BindingFlags.Instance | BindingFlags.Public);
        #endregion

        [Detour(typeof(Barricade), "askDamage", BindingFlags.Instance | BindingFlags.Public)]
        public static void askDamage(this Barricade barricade, ushort amount)
        {
            // Set the variables
            bool cancel = false;

            // Run the events
            BarricadeEvents.RunBarricadeDamage(UnturnedBarricade.FindBarricade(barricade), ref amount, ref cancel);

            // Run the original function
            if (!cancel)
                DetourManager.CallOriginal(mi_askDamage, barricade, amount);
        }

        [Detour(typeof(Barricade), "askRepair", BindingFlags.Instance | BindingFlags.Public)]
        public static void askRepair(this Barricade barricade, ushort amount)
        {
            // Set the variables
            bool cancel = false;

            // Run the events
            BarricadeEvents.RunBarricadeRepair(UnturnedBarricade.FindBarricade(barricade), ref amount, ref cancel);

            // Run the original function
            if (!cancel)
                DetourManager.CallOriginal(mi_askRepair, barricade, amount);
        }
    }
}
