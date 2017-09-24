using System.Linq;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using UnityEngine;
using UStructure = SDG.Unturned.Structure;

namespace PointBlank.API.Unturned.Structure
{
    /// <summary>
    /// The unturned structure instance
    /// </summary>
    public class UnturnedStructure
    {
        #region Properties
        // Important information
        /// <summary>
        /// The structure data
        /// </summary>
        public StructureData Data { get; private set; }
        /// <summary>
        /// The structure object
        /// </summary>
        public UStructure Structure => Data.structure;

        // Structure data information
        /// <summary>
        /// The owner's steam64
        /// </summary>
        public ulong Owner => Data.owner;
        /// <summary>
        /// The group's steam64
        /// </summary>
        public ulong Group => Data.group;

        // Structure information
        /// <summary>
        /// The structure health
        /// </summary>
        public ushort Health
        {
            get => Structure.health;
            set
            {
                if (value == Health)
                    return;

                if (Health < value)
                    Repair((ushort)(value - Health));
                if (Health > value)
                    Damage((ushort)(Health - value));
            }
        }
        /// <summary>
        /// The structure ID
        /// </summary>
        public ushort Id => Structure.asset.id;
        /// <summary>
        /// The name of the structure
        /// </summary>
        public string Name => Structure.asset.itemName;
        /// <summary>
        /// The position of the structure
        /// </summary>
        public Vector3 Position => Structure.asset.structure.transform.position;
        /// <summary>
        /// The rotation of the structure
        /// </summary>
        public Quaternion Rotation => Structure.asset.structure.transform.rotation;
        /// <summary>
        /// ID of structure asset
        /// </summary>
        public ushort AssetId => Structure.asset.id;
        #endregion

        /// <summary>
        /// The unturned structure instance
        /// </summary>
        /// <param name="data">The structure data</param>
        private UnturnedStructure(StructureData data)
        {
            // Set the variables
            this.Data = data;

            // Run code
            UnturnedServer.AddStructure(this);
        }

        #region Static Functions
        /// <summary>
        /// Creates an unturned structure instance or returns an existing one
        /// </summary>
        /// <param name="data">The structure data</param>
        /// <returns>The unturned structure instance</returns>
        internal static UnturnedStructure Create(StructureData data)
        {
            UnturnedStructure stru = UnturnedServer.Structures.FirstOrDefault(a => a.Data == data);

            if (stru != null)
                return stru;
            return new UnturnedStructure(data);
        }

        /// <summary>
        /// Finds a structure based on the unturned structure instance
        /// </summary>
        /// <param name="structure">The unturned structure instance</param>
        /// <returns>The instance of the custom structure class</returns>
        public static UnturnedStructure FindStructure(UStructure structure) => UnturnedServer.Structures.FirstOrDefault(a => a.Structure == structure);

        /// <summary>
        /// Finds a structure based on the structure data
        /// </summary>
        /// <param name="data">The structure data</param>
        /// <returns>The unturned structure instance</returns>
        public static UnturnedStructure FindStructure(StructureData data) => UnturnedServer.Structures.FirstOrDefault(a => a.Data == data);
        #endregion

        #region Public Functions
        /// <summary>
        /// Damage the structure
        /// </summary>
        /// <param name="amount">The amount of damage to cause</param>
        public void Damage(float amount) => StructureManager.damage(Structure.asset.structure.transform, new Vector3(0, 0, 0), amount, 1, false);

        /// <summary>
        /// Repair the structure
        /// </summary>
        /// <param name="amount">The amount to repair it by</param>
        public void Repair(ushort amount) => Structure.askRepair(amount);

        /*/// <summary>
        /// Duplicate the structure
        /// </summary>
        public UnturnedStructure Duplicate()
        {
            UnturnedStructure Dupe = Create(new StructureData(new UStructure((ushort)(UnturnedServer.Structures.Length + 1), Structure.health, Structure.asset), Data.point, Data.angle_x, Data.angle_y, Data.angle_z, Data.owner, Data.group, Data.objActiveDate));

            return Dupe;
        }*/
        #endregion
    }
}
