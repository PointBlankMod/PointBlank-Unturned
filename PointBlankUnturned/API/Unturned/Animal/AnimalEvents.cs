namespace PointBlank.API.Unturned.Animal
{
    /// <summary>
    /// All the events for the unturned animal
    /// </summary>
    public static class AnimalEvents
    {
        #region Handlers
        /// <summary>
        /// Handles animal panic events
        /// </summary>
        /// <param name="animal">The affected animal</param>
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void AnimalPanicHandler(UnturnedAnimal animal, ref bool cancel);

        /// <summary>
        /// Handles animal damages
        /// </summary>
        /// <param name="animal">The affected animal</param>
        /// <param name="amount">The amount of damage caused</param>
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void AnimalDamageHandler(UnturnedAnimal animal, ref byte amount, ref bool cancel);
        /// <summary>
        /// Handles animal deaths
        /// </summary>
        /// <param name="animal">The affected animal</param>
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void AnimalDieHandler(UnturnedAnimal animal, ref bool cancel);
        #endregion

        #region Events
        /// <summary>
        /// Called when an animal panics
        /// </summary>
        public static event AnimalPanicHandler OnAnimalPanic;

        /// <summary>
        /// Called when an animal is damaged
        /// </summary>
        public static event AnimalDamageHandler OnAnimalDamage;
        /// <summary>
        /// Called when an animal dies
        /// </summary>
        public static event AnimalDieHandler OnAnimalDie;
        #endregion

        #region Functions
        internal static void RunAnimalPanic(UnturnedAnimal animal, ref bool cancel) => OnAnimalPanic?.Invoke(animal, ref cancel);

        internal static void RunAnimalDamage(UnturnedAnimal animal, ref byte amount, ref bool cancel) => OnAnimalDamage?.Invoke(animal, ref amount, ref cancel);
        internal static void RunAnimalDie(UnturnedAnimal animal, ref bool cancel) => OnAnimalDie?.Invoke(animal, ref cancel);
        #endregion
    }
}
