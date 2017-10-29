using System.Linq;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;

namespace PointBlank.API.Unturned.Animal
{
    /// <summary>
    /// The unturned animal API
    /// </summary>
    public class UnturnedAnimal
    {
        #region Properties
        /// <summary>
        /// The unturned animal instance
        /// </summary>
        public SDG.Unturned.Animal Animal { get; private set; }
        /// <summary>
        /// The animal asset
        /// </summary>
        public AnimalAsset Asset => Animal.asset;

        // Animal information
        /// <summary>
        /// The animal ID
        /// </summary>
        public ushort ID => Animal.id;
        /// <summary>
        /// The health of the animal
        /// </summary>
        public ushort Health
        {
            get => PointBlankReflect.GetField<SDG.Unturned.Animal>("health", PointBlankReflect.INSTANCE_FLAG).GetValue<ushort>(Animal);
            set => PointBlankReflect.GetField<SDG.Unturned.Animal>("health", PointBlankReflect.INSTANCE_FLAG).SetValue(Animal, value);
        }
        /// <summary>
        /// Is the animal fleeing
        /// </summary>
        public bool IsFleeing => Animal.isFleeing;
        /// <summary>
        /// Is the animal wandering
        /// </summary>
        public bool IsWandering => !Animal.isFleeing;
        /// <summary>
        /// Is the animal dead
        /// </summary>
        public bool IsDead => Animal.isDead;

        // Asset information
        /// <summary>
        /// The name of the animal
        /// </summary>
        public string Name => Asset.animalName;
        /// <summary>
        /// The category of the animal
        /// </summary>
        public EAssetType Category => Asset.assetCategory;
        /// <summary>
        /// The animal behaviour
        /// </summary>
        public EAnimalBehaviour Behaviour => Asset.behaviour;
        /// <summary>
        /// The max health of the animal
        /// </summary>
        public ushort MaxHealth => Asset.health;
        #endregion

        private UnturnedAnimal(SDG.Unturned.Animal animal)
        {
            // Set the variables
            Animal = animal;

            // Run the code
            UnturnedServer.AddAnimal(this);
        }

        #region Static Functions
        internal static UnturnedAnimal Create(SDG.Unturned.Animal animal)
        {
            UnturnedAnimal ani = UnturnedServer.Animals.FirstOrDefault(a => a.Animal == animal);

            return ani ?? new UnturnedAnimal(animal);
        }
        #endregion
    }
}
