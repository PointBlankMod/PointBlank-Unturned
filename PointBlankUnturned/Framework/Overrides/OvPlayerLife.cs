using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Steamworks;

namespace PointBlank.Framework.Overrides
{
	internal static class OvPlayerLife
	{
        #region Reflection
        private static MethodInfo _miDoDamage = PointBlankReflect.GetMethod<SDG.Unturned.PlayerLife>("doDamage", BindingFlags.NonPublic | BindingFlags.Instance);
        #endregion

        [Detour(typeof(SDG.Unturned.PlayerLife), "doDamage", BindingFlags.NonPublic | BindingFlags.Instance)]
        private static void DoDamage(this SDG.Unturned.PlayerLife life, byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, out EPlayerKill kill)
        {
            UnturnedPlayer ply = UnturnedPlayer.Get(newKiller);
            bool cancel = false;
            kill = EPlayerKill.NONE;

            PlayerEvents.RunPlayerHurt(life.channel.owner, ref amount, ref newCause, ref newLimb, ref ply, ref cancel);
            
            if (cancel) return;
            
            object[] paramaters = new object[] { amount, newRagdoll, newCause, newLimb, newKiller, null };
            PointBlankDetourManager.CallOriginal(_miDoDamage, life, paramaters);
            kill = (EPlayerKill)paramaters[5];
        }
    }
}
