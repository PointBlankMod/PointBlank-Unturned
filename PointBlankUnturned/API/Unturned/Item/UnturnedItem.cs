using SDG.Unturned;
using UnityEngine;

namespace PointBlank.API.Unturned.Item
{
    /// <summary>
    /// The unturned item that is located somewhere in the world
    /// </summary>
    public class UnturnedItem
    {
        #region Properties
        // Important info
        /// <summary>
        /// The drop instance
        /// </summary>
        public ItemDrop Drop { get; private set; }
        /// <summary>
        /// The interactable item instance of the item
        /// </summary>
        public InteractableItem Interactable => Drop.interactableItem;
        /// <summary>
        /// The asset of the item
        /// </summary>
        public ItemAsset Asset => Interactable.asset;
        /// <summary>
        /// The jar of the item
        /// </summary>
        public ItemJar Jar
        {
            get => Interactable.jar;
            set => Interactable.jar = value;
        }
        /// <summary>
        /// The item instance of the item drop
        /// </summary>
        public SDG.Unturned.Item Item
        {
            get => Interactable.item;
            set => Interactable.item = value;
        }

        // Basic information
        /// <summary>
        /// The transform of the item model
        /// </summary>
        public Transform Model => Drop.model;
        /// <summary>
        /// The instance ID of the item
        /// </summary>
        public uint InstanceID => Drop.instanceID;

        // Asset information
        /// <summary>
        /// The gameobject of the asset
        /// </summary>
        public GameObject GameObject => Asset.item;
        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name => Asset.itemName;
        /// <summary>
        /// The description of the item
        /// </summary>
        public string Description => Asset.itemDescription;
        /// <summary>
        /// The asset category of the item
        /// </summary>
        public EAssetType AssetCategory => Asset.assetCategory;
        /// <summary>
        /// The type of item
        /// </summary>
        public EItemType ItemType => Asset.type;
        /// <summary>
        /// The slot type of the item
        /// </summary>
        public ESlotType SlotType => Asset.slot;
        /// <summary>
        /// How rare is the item
        /// </summary>
        public EItemRarity ItemRarity => Asset.rarity;

        // Item information
        /// <summary>
        /// The item ID
        /// </summary>
        public ushort ID => Item.id;
        /// <summary>
        /// How durable is the item
        /// </summary>
        public byte Durability => Item.durability;
        #endregion Properties

        internal UnturnedItem(ItemDrop drop)
        {
            // Set the variables
            Drop = drop;
        }
    }
}
