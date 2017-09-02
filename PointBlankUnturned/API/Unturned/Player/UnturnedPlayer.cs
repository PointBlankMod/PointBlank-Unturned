﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using PointBlank.API.Implements;
using PointBlank.API.Player;
using PointBlank.API.Permissions;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Vehicle;
using PointBlank.API.Unturned.Item;
using SP = PointBlank.API.Steam.SteamPlayer;
using RG = PointBlank.API.Steam.SteamGroup;
using CM = PointBlank.API.Unturned.Chat.UnturnedChat;
using UPlayer = SDG.Unturned.Player;
using SPlayer = SDG.Unturned.SteamPlayer;

namespace PointBlank.API.Unturned.Player
{
    /// <summary>
    /// The unturned player instance
    /// </summary>
    public class UnturnedPlayer : PointBlankPlayer
    {
        #region Variables
        private List<UnturnedPlayer> _PlayerList = new List<UnturnedPlayer>();
        private List<string> _Prefixes = new List<string>();
        private List<string> _Suffixes = new List<string>();

        private readonly FieldInfo _fiItems = typeof(Items).GetField("items", BindingFlags.Instance | BindingFlags.NonPublic);
        #endregion

        #region Properties
        // Important information
        /// <summary>
        /// The unturned player instance
        /// </summary>
        public UPlayer Player => SteamPlayer.player;
        /// <summary>
        /// The steam player instance
        /// </summary>
        public SPlayer SteamPlayer { get; private set; }
        /// <summary>
        /// The steam player ID instance
        /// </summary>
        public SteamPlayerID SteamPlayerID => SteamPlayer.playerID;
        /// <summary>
        /// The steam information about the player
        /// </summary>
        public SP Steam { get; private set; }
        /// <summary>
        /// The movement instance of the player
        /// </summary>
        public PlayerMovement Movement => Player.movement;
        /// <summary>
        /// The life instance of the player
        /// </summary>
        public PlayerLife Life => Player.life;
        /// <summary>
        /// The look instance of the player
        /// </summary>
        public PlayerLook Look => Player.look;
        /// <summary>
        /// The clothing instance of the player
        /// </summary>
        public PlayerClothing Clothing => Player.clothing;
        /// <summary>
        /// The inventory instance of the player
        /// </summary>
        public PlayerInventory Inventory => Player.inventory;
        /// <summary>
        /// The equipment instance of the player
        /// </summary>
        public PlayerEquipment Equipment => Player.equipment;
        /// <summary>
        /// The stance instance of the player
        /// </summary>
        public PlayerStance Stance => Player.stance;
        /// <summary>
        /// The steam channel instance of the player
        /// </summary>
        public SteamChannel Channel => Player.channel;
        /// <summary>
        /// The skills instance of the player
        /// </summary>
        public PlayerSkills USkills => Player.skills;

        // Steam player ID information
        /// <summary>
        /// The player's name
        /// </summary>
        public string PlayerName => SteamPlayerID.playerName;
        /// <summary>
        /// The character's name
        /// </summary>
        public string CharacterName
        {
            get => SteamPlayerID.characterName;
            set => SteamPlayerID.characterName = value;
        }
        /// <summary>
        /// The character name without modifications
        /// </summary>
        public string UnturnedCharacterName { get; internal set; }
        /// <summary>
        /// The player's steam ID
        /// </summary>
        public CSteamID SteamID => SteamPlayerID.steamID;
        /// <summary>
        /// The character's ID
        /// </summary>
        public byte CharacterID
        {
            get => SteamPlayerID.characterID;
            set => SteamPlayerID.characterID = value;
        }
        /// <summary>
        /// The player's nick name
        /// </summary>
        public string NickName
        {
            get => SteamPlayerID.nickName;
            set => SteamPlayerID.nickName = value;
        }
        /// <summary>
        /// The nickname without modifications
        /// </summary>
        public string UnturnedNickName { get; internal set; }

