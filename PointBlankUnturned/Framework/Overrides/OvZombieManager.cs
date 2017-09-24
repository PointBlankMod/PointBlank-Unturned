using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Zombie;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
    internal class OvZombieManager
    {
        #region Reflection
        private static MethodInfo _miSendZombieStun = PointBlankReflect.GetMethod<SDG.Unturned.ZombieManager>("sendZombieStun", PointBlankReflect.STATIC_FLAG);
        #endregion

        [Detour(typeof(SDG.Unturned.ZombieManager), "sendZombieStun", BindingFlags.Static | BindingFlags.Public)]
        public static void SendZombieStun(SDG.Unturned.Zombie zombie, byte stun)
        {
            ZombieEvents.RunZombieStun(UnturnedZombie.Create(zombie), ref stun);

            PointBlankDetourManager.CallOriginal(_miSendZombieStun, null, stun);
        }
    }
}
