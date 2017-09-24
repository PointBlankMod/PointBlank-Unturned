using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using PointBlank.API.Services;
using PointBlank.API.Implements;
using PointBlank.Services.APIManager;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Structure;
using PointBlank.API.Unturned.Vehicle;
using PointBlank.API.Unturned.Barricade;
using PointBlank.API.Unturned.Item;
using PointBlank.API.Unturned.Zombie;
using PointBlank.API.Unturned.Animal;
using Steamworks;
using UPlayer = SDG.Unturned.Player;

namespace PointBlank.API.Unturned.Server
{
    /// <summary>
    /// Functions for managing the unturned server
    /// </summary>
    public static class UnturnedServer
    {
        #region Variables
        private static HashSet<UnturnedPlayer> _players = new HashSet<UnturnedPlayer>();
        private static HashSet<StoredPlayer> _storedPlayers = new HashSet<StoredPlayer>();
        private static HashSet<UnturnedVehicle> _vehicles = new HashSet<UnturnedVehicle>();
        private static HashSet<UnturnedStructure> _structures = new HashSet<UnturnedStructure>();
        private static HashSet<UnturnedBarricade> _barricades = new HashSet<UnturnedBarricade>();
        private static HashSet<UnturnedItem> _items = new HashSet<UnturnedItem>();
        private static HashSet<UnturnedZombie> _zombies = new HashSet<UnturnedZombie>();
        private static HashSet<UnturnedAnimal> _animals = new HashSet<UnturnedAnimal>();
        #endregion

        #region Properties
        /// <summary>
        /// The currently online players
        /// </summary>
        public static UnturnedPlayer[] Players => _players.ToArray();
#if DEBUG
        /// <summary>
        /// All players that have connected to the server
        /// </summary>
        public static StoredPlayer[] StoredPlayers => _storedPlayers.ToArray();
#endif
        /// <summary>
        /// All vehicles on the server
        /// </summary>
        public static UnturnedVehicle[] Vehicles => _vehicles.ToArray();
        /// <summary>
        /// Structures within the server
        /// </summary>
        public static UnturnedStructure[] Structures => _structures.ToArray();
        /// <summary>
        /// Barricades within the server
        /// </summary>
        public static UnturnedBarricade[] Barricades => _barricades.ToArray();
        /// <summary>
        /// Items within the server
        /// </summary>
        public static UnturnedItem[] Items => _items.ToArray();
        /// <summary>
        /// The currently spawned zombies in the server
        /// </summary>
        public static UnturnedZombie[] Zombies => _zombies.ToArray();
        /// <summary>
        /// The currently spawned animals in the server
        /// </summary>
        public static UnturnedAnimal[] Animals => _animals.ToArray();
        /// <summary>
        /// Current game time
        /// </summary>
        public static uint GameTime
        {
            get => LightingManager.time;
            set => LightingManager.time = value;
        }
        /// <summary>
        /// Is it day time on the server
        /// </summary>
        public static bool IsDay => LightingManager.isDaytime;
        /// <summary>
        /// Is it currently full moon on the server
        /// </summary>
        public static bool IsFullMoon
        {
            get => LightingManager.isFullMoon;
            set => LightingManager.isFullMoon = value;
        }
        /// <summary>
        /// Is it currently raining/snowing
        /// </summary>
        public static bool IsRaining => LightingManager.hasRain;
        #endregion

        #region Functions
        internal static UnturnedPlayer AddPlayer(UnturnedPlayer player)
        {
            UnturnedPlayer ply = Players.FirstOrDefault(a => a.SteamPlayer == player.SteamPlayer);

            if (ply != null)
                return ply;

            _players.Add(player);
            return player;
        }
        internal static bool RemovePlayer(UnturnedPlayer player)
        {
            UnturnedPlayer ply = Players.FirstOrDefault(a => a.SteamPlayer == player.SteamPlayer);

            if (ply == null)
                return false;

            _players.Remove(ply);
            return true;
        }

        internal static UnturnedStructure AddStructure(UnturnedStructure structure)
        {
            UnturnedStructure stru = Structures.FirstOrDefault(a => a.Data == structure.Data);

            if (stru != null)
                return stru;

            _structures.Add(structure);
            return structure;
        }
        internal static bool RemoveStructure(UnturnedStructure structure)
        {
            UnturnedStructure stru = Structures.FirstOrDefault(a => a.Data == structure.Data);

            if (stru == null)
                return false;

            _structures.Remove(stru);
            return true;
        }