        // Steam player information
        /// <summary>
        /// Is the player an admin
        /// </summary>
        public override bool IsAdmin
        {
            get => SteamPlayer.isAdmin;
            set
            {
                if (value)
                    SteamAdminlist.admin(SteamID, CSteamID.Nil);
                else
                    SteamAdminlist.unadmin(SteamID);
            }
        }
        /// <summary>
        /// player ping
        /// </summary>
        public float Ping => SteamPlayer.ping;
        /// <summary>
        /// Is the player a pro buyer
        /// </summary>
        public bool IsPro => SteamPlayer.isPro;
        /// <summary>
        /// The player skillset
        /// </summary>
        public EPlayerSkillset Skillset => SteamPlayer.skillset;
        /// <summary>
        /// Is the player muted
        /// </summary>
        public bool IsMuted 
        {
            get => SteamPlayer.isMuted;
            set => SteamPlayer.isMuted = value;
        }
        /// <summary>
        /// Is the player the owner
        /// </summary>
        public bool IsOwner => SteamAdminlist.ownerID == SteamID;

        // Player information
        /// <summary>
        /// The player's health
        /// </summary>
        public byte Health
        {
            get => Life.health;
            set
            {
                // Do checks
                if (Life.isDead)
                    return;
                if (value < 1)
                    Life.sendSuicide();

                // Set data
                typeof(PlayerLife).GetField("_health", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Life, value);

                // Send update
                Life.channel.send("tellHealth", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Health
                });
            }
        }
        /// <summary>
        /// The player's stamina
        /// </summary>
        public byte Stamina => Life.stamina;
        /// <summary>
        /// The player's food/hunger
        /// </summary>
        public byte Food
        {
            get => Life.food;
            set
            {
                // Checks
                if (Life.isDead)
                    return;
                if (value < 0)
                    return;

                // Data
                Life.tellFood(Provider.server, value);

                // Update
                Life.channel.send("tellFood", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Food
                });
            }
        }
        /// <summary>
        /// The player's water/thirst
        /// </summary>
        public byte Thirst
        {
            get => Life.water;
            set
            {
                // Checks
                if (Life.isDead)
                    return;
                if (value < 0)
                    return;

                // Data
                Life.tellWater(Provider.server, value);

                // Update
                Life.channel.send("tellWater", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Thirst
                });
            }
        }
        /// <summary>
        /// The player's virus/infection
        /// </summary>
        public byte Virus
        {
            get => Life.virus;
            set
            {
                // Checks
                if (Life.isDead)
                    return;
                if (value < 0)
                    return;

                // Data
                Life.tellVirus(Provider.server, value);

                // Update
                Life.channel.send("tellVirus", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Virus
                });
            }
        }
        /// <summary>
        /// Are the player's legs broken
        /// </summary>
        public bool IsBroken
        {
            get => Life.isBroken;
            set
            {
                // Checks
                if (Life.isDead)
                    return;
                if (value == IsBroken)
                    return;

                // Data
                Life.tellBroken(Provider.server, value);

                // Update
                Life.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    IsBroken
                });
            }
        }
        /// <summary>
        /// Is the player bleeding
        /// </summary>
        public bool IsBleeding
        {
            get => Life.isBleeding;
            set
            {
                // Checks
                if (Life.isDead)
                    return;
                if (value == IsBleeding)
                    return;

                // Data
                Life.tellBleeding(Provider.server, value);

                // Update
                Life.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    IsBleeding
                });
            }
        }
        /// <summary>
        /// Is the player dead
        /// </summary>
        public bool IsDead => Life.isDead;
        /// <summary>
        /// The player's skill boost
        /// </summary>
        public EPlayerBoost SkillBoost
        {
            get => Player.skills.boost;
            set
            {
                // Data
                USkills.tellBoost(Provider.server, (byte)value);

                // Update
                Player.skills.channel.send("tellBoost", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    SkillBoost
                });
            }
        }
        /// <summary>
        /// The player's skill experience
        /// </summary>
        public uint Experience
        {
            get => USkills.experience;
            set
            {
                // Data
                USkills.tellExperience(Provider.server, value);

                // Update
                Player.skills.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Experience
                });
            }
        }
        /// <summary>
        /// Checks if player is in vehicle
        /// </summary>
        public bool IsInVehicle => Movement.getVehicle() != null;
        /// <summary>
        /// The player current vehicle
        /// </summary>
        public UnturnedVehicle Vehicle => UnturnedVehicle.Create(Movement.getVehicle());
        /// <summary>
        /// The player's reputation
        /// </summary>
        public int Reputation
        {
            get => Player.skills.reputation;
            set
            {
                // Data
                USkills.tellReputation(Provider.server, value);

                // Update
                Player.skills.channel.send("tellReputation", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Reputation
                });
            }
        }
        /// <summary>
        /// The player's skills
        /// </summary>
        public Skill[][] Skills => Player.skills.skills;
        /// <summary>
        /// Has the player got anything equipped
        /// </summary>
        public bool HasAnythingEquipped => (Equipment.isEquipped && Equipment.asset != null && Equipment.useable != null);
        /// <summary>
        /// The currently equipped asset
        /// </summary>
        public ItemAsset EquippedAsset => Equipment.asset;
        /// <summary>
        /// The currently equpped useable
        /// </summary>
        public Useable EquippedUseable => Equipment.useable;
        /// <summary>
        /// The currently equipped item ID
        /// </summary>
        public ushort EquippedItemID => Equipment.itemID;
        /// <summary>
        /// Is the currently equpped item a primary
        /// </summary>
        public bool IsEquippedPrimary => Equipment.primary;
        /// <summary>
        /// Is the current equipment busy
        /// </summary>
        public bool IsEquipmentBusy
        {
            get => Equipment.isBusy;
            set => Equipment.isBusy = value;
        }
        /// <summary>
        /// Can the equipped item be inspected
        /// </summary>
        public bool CanInspectEquipped => Equipment.canInspect;
        /// <summary>
        /// Is the player currently inspecting the equipped item
        /// </summary>
        public bool IsInspectingEquipped => Equipment.isInspecting;
        /// <summary>
        /// Current position of the player
        ///</summary>
        public Vector3 Position => Player.transform.position;
        /// <summary>
        /// Current rotation of the player
        ///</summary>
        public Quaternion Rotation => Player.transform.rotation;
        /// <summary>
        /// Is the player moving
        ///</summary>
        public bool IsMoving => Movement.isMoving;
        /// <summary>
        /// Is the player grounded
        ///</summary>
        public bool IsGrounded => Movement.isGrounded;
        /// <summary>
        /// Is the player safe
        ///</summary>
        public bool IsSafe => Movement.isSafe;
        /// <summary>
        /// Is the player loading (Thanks Trojaner)
        ///</summary>
        public bool IsLoading => Provider.pending.Contains(Provider.pending.Find((c) => c.playerID.steamID == SteamID));
        /// <summary>
        /// IP of the player
        ///</summary>
        public string IP => Parser.getIPFromUInt32(SteamIP);
        /// <summary>
        /// The steam defined IP of the player
        /// </summary>
        public uint SteamIP
        {
            get
            {
                SteamGameServerNetworking.GetP2PSessionState(SteamID, out P2PSessionState_t state);

                return state.m_nRemoteIP;
            }
        }
        /// <summary>
        /// Array of items in the player's inventory
        ///</summary>
        public UnturnedStoredItem[] Items
        {
            get
            {
                List<UnturnedStoredItem> retval = new List<UnturnedStoredItem>();
                Inventory.items.ForEach((items) =>
                {
                    ((List<ItemJar>)_fiItems.GetValue(items)).For((i, item) =>
                    {
                        retval.Add(new UnturnedStoredItem(item, items, (byte)i, this));
                    });
                });
                return retval.ToArray();
            }
        }
        /// <summary>
        /// Is the player member of a quest group
        /// </summary>
        public bool IsInQuestGroup => Player.quests.isMemberOfAGroup;
        /// <summary>
        /// The ID of the quest group
        /// </summary>
        public CSteamID QuestGroupID => Player.quests.groupID;
        /// <summary>
        /// Vehicles the player has locked
        ///</summary>
        public UnturnedVehicle[] LockedVehicles => UnturnedServer.Vehicles.Where(v => v.LockedOwner == SteamID || (IsInQuestGroup && QuestGroupID == v.LockedGroup)).ToArray();
        /// <summary>
        /// The current storage the player is looking in
        /// </summary>
        public InteractableStorage Storage => Inventory.storage;

        // Extra data
        /// <summary>
        /// The player list that the player can see
        /// </summary>
        public UnturnedPlayer[] PlayerList => _PlayerList.ToArray();
        /// <summary>
        /// The steam groups this player is part of
        /// </summary>
        public RG[] SteamGroups => Steam.Groups;
        /// <summary>
        /// The prefixes of the player
        /// </summary>
        public string[] Prefixes => _Prefixes.ToArray();
        /// <summary>
        /// The suffixes of the player
        /// </summary>
        public string[] Suffixes => _Suffixes.ToArray();
        /// <summary>
        /// The number of kills since the player connected
        /// </summary>
        public int Kills { get; internal set; }
        /// <summary>
        /// The number of deaths since the player connected
        /// </summary>
        public int Deaths { get; internal set; }
        /// <summary>
        /// The number of kills since the player first joined the server
        /// </summary>
        public int TotalKills { get; internal set; }
        /// <summary>
        /// The number of deaths since the player first joined the server
        /// </summary>
        public int TotalDeaths { get; internal set; }
        /// <summary>
        /// The gameobject of the player
        /// </summary>
        public override GameObject GameObject => Player.gameObject;
        #endregion

        private UnturnedPlayer(SPlayer steamplayer)
        {
            // Set the variables
            this.SteamPlayer = steamplayer;
            this.Steam = new SP(SteamID.m_SteamID);
            this.Deaths = 0;
            this.TotalDeaths = 0;
            this.Kills = 0;
            this.TotalKills = 0;

            // Run code
            UnturnedServer.AddPlayer(this);
        }

        #region Static Functions
        /// <summary>
        /// Creates the unturned player instance or returns an existing one
        /// </summary>
        /// <param name="steamplayer">The steam player to build from</param>
        /// <returns>An unturned player instance</returns>
        internal static UnturnedPlayer Create(SPlayer steamplayer) => UnturnedServer.Players.FirstOrDefault(a => a.SteamPlayer == steamplayer) ?? new UnturnedPlayer(steamplayer);

        /// <summary>
        /// Checks if the player is still in the server and returns the result
        /// </summary>
        /// <param name="player">The player to look for</param>
        /// <returns>If the player is still in the server or not</returns>
        public static bool IsInServer(UnturnedPlayer player) => UnturnedServer.IsInServer(player);

        /// <summary>
        /// Gets the player/server name and returns it
        /// </summary>
        /// <param name="player">The player instance/null for server</param>
        /// <returns>The name of player/server</returns>
        public static string GetName(PointBlankPlayer player) => IsServer(player) ? "Server" : player.ToString();

        /// <summary>
        /// Gets the unturned player instance based on steam player instance
        /// </summary>
        /// <param name="player">The steam player instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer Get(SPlayer player) => UnturnedServer.GetPlayer(player);
        /// <summary>
        /// Gets the unturned player instance based on player instance
        /// </summary>
        /// <param name="player">The player instance</param>
        /// <returns>The unturned player instace</returns>
        public static UnturnedPlayer Get(UPlayer player) => UnturnedServer.GetPlayer(player);
        /// <summary>
        /// Gets the unturned player instance based on arena player instance
        /// </summary>
        /// <param name="player">The unturned player instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer Get(ArenaPlayer player) => UnturnedServer.GetPlayer(player);
        /// <summary>
        /// Gets the unturned player instance based on steam player id instance
        /// </summary>
        /// <param name="playerID">The steam player id instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer Get(SteamPlayerID playerID) => UnturnedServer.GetPlayer(playerID);
        /// <summary>
        /// Gets the unturned player instance based on steam id instance
        /// </summary>
        /// <param name="steamID">The steam id instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer Get(CSteamID steamID) => UnturnedServer.GetPlayer(steamID);
        /// <summary>
        /// Gets the unturned player instance based on steam64 ID
        /// </summary>
        /// <param name="steam64">The steam64 ID</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer Get(ulong steam64) => UnturnedServer.GetPlayer(steam64);

        /// <summary>
        /// Tries to get the unturned player instance based on the parameter
        /// </summary>
        /// <param name="param">The parameter to test</param>
        /// <param name="player">The unturned player instance</param>
        /// <returns>If the player was gotten successfully</returns>
        public static bool TryGetPlayer(string param, out UnturnedPlayer player)
        {
            if (ulong.TryParse(param, out ulong steam64))
            {
                player = Get(steam64);
                return true;
            }

            for(int i = 0; i < UnturnedServer.Players.Length; i++)
            {
                if (!NameTool.checkNames(param, UnturnedServer.Players[i].PlayerName) &&
                    !NameTool.checkNames(param, UnturnedServer.Players[i].CharacterName)) continue;
                player = UnturnedServer.Players[i];
                return true;
            }
            player = null;
            return false;
        }

        /// <summary>
        /// Tries to get the unturned players based on the parameters
        /// </summary>
        /// <param name="param">The parameter to test</param>
        /// <param name="players">The unturned player instance</param>
        /// <returns>If the players were gotten successfully</returns>
        public static bool TryGetPlayers(string param, out UnturnedPlayer[] players)
        {
            if(ulong.TryParse(param, out ulong steam64))
            {
                players = new UnturnedPlayer[1];
                players[0] = Get(steam64);
                return true;
            }
            if(param == "*")
            {
                players = UnturnedServer.Players;
                return true;
            }

            List<UnturnedPlayer> plys = new List<UnturnedPlayer>();
            UnturnedServer.Players.For((i, player) =>
            {
                if (!NameTool.checkNames(param, player.PlayerName) &&
                   !NameTool.checkNames(param, player.CharacterName)) return;

                plys.Add(player);
            });

            players = plys.ToArray();
            return (players.Length > 0);
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Adds a player to the player's player list
        /// </summary>
        /// <param name="player">The player to add to the list</param>
        /// <param name="triggerEvent">Should the event be triggered</param>
        public void AddPlayer(UnturnedPlayer player, bool triggerEvent = true)
        {
            if (PlayerList.Contains(player))
                return;

            if (triggerEvent)
                PlayerEvents.RunListPlayerAdd(this, player);
            _PlayerList.Add(player);
        }
        /// <summary>
        /// Removes a player from the player's player list
        /// </summary>
        /// <param name="player">The player to remove from the list</param>
        /// <param name="triggerEvent">Should the event be triggered</param>
        public void RemovePlayer(UnturnedPlayer player, bool triggerEvent = true)
        {
            if (!PlayerList.Contains(player))
                return;

            if (triggerEvent)
                PlayerEvents.RunListPlayerRemove(this, player);
            _PlayerList.Remove(player);
        }

        /// <summary>
        /// Adds a prefix to the player
        /// </summary>
        /// <param name="prefix">The prefix to add</param>
        public void AddPrefix(string prefix)
        {
            if (Prefixes.Contains(prefix))
                return;

            _Prefixes.Add(prefix);
            if (Loaded)
                PlayerEvents.RunPrefixAdd(this, prefix);
        }
        /// <summary>
        /// Removes a prefix from the player
        /// </summary>
        /// <param name="prefix">The prefix to remove</param>
        public void RemovePrefix(string prefix)
        {
            if (!Prefixes.Contains(prefix))
                return;

            _Prefixes.Remove(prefix);
            if (Loaded)
                PlayerEvents.RunPrefixRemove(this, prefix);
        }
        /// <summary>
        /// Gets the prefix string of the player
        /// </summary>
        /// <returns>The prefix string</returns>
        public string GetPrefix()
        {
            string prefix = "";

            for (int a = 0; a < Groups.Length; a++)
                for (int b = 0; b < Groups[a].Prefixes.Length; b++)
                    prefix += "[" + Groups[a].Prefixes[b] + "]";
            for (int i = 0; i < Prefixes.Length; i++)
                prefix += "[" + Prefixes[i] + "]";

            return prefix;
        }

        /// <summary>
        /// Adds a suffix to the player
        /// </summary>
        /// <param name="suffix">The suffix to add</param>
        public void AddSuffix(string suffix)
        {
            if (Suffixes.Contains(suffix))
                return;

            _Suffixes.Add(suffix);
            if (Loaded)
                PlayerEvents.RunSuffixAdd(this, suffix);
        }
        /// <summary>
        /// Removes a suffix from the player
        /// </summary>
        /// <param name="suffix">The suffix to remove</param>
        public void RemoveSuffix(string suffix)
        {
            if (!Suffixes.Contains(suffix))
                return;

            _Suffixes.Remove(suffix);
            if (Loaded)
                PlayerEvents.RunSuffixRemove(this, suffix);
        }
        /// <summary>
        /// Gets the suffix string of the player
        /// </summary>
        /// <returns>The suffix string</returns>
        public string GetSuffix()
        {
            string suffix = "";

            for(int a = 0; a < Groups.Length; a++)
                for(int b = 0; b < Groups[a].Suffixes.Length; b++)
                    suffix += "[" + Groups[a].Suffixes[b] + "]";
            for(int i = 0; i < Suffixes.Length; i++)
                suffix += "[" + Suffixes[i] + "]";

            return suffix;
        }

        /// <summary>
        /// Gets all permissions attached to the user
        /// </summary>
        /// <returns>The list of permissions attached to the user</returns>
        public override PointBlankPermission[] GetPermissions()
        {
            List<PointBlankPermission> permissions = Permissions.ToList();

            for (int i = 0; i < Groups.Length; i++)
            {
                foreach (PointBlankPermission perm in Groups[i].GetPermissions())
                {
                    PointBlankPermission cPerm = permissions.FirstOrDefault(a => a == perm);

                    if (cPerm != null && cPerm.Cooldown != null && perm.Cooldown != null)
                        if (cPerm.Cooldown > perm.Cooldown)
                            permissions.Remove(cPerm);
                    permissions.Add(perm);
                }
            }
            for (int i = 0; i < SteamGroups.Length; i++)
            {
                foreach (PointBlankPermission perm in SteamGroups[i].GetPermissions())
                {
                    PointBlankPermission cPerm = permissions.FirstOrDefault(a => a == perm);

                    if (cPerm != null && cPerm.Cooldown != null && perm.Cooldown != null)
                        if (cPerm.Cooldown > perm.Cooldown)
                            permissions.Remove(cPerm);
                    permissions.Add(perm);
                }
            }

            return permissions.ToArray();

        }

        /// <summary>
        /// Returns the position the player is looking at
        /// </summary>
        /// <param name="distance">The ray distance</param>
        /// <param name="masks">The ray masks</param>
        /// <returns>The position the player is looking at</returns>
        public Vector3? GetEyePosition(float distance, int masks) => GetEyePosition(Look.aim.position, Look.aim.forward, distance, masks);
        /// <summary>
        /// Returns the position the player is looking at
        /// </summary>
        /// <param name="distance">The ray distance</param>
        /// <returns>The position the player is looking at</returns>
        public Vector3? GetEyePosition(float distance) => GetEyePosition(distance, RayMasks.BLOCK_COLLISION & ~(1 << 0x15));

        /// <summary>
        /// Sends a message to the player
        /// </summary>
        /// <param name="message">The message to tell the player</param>
        /// <param name="color">The color of the message</param>
        /// <param name="mode">The mode of the message</param>
        public override void SendMessage(object message, Color color) => ChatManager.say(SteamID, message.ToString(), color);
        /// <summary>
        /// Fake sends the message to look like the player sent it
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="color">The color of the message</param>
        public void ForceSay(string message, Color color) => CM.FakeMessage(SteamID, message, color);
        #endregion

        #region Unturned Functions
        /// <summary>
        /// Checks if the player has a specific item
        /// </summary>
        /// <param name="ID">The item ID to find</param>
        /// <returns>If the player has the specific item in their inventory</returns>
        public bool HasItem(ushort ID)
        {
            if (EquippedItemID == ID) return true;

            return (Inventory.search(ID, true, true).Count > 0); // The easy way
        }
        /// <summary>
        /// Checks if the player has a specific item
        /// </summary>
        /// <param name="Item">The item to find</param>
        /// <returns>If the player has the item in the inventory</returns>
        public bool HasItem(UnturnedStoredItem Item) => HasItem(Item.ID);
        /// <summary>
        /// Checks if the player has a specific item
        /// </summary>
        /// <param name="Name">The item to find</param>
        /// <returns>If the player has the item in the inventory</returns>
        public bool HasItem(string Name) => HasItem(((ItemAsset)Assets.find(EAssetType.ITEM, Name)).id);

        /// <summary>
        /// Gives the player an item
        /// </summary>
        /// <param name="ID">The ID of the item</param>
        /// <returns>If the item was given to the player</returns>
        public bool GiveItem(ushort ID) => ItemTool.tryForceGiveItem(Player, ID, 1);
        /// <summary>
        /// Gives the player an item
        /// </summary>
        /// <param name="ID">The item instance to give to the player</param>
        /// <returns>If the item was given to the player</returns>
        public bool GiveItem(UnturnedStoredItem Item) => GiveItem(Item.ID);
        /// <summary>
        /// Gives the player an item
        /// </summary>
        /// <param name="ID">The name of the item to give to the player</param>
        /// <returns>If the item was given to the player</returns>
        public bool GiveItem(String Name) => GiveItem((Assets.find(EAssetType.ITEM, Name) as ItemAsset).id);

        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="ID">The ID of the item to remove</param>
        /// <returns>If the item was removed</returns>
        public bool RemoveItem(ushort ID)
        {
            if (EquippedItemID == ID)
                Equipment.dequip();
            InventorySearch search = Inventory.search(ID, true, true).FirstOrDefault();

            if (search == null)
                return false;
            Items items = Inventory.items[search.page];

            items.removeItem(items.getIndex(search.jar.x, search.jar.y));
            return true;
        }
        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="Item">The item instance to remove</param>
        /// <returns>If the item was removed</returns>
        public bool RemoveItem(UnturnedStoredItem Item) => RemoveItem(Item.ID);
        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="Name">The item's name to remove</param>
        /// <returns>If the item was removed</returns>
        public bool RemoveItem(string Name) => RemoveItem((Assets.find(EAssetType.ITEM, Name) as ItemAsset).id);

        /// <summary>
        /// Sends effect to the player
        /// </summary>
        /// <param name="id">The effect id to trigger</param>
        public void SendEffect(ushort id) => EffectManager.instance.tellEffectPoint(SteamID, id, Position);
        /// <summary>
        /// Clear effect by id
        /// </summary>
        /// <param name="id">The effect id to clear</param>
        public void ClearEffect(ushort id) => EffectManager.instance.tellEffectClearByID(SteamID, id);

        /// <summary>
        /// Dequip the player's equipped item
        ///</summary>
        public void DequipItem() => Equipment.dequip();

        /// <summary>
        /// Teleports the player to a specific position
        /// </summary>
        /// <param name="position">The position to teleport the player to</param>
        public void Teleport(Vector3 position) => Player.sendTeleport(position, MeasurementTool.angleToByte(Player.transform.rotation.eulerAngles.y));

        /// <summary>
        /// Disconnects the player
        /// </summary>
        /// <param name="message">Message displayed when kicked</param>
        public void Kick(string message) => Provider.kick(SteamID, message);

        /// <summary>
        /// Executes a message as the player(commands included)
        /// </summary>
        /// <param name="message">The message to execute as the player</param>
        public void Sudo(string message) => ChatManager.instance.askChat(SteamID, (byte)EChatMode.GLOBAL, message);
        #endregion

        #region Override Functions
        public override string ToString() => CharacterName;
        #endregion
    }
}
