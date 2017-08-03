using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.API.Unturned.Item
{
    public class UnturnedStoredItem
    {
        #region Properties
        // Information
        /// <summary>
        /// The item jar instance
        /// </summary>
        public ItemJar Jar { get; private set; }
        /// <summary>
        /// The item instance
        /// </summary>
        public SDG.Unturned.Item Item => Jar.item;
        /// <summary>
        /// The interactable item instance
        /// </summary>
        public InteractableItem Interactable => Jar.interactableItem;
        /// <summary>
        /// The asset of the item
        /// </summary>
        public ItemAsset Asset => Interactable.asset;

        // ItemJar information
        /// <summary>
        /// The rotation of the item inside the player's inventory
        /// </summary>
        public byte Rotation => Jar.rot;
        /// <summary>
        /// The size of the item in the inventory(X axis)
        /// </summary>
        public byte Size_X => Jar.size_x;
        /// <summary>
        /// The size of the item in the inventory(Y axis)
        /// </summary>
        public byte Size_Y => Jar.size_y;
        /// <summary>
        /// The X position of the item in the inventory
        /// </summary>
        public byte X => Jar.x;
        /// <summary>
        /// The Y position of the item in the inventory
        /// </summary>
        public byte Y => Jar.y;

        // Item information
        /// <summary>
        /// The item ID
        /// </summary>
        public ushort ID => Item.id;
        /// <summary>
        /// How durable is the item
        /// </summary>
        public byte Durability => Item.durability;

        // Asset information
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
        #endregion

        internal UnturnedStoredItem(ItemJar jar)
        {
            // Set the variables
            Jar = jar;
        }
    }
}