﻿using System.Collections.Generic;
using System.Linq;
using PointBlank.API.Services;
using PointBlank.Services.APIManager;

namespace PointBlank.API.Steam
{
    public static class SteamGroupManager
    {
        #region Variables
        private static Dictionary<ulong, SteamGroup> _Groups = new Dictionary<ulong, SteamGroup>();
        #endregion

        #region Properties
        /// <summary>
        /// The steam group list on the server
        /// </summary>
        public static SteamGroup[] Groups => _Groups.Values.ToArray();

        /// <summary>
        /// Is the steam group manager loaded(this is for events)
        /// </summary>
        public static bool Loaded { get; set; } = false;
        #endregion

        #region Public Functions
        /// <summary>
        /// Adds a steam group to the server
        /// </summary>
        /// <param name="group">The steam group to add</param>
        public static void AddSteamGroup(SteamGroup group)
        {
            if (_Groups.ContainsKey(group.ID))
                return;
            _Groups.Add(group.ID, group);

            if (Loaded)
                SteamGroupEvents.RunSteamGroupAdded(group);
        }
        /// <summary>
        /// Creates and adds a steam group to the server
        /// </summary>
        /// <param name="ID">The ID of the steam group</param>
        /// <param name="cooldown">The command cooldown of the steam group</param>
        public static void AddSteamGroup(ulong ID)
        {
            if (_Groups.ContainsKey(ID))
                return;
            SteamGroup group = new SteamGroup(ID);

            _Groups.Add(ID, group);

            if (Loaded)
                SteamGroupEvents.RunSteamGroupAdded(group);
        }

        /// <summary>
        /// Removes a steam group from the server
        /// </summary>
        /// <param name="group">The group to remove</param>
        public static void RemoveSteamGroup(SteamGroup group)
        {
            if (!_Groups.ContainsValue(group))
                return;
            _Groups.Remove(group.ID);

            if (Loaded)
                SteamGroupEvents.RunSteamGroupRemoved(group);
        }
        /// <summary>
        /// Removes a steam group from the server
        /// </summary>
        /// <param name="ID">The steam group ID to remove</param>
        public static void RemoveSteamGroup(ulong ID)
        {
            if (_Groups.ContainsKey(ID))
                return;
            SteamGroup group = _Groups[ID];

            _Groups.Remove(ID);

            if (Loaded)
                SteamGroupEvents.RunSteamGroupRemoved(group);
        }

        /// <summary>
        /// Finds a steam group in the server and returns it
        /// </summary>
        /// <param name="ID">The steam64 ID of the group</param>
        /// <returns>The steam group instance</returns>
        public static SteamGroup Find(ulong ID) => Groups.FirstOrDefault(a => a.ID == ID);

        /// <summary>
        /// Tries to find the steam group by ID and returns it
        /// </summary>
        /// <param name="ID">The steam64 of the steam group</param>
        /// <param name="group">The returned instace of the found steam group</param>
        /// <returns>Has the group been found</returns>
        public static bool TryFindSteamGroup(ulong ID, out SteamGroup group)
        {
            SteamGroup g = Find(ID);

            group = g;
            return g != null;
        }

        /// <summary>
        /// Reloads the steam group
        /// </summary>
        public static void Reload()
        {
            InfoManager im = (InfoManager)PointBlankServiceManager.GetService("InfoManager.InfoManager");

            im.LoadSteamGroups();
        }
        #endregion
    }
}
