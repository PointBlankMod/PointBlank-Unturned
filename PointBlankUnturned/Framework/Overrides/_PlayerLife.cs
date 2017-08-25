﻿using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Steamworks;

namespace PointBlank.Framework.Overrides
{
    internal static class _PlayerLife
    {
        #region Reflection
        private static MethodInfo mi_doDamage = PointBlankReflect.GetMethod<PlayerLife>("doDamage", BindingFlags.NonPublic | BindingFlags.Instance);
        #endregion

        [Detour(typeof(PlayerLife), "doDamage", BindingFlags.NonPublic | BindingFlags.Instance)]
        private static void doDamage(this PlayerLife life, byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, out EPlayerKill kill)
        {
            UnturnedPlayer ply = UnturnedPlayer.Get(newKiller);
            bool cancel = false;
            kill = EPlayerKill.NONE;

            PlayerEvents.RunPlayerHurt(life.channel.owner, ref amount, ref newCause, ref newLimb, ref ply, ref cancel);
            
            if (cancel) return;
            
            object[] paramaters = new object[] { amount, newRagdoll, newCause, newLimb, newKiller, null };
            PointBlankDetourManager.CallOriginal(mi_doDamage, life, paramaters);
            kill = (EPlayerKill)paramaters[5];
        }
    }
}
