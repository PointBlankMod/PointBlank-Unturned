using System.Linq;
using PointBlank.API.Unturned.Server;
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
        /// The interactable item instance of the item
        /// </summary>
        public InteractableItem Interactable { get; private set; }
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
        public ushort Id => Item.id;
        /// <summary>
        /// How durable is the item
        /// </summary>
        public byte Durability => Item.durability;
        #endregion

        private UnturnedItem(InteractableItem item)
        {
            // Set the variables
            Interactable = item;

            // Run code
            UnturnedServer.AddItem(this);
        }

        #region Static Functions
        internal static UnturnedItem Create(InteractableItem item)
        {
            UnturnedItem itm = UnturnedServer.Items.FirstOrDefault(a => a.Interactable == item);

            if (itm != null)
                return itm;
            return new UnturnedItem(item);
        }
        #endregion
    }
}
