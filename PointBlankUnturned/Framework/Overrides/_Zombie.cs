using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Zombie;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace PointBlank.Framework.Overrides
{
    internal static class _Zombie
    {
        #region Reflection
        private static MethodInfo mi_init = PointBlankReflect.GetMethod<Zombie>("init", PointBlankReflect.INSTANCE_FLAG);
        private static MethodInfo mi_stop = PointBlankReflect.GetMethod<Zombie>("stop", PointBlankReflect.INSTANCE_FLAG);
        private static MethodInfo mi_alert = PointBlankReflect.GetMethod<Zombie>("alert", PointBlankReflect.INSTANCE_FLAG);

        private static FieldInfo fi_ragdoll = PointBlankReflect.GetField<Zombie>("ragdoll", PointBlankReflect.INSTANCE_FLAG);
        private static FieldInfo fi_lastRegen = PointBlankReflect.GetField<Zombie>("lastRegen", PointBlankReflect.INSTANCE_FLAG);
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

        [Detour(typeof(Zombie), "askDamage", BindingFlags.Public | BindingFlags.Instance)]
        public static void askDamage(this Zombie zombie, ushort amount, Vector3 newRagdoll, out EPlayerKill kill, out uint xp, bool trackKill = true, bool dropLoot = true)
        {
            kill = EPlayerKill.NONE;
            xp = 0u;
            UnturnedZombie zmb = UnturnedZombie.Create(zombie);
            bool cancel = false;

            if (amount == 0 || zombie.isDead)
                return;
            if (!zombie.isDead)
            {
                if (ZombieManager.regions[(int)zombie.bound].hasBeacon)
                    amount = (ushort)((byte)Mathf.CeilToInt((float)amount / ((float)Mathf.Max(1, BeaconManager.checkBeacon(zombie.bound).initialParticipants) * 1.5f)));
                ZombieEvents.RunZombieDamage(zmb, UnturnedServer.Players.FirstOrDefault(a => a.Movement.bound == zombie.bound), ref amount, ref cancel);
                if (cancel)
                    return;
                if (amount >= zmb.Health)
                    zmb.Health = 0;
                else
                    zmb.Health -= amount;
                fi_ragdoll.SetValue(zombie, newRagdoll);

                if (zmb.Health == 0)
                {
                    if (zombie.isMega)
                        kill = EPlayerKill.MEGA;
                    else
                        kill = EPlayerKill.ZOMBIE;

                    ZombieEvents.RunZombieKill(zmb, ref cancel);
                    if (cancel)
                        return;
                    xp = LevelZombies.tables[(int)zombie.type].xp;
                    if (ZombieManager.regions[(int)zombie.bound].hasBeacon)
                    {
                        xp = (uint)(xp * Provider.modeConfigData.Zombies.Full_Moon_Experience_Multiplier);
                    }
                    else
                    {
                        if (LightingManager.isFullMoon)
                            xp = (uint)(xp * Provider.modeConfigData.Zombies.Full_Moon_Experience_Multiplier);
                        if (dropLoot)
                            ZombieManager.dropLoot(zombie);
                    }
                    ZombieManager.sendZombieDead(zombie, fi_ragdoll.GetValue<Vector3>(zombie));
                    if (zombie.isRadioactive)
                        DamageTool.explode(zombie.transform.position + new Vector3(0f, 0.25f, 0f), 2f, EDeathCause.ACID, CSteamID.Nil, 20f, 0f, 20f, 0f, 0f, 0f, 0f, 0f, EExplosionDamageType.ZOMBIE_ACID, 2f, true);
                    if (zombie.speciality == EZombieSpeciality.BURNER || zombie.speciality == EZombieSpeciality.BOSS_FIRE || zombie.speciality == EZombieSpeciality.BOSS_MAGMA)
                        DamageTool.explode(zombie.transform.position + new Vector3(0f, 0.25f, 0f), 4f, EDeathCause.BURNER, CSteamID.Nil, 40f, 0f, 40f, 0f, 0f, 0f, 0f, 0f, EExplosionDamageType.ZOMBIE_FIRE, 4f, true);
                    if (trackKill)
                    {
                        for (int i = 0; i < Provider.clients.Count; i++)
                        {
                            SteamPlayer steamPlayer = Provider.clients[i];
                            if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead)
                                if (steamPlayer.player.movement.bound == zombie.bound)

                                    steamPlayer.player.quests.trackZombieKill(zombie);
                        }
                    }
                }
                else if (Provider.modeConfigData.Zombies.Can_Stun && amount > ((!zombie.isMega) ? 20 : 150))
                    zmb.Stun();
                fi_lastRegen.SetValue(zombie, Time.time);
            }
        }
    }
}
