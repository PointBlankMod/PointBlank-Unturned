using System.Reflection;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.Framework.Overrides
{
    internal class _AnimalManager
    {
        [Detour(typeof(AnimalManager), "addAnimal", BindingFlags.NonPublic | BindingFlags.Instance)]
        private Animal addAnimal(ushort id, Vector3 point, float angle, bool isDead)
        {
            AnimalAsset animalAsset = (AnimalAsset)Assets.find(EAssetType.ANIMAL, id);

            if (animalAsset != null)
            {
                Transform transform;
                if (Dedicator.isDedicated)
                    transform = UnityEngine.Object.Instantiate<GameObject>(animalAsset.dedicated).transform;
                else if (Provider.isServer)
                    transform = UnityEngine.Object.Instantiate<GameObject>(animalAsset.server).transform;
                else
                    transform = UnityEngine.Object.Instantiate<GameObject>(animalAsset.client).transform;
                transform.name = id.ToString();
                transform.parent = LevelAnimals.models;
                transform.position = point;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Animal animal = transform.gameObject.AddComponent<Animal>();
                animal.index = (ushort)AnimalManager.animals.Count;
                animal.id = id;
                animal.isDead = isDead;

                AnimalManager.animals.Add(animal);
                ServerEvents.RunAnimalCreated(animal);
                return animal;
            }
            return null;
        }
    }
}
