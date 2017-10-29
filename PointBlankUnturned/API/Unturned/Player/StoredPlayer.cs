using System;
using System.Collections.Generic;
using Steamworks;

namespace PointBlank.API.Unturned.Player
{
    public class StoredPlayer
    {
        #region Properties

        public CSteamID SteamID;

        public List<String> CharacterNames;

        public List<String> IPs;

        public List<String> SteamNames;

        public DateTime LastLogin;
        
        #endregion
        
        public StoredPlayer(CSteamID steamID, List<String> characterNames, List<String> ips, List<String> steamNames, DateTime lastLogin)
        {
            SteamID = steamID;
            CharacterNames = characterNames;
            IPs = ips;
            SteamNames = steamNames;
            LastLogin = lastLogin;
        }
    }
}