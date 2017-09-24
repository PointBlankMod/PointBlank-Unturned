using System;
using System.Collections.Generic;
using Steamworks;

namespace PointBlank.API.Unturned.Player
{
    public class StoredPlayer
    {
        #region Properties

        public CSteamID SteamId;

        public List<String> CharacterNames;

        public List<String> Ps;

        public List<String> SteamNames;

        public DateTime LastLogin;
        
        #endregion
        
        public StoredPlayer(CSteamID steamId, List<String> characterNames, List<String> ips, List<String> steamNames, DateTime lastLogin)
        {
            SteamId = steamId;
            CharacterNames = characterNames;
            Ps = ips;
            SteamNames = steamNames;
            LastLogin = lastLogin;
        }
    }
}