        internal static UnturnedBarricade AddBarricade(UnturnedBarricade barricade)
        {
            if (barricade != null)
                return barricade;

            _barricades.Add(barricade);
            return barricade;
        }
        internal static bool RemoveBarricade(UnturnedBarricade barricade)
        {
            if (barricade == null)
                return false;

            _barricades.Remove(barricade);
            return true;
        }

        internal static UnturnedVehicle AddVehicle(UnturnedVehicle vehicle)
        {
            UnturnedVehicle veh = Vehicles.FirstOrDefault(a => a.Vehicle == vehicle.Vehicle);

            if (veh != null)
                return veh;

            _vehicles.Add(vehicle);
            return vehicle;
        }
        internal static bool RemoveVehicle(UnturnedVehicle vehicle)
        {
            UnturnedVehicle veh = Vehicles.FirstOrDefault(a => a.Vehicle == vehicle.Vehicle);

            if (veh == null)
                return false;

            _vehicles.Remove(vehicle);
            return true;
        }

        internal static UnturnedItem AddItem(UnturnedItem item)
        {
            UnturnedItem itm = Items.FirstOrDefault(a => a.Item == item.Item);

            if (itm != null)
                return itm;

            _items.Add(item);
            return item;
        }
        internal static bool RemoveItem(UnturnedItem item)
        {
            UnturnedItem itm = Items.FirstOrDefault(a => a.Item == item.Item);

            if (itm == null)
                return false;

            _items.Remove(itm);
            return true;
        }

        internal static UnturnedZombie AddZombie(UnturnedZombie zombie)
        {
            UnturnedZombie zmb = Zombies.FirstOrDefault(a => a.Zombie == zombie.Zombie);

            if (zmb != null)
                return zmb;

            _zombies.Add(zombie);
            return zombie;
        }
        internal static bool RemoveZombie(UnturnedZombie zombie)
        {
            UnturnedZombie zmb = Zombies.FirstOrDefault(a => a.Zombie == zombie.Zombie);

            if (zmb == null)
                return false;

            _zombies.Remove(zombie);
            return true;
        }

