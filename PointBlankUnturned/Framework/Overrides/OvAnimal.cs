using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Animal;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.Framework.Overrides
{
	internal static class OvAnimal
	{
        #region Reflection
        private static MethodInfo _miStop = PointBlankReflect.GetMethod<Animal>("stop", PointBlankReflect.INSTANCE_FLAG);
        private static MethodInfo _miAskPanic = PointBlankReflect.GetMethod<Animal>("askPanic", PointBlankReflect.INSTANCE_FLAG);

        private static FieldInfo _fiRagdoll = PointBlankReflect.GetField<Animal>("ragdoll", PointBlankReflect.INSTANCE_FLAG);
        private static FieldInfo _fiLastRegen = PointBlankReflect.GetField<Animal>("lastRegen", PointBlankReflect.INSTANCE_FLAG);
        #endregion

        [Detour(typeof(Animal), "stop", BindingFlags.NonPublic | BindingFlags.Instance)]
        private static void Stop(this Animal animal)
        {
            ServerEvents.RunAnimalRemoved(UnturnedAnimal.Create(animal));

            PointBlankDetourManager.CallOriginal(_miStop, animal);
        }

        [Detour(typeof(Animal), "askDamage", BindingFlags.Public | BindingFlags.Instance)]
        public static void AskDamage(this Animal animal, byte amount, Vector3 newRagdoll, out EPlayerKill kill, out uint xp)
        {
            kill = EPlayerKill.NONE;
            xp = 0u;
            UnturnedAnimal ani = UnturnedAnimal.Create(animal);
            bool cancel = false;

            AnimalEvents.RunAnimalDamage(ani, ref amount, ref cancel);
            if (cancel)
                return;
            if (amount == 0 || animal.isDead)
                return;
            if (!animal.isDead)
            {
                if ((ushort)amount >= ani.Health)
                    ani.Health = 0;
                else
                    ani.Health -= (ushort)amount;
                _fiRagdoll.SetValue(animal, newRagdoll);

                if (ani.Health == 0)
                {
                    AnimalEvents.RunAnimalDie(ani, ref cancel);
                    if (cancel)
                        return;
                    kill = EPlayerKill.ANIMAL;
                    if (animal.asset != null)
                        xp = animal.asset.rewardXP;

                    AnimalManager.dropLoot(animal);
                    AnimalManager.sendAnimalDead(animal, _fiRagdoll.GetValue<Vector3>(animal));
                }
                else if (animal.asset != null && animal.asset.panics != null && animal.asset.panics.Length > 0)
                {
                    AnimalManager.sendAnimalPanic(animal);
                }
                _fiLastRegen.SetValue(animal, Time.time);
            }
        }

        [Detour(typeof(Animal), "askPanic", BindingFlags.Public | BindingFlags.Instance)]
        public static void AskPanic(this Animal animal)
        {
            bool cancel = false;

            AnimalEvents.RunAnimalPanic(UnturnedAnimal.Create(animal), ref cancel);
            if (!cancel)
                PointBlankDetourManager.CallOriginal(_miAskPanic, animal);
        }
    }
}
