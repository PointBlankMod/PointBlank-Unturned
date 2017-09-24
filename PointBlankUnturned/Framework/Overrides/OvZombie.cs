using System.Reflection;
using System.Linq;
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
	internal static class OvZombie
	{
        #region Reflection
        private static MethodInfo _miInit = PointBlankReflect.GetMethod<SDG.Unturned.Zombie>("init", PointBlankReflect.INSTANCE_FLAG);
        private static MethodInfo _miStop = PointBlankReflect.GetMethod<SDG.Unturned.Zombie>("stop", PointBlankReflect.INSTANCE_FLAG);
        private static MethodInfo _miAlert = PointBlankReflect.GetMethod<SDG.Unturned.Zombie>("alert", PointBlankReflect.INSTANCE_FLAG);

        private static FieldInfo _fiRagdoll = PointBlankReflect.GetField<SDG.Unturned.Zombie>("ragdoll", PointBlankReflect.INSTANCE_FLAG);
        private static FieldInfo _fiLastRegen = PointBlankReflect.GetField<SDG.Unturned.Zombie>("lastRegen", PointBlankReflect.INSTANCE_FLAG);
        #endregion

        [Detour(typeof(SDG.Unturned.Zombie), "init", BindingFlags.Instance | BindingFlags.Public)]
        public static void Init(this SDG.Unturned.Zombie zombie)
        {
            ServerEvents.RunZombieCreated(zombie);

            PointBlankDetourManager.CallOriginal(_miInit, zombie);
        }

        [Detour(typeof(SDG.Unturned.Zombie), "stop", BindingFlags.NonPublic | BindingFlags.Instance)]
        public static void Stop(this SDG.Unturned.Zombie zombie)
        {
            ServerEvents.RunZombieRemoved(UnturnedZombie.Create(zombie));

            PointBlankDetourManager.CallOriginal(_miStop, zombie);
        }

        [Detour(typeof(SDG.Unturned.Zombie), "alert", BindingFlags.Public | BindingFlags.Instance)]
        public static void Alert(this SDG.Unturned.Zombie zombie, Player player)
        {
            bool cancel = false;
            UnturnedPlayer ply = UnturnedPlayer.Get(player);

            ZombieEvents.RunZombieAlert(UnturnedZombie.Create(zombie), ref ply, ref cancel);
            if (cancel)
                return;
            player = ply.Player;
            PointBlankDetourManager.CallOriginal(_miAlert, zombie, player);
        }

        [Detour(typeof(SDG.Unturned.Zombie), "askDamage", BindingFlags.Public | BindingFlags.Instance)]
        public static void AskDamage(this SDG.Unturned.Zombie zombie, ushort amount, Vector3 newRagdoll, out EPlayerKill kill, out uint xp, bool trackKill = true, bool dropLoot = true)
        {
            kill = EPlayerKill.NONE;
            xp = 0u;
            UnturnedZombie zmb = UnturnedZombie.Create(zombie);
            bool cancel = false;

            if (amount == 0 || zombie.isDead)
                return;
            if (!zombie.isDead)
            {
                if (SDG.Unturned.ZombieManager.regions[(int)zombie.bound].hasBeacon)
                    amount = (ushort)((byte)Mathf.CeilToInt((float)amount / ((float)Mathf.Max(1, BeaconManager.checkBeacon(zombie.bound).initialParticipants) * 1.5f)));
                ZombieEvents.RunZombieDamage(zmb, UnturnedServer.Players.FirstOrDefault(a => a.Movement.bound == zombie.bound), ref amount, ref cancel);
                if (cancel)
                    return;
                if (amount >= zmb.Health)
                    zmb.Health = 0;
                else
                    zmb.Health -= amount;
                _fiRagdoll.SetValue(zombie, newRagdoll);

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
                    if (SDG.Unturned.ZombieManager.regions[(int)zombie.bound].hasBeacon)
                    {
                        xp = (uint)(xp * SDG.Unturned.Provider.modeConfigData.Zombies.Full_Moon_Experience_Multiplier);
                    }
                    else
                    {
                        if (LightingManager.isFullMoon)
                            xp = (uint)(xp * SDG.Unturned.Provider.modeConfigData.Zombies.Full_Moon_Experience_Multiplier);
                        if (dropLoot)
                            SDG.Unturned.ZombieManager.dropLoot(zombie);
                    }
                    SDG.Unturned.ZombieManager.sendZombieDead(zombie, _fiRagdoll.GetValue<Vector3>(zombie));
                    if (zombie.isRadioactive)
                        DamageTool.explode(zombie.transform.position + new Vector3(0f, 0.25f, 0f), 2f, EDeathCause.ACID, CSteamID.Nil, 20f, 0f, 20f, 0f, 0f, 0f, 0f, 0f, EExplosionDamageType.ZOMBIE_ACID, 2f, true);
                    if (zombie.speciality == EZombieSpeciality.BURNER || zombie.speciality == EZombieSpeciality.BOSS_FIRE || zombie.speciality == EZombieSpeciality.BOSS_MAGMA)
                        DamageTool.explode(zombie.transform.position + new Vector3(0f, 0.25f, 0f), 4f, EDeathCause.BURNER, CSteamID.Nil, 40f, 0f, 40f, 0f, 0f, 0f, 0f, 0f, EExplosionDamageType.ZOMBIE_FIRE, 4f, true);
                    if (trackKill)
                    {
                        for (int i = 0; i < SDG.Unturned.Provider.clients.Count; i++)
                        {
                            SteamPlayer steamPlayer = SDG.Unturned.Provider.clients[i];
                            if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead)
                                if (steamPlayer.player.movement.bound == zombie.bound)

                                    steamPlayer.player.quests.trackZombieKill(zombie);
                        }
                    }
                }
                else if (SDG.Unturned.Provider.modeConfigData.Zombies.Can_Stun && amount > ((!zombie.isMega) ? 20 : 150))
                    zmb.Stun();
                _fiLastRegen.SetValue(zombie, Time.time);
            }
        }
    }
}
