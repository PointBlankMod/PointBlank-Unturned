using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Zombie;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
    internal static class _Zombie
    {
        #region Reflection
        private static MethodInfo mi_init = PointBlankReflect.GetMethod<Zombie>("init", PointBlankReflect.INSTANCE_FLAG);
        private static MethodInfo mi_stop = PointBlankReflect.GetMethod<Zombie>("stop", PointBlankReflect.INSTANCE_FLAG);
        private static MethodInfo mi_alert = PointBlankReflect.GetMethod<Zombie>("alert", PointBlankReflect.INSTANCE_FLAG);
        #endregion

        [Detour(typeof(Zombie), "init", BindingFlags.Instance | BindingFlags.Public)]
        public static void init(this Zombie zombie)
        {
            ServerEvents.RunZombieCreated(zombie);

            DetourManager.CallOriginal(mi_init, zombie);
        }

        [Detour(typeof(Zombie), "stop", BindingFlags.NonPublic | BindingFlags.Instance)]
        public static void stop(this Zombie zombie)
        {
            ServerEvents.RunZombieRemoved(UnturnedZombie.Create(zombie));

            DetourManager.CallOriginal(mi_stop, zombie);
        }

        [Detour(typeof(Zombie), "alert", BindingFlags.Public | BindingFlags.Instance)]
        public static void alert(this Zombie zombie, Player player)
        {
            bool cancel = false;
            UnturnedPlayer ply = UnturnedPlayer.Get(player);

            ZombieEvents.RunZombieAlert(UnturnedZombie.Create(zombie), ref ply, ref cancel);
            if (cancel)
                return;
            player = ply.Player;
            DetourManager.CallOriginal(mi_alert, zombie, player);
        }
    }
}
