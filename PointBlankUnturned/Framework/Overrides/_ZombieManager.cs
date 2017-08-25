using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Zombie;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
    internal class _ZombieManager
    {
        #region Reflection
        private static MethodInfo mi_sendZombieStun = PointBlankReflect.GetMethod<ZombieManager>("sendZombieStun", PointBlankReflect.STATIC_FLAG);
        #endregion

        [Detour(typeof(ZombieManager), "sendZombieStun", BindingFlags.Static | BindingFlags.Public)]
        public static void sendZombieStun(Zombie zombie, byte stun)
        {
            ZombieEvents.RunZombieStun(UnturnedZombie.Create(zombie), ref stun);

            PointBlankDetourManager.CallOriginal(mi_sendZombieStun, null, stun);
        }
    }
}