        internal static UnturnedAnimal AddAnimal(UnturnedAnimal animal)
        {
            UnturnedAnimal ani = Animals.FirstOrDefault(a => a.Animal == animal.Animal);

            if (ani != null)
                return ani;

            _animals.Add(animal);
            return animal;
        }
        internal static bool RemoveAnimal(UnturnedAnimal animal)
        {
            UnturnedAnimal ani = Animals.FirstOrDefault(a => a.Animal == animal.Animal);

            if (ani != null)
                return false;

            _animals.Remove(animal);
            return true;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Checks if the UnturnedPlayer is the server player
        /// </summary>
        /// <param name="player">The unturned player instance to check</param>
        /// <returns>If the UnturnedPlayer instance is the server</returns>
        public static bool IsServer(UnturnedPlayer player) => (player == null);
        /// <summary>
        /// Checks if the player is still in the server and returns the result
        /// </summary>
        /// <param name="player">The player to look for</param>
        /// <returns>If the player is still in the server or not</returns>
        public static bool IsInServer(UnturnedPlayer player) => Players.Contains(player);

        /// <summary>
        /// Gets the unturned player instance based on steam player instance
        /// </summary>
        /// <param name="player">The steam player instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer GetPlayer(SteamPlayer player) => Players.FirstOrDefault(a => a.SteamPlayer == player);
        /// <summary>
        /// Gets the unturned player instance based on player instance
        /// </summary>
        /// <param name="player">The player instance</param>
        /// <returns>The unturned player instace</returns>
        public static UnturnedPlayer GetPlayer(UPlayer player) => Players.FirstOrDefault(a => a.Player == player);
        /// <summary>
        /// Gets the unturned player instance based on arena player instance
        /// </summary>
        /// <param name="player">The unturned player instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer GetPlayer(ArenaPlayer player) => Players.FirstOrDefault(a => a.SteamPlayer == player.steamPlayer);
        /// <summary>
        /// Gets the unturned player instance based on steam player id instance
        /// </summary>
        /// <param name="playerId">The steam player id instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer GetPlayer(SteamPlayerID playerId) => Players.FirstOrDefault(a => a.SteamPlayerId == playerId);
        /// <summary>
        /// Gets the unturned player instance based on steam id instance
        /// </summary>
        /// <param name="steamId">The steam id instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer GetPlayer(CSteamID steamId) => Players.FirstOrDefault(a => a.SteamId == steamId);
        /// <summary>
        /// Gets the unturned player instance based on steam64 ID
        /// </summary>
        /// <param name="steam64">The steam64 ID</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer GetPlayer(ulong steam64) => Players.FirstOrDefault(a => a.SteamId.m_SteamID == steam64);

        /// <summary>
        /// Changes the map
        /// </summary>
        /// <param name="mapName">The name of the map to change to</param>
        /// <returns>Whether the map was successfully changed or not</returns>
        public static bool ChangeMap(string mapName)
        {
            if (!Level.exists(mapName))
                return false;

            // Kick players
            for (int i = 0; i < Players.Length; i++)
                Players[i].Kick("Map is changing.");

            // Reset PointBlank
            _vehicles.Clear();
            _structures.Clear();
            _items.Clear();
            _barricades.Clear();
            _zombies.Clear();

            // Reset Unturned
            Provider.gameMode = null;
            Provider.selectedGameModeName = null;
            PointBlankReflect.GetField<Level>("_isLoaded", PointBlankReflect.STATIC_FLAG).SetValue(null, false);

            // Setup map
            Provider.map = mapName;
            Level.load(Level.getLevel(Provider.map));
            PointBlankReflect.GetMethod<Provider>("loadGameMode", PointBlankReflect.STATIC_FLAG).RunMethod(null);
            SteamGameServer.SetMapName(Provider.map);
            SteamGameServer.SetGameTags(string.Concat(new object[]
            {
                (!Provider.isPvP) ? "PVE" : "PVP",
                ",GAMEMODE:",
                Provider.gameMode.GetType().Name,
                ',',
                (!Provider.hasCheats) ? "STAEHC" : "CHEATS",
                ',',
                Provider.mode.ToString(),
                ",",
                Provider.cameraMode.ToString(),
                ",",
                (Provider.serverWorkshopFileIDs.Count <= 0) ? "KROW" : "WORK",
                ",",
                (!Provider.isGold) ? "YLNODLOG" : "GOLDONLY",
                ",",
                (!Provider.configData.Server.BattlEye_Secure) ? "BATTLEYE_OFF" : "BATTLEYE_ON"
            }));
            if (Provider.serverWorkshopFileIDs.Count > 0)
            {
                string text = string.Empty;
                for (int l = 0; l < Provider.serverWorkshopFileIDs.Count; l++)
                {
                    if (text.Length > 0)
                    {
                        text += ',';
                    }
                    text += Provider.serverWorkshopFileIDs[l];
                }
                int num4 = (text.Length - 1) / 120 + 1;
                int num5 = 0;
                SteamGameServer.SetKeyValue("Browser_Workshop_Count", num4.ToString());
                for (int m = 0; m < text.Length; m += 120)
                {
                    int num6 = 120;
                    if (m + num6 > text.Length)
                    {
                        num6 = text.Length - m;
                    }
                    string pValue2 = text.Substring(m, num6);
                    SteamGameServer.SetKeyValue("Browser_Workshop_Line_" + num5, pValue2);
                    num5++;
                }
            }
            string text2 = string.Empty;
            Type type = Provider.modeConfigData.GetType();
            FieldInfo[] fields = type.GetFields();
            for (int n = 0; n < fields.Length; n++)
            {
                FieldInfo fieldInfo = fields[n];
                object value = fieldInfo.GetValue(Provider.modeConfigData);
                Type type2 = value.GetType();
                FieldInfo[] fields2 = type2.GetFields();
                for (int num7 = 0; num7 < fields2.Length; num7++)
                {
                    FieldInfo fieldInfo2 = fields2[num7];
                    object value2 = fieldInfo2.GetValue(value);
                    if (text2.Length > 0)
                    {
                        text2 += ',';
                    }
                    if (value2 is bool)
                    {
                        text2 += ((!(bool)value2) ? "F" : "T");
                    }
                    else
                    {
                        text2 += value2;
                    }
                }
            }
            int num8 = (text2.Length - 1) / 120 + 1;
            int num9 = 0;

            SteamGameServer.SetKeyValue("Browser_Config_Count", num8.ToString());
            for (int num10 = 0; num10 < text2.Length; num10 += 120)
            {
                int num11 = 120;

                if (num10 + num11 > text2.Length)
                    num11 = text2.Length - num10;
                string pValue3 = text2.Substring(num10, num11);

                SteamGameServer.SetKeyValue("Browser_Config_Line_" + num9, pValue3);
                num9++;
            }

            return true;
        }

        /// <summary>
        /// Reloads the player configs
        /// </summary>
        public static void ReloadPlayers()
        {
            InfoManager info = (InfoManager)PointBlankServiceManager.GetService("InfoManager.InfoManager");

            foreach(UnturnedPlayer player in Players)
                info.OnPlayerJoin(player);
        }
        #endregion
    }
}