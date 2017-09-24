using SDG.Unturned;
using Steamworks;

namespace PointBlank.API.Unturned
{
    /// <summary>
    /// Implementation of the unturned ownership tool
    /// </summary>
    public static class OwnershipTool
    {
        public static bool CheckToggle(ulong player, ulong group)
        {
            return !Dedicator.isDedicated && OwnershipTool.CheckToggle(Provider.client, player, SDG.Unturned.Player.player.quests.groupID, group);
        }

        public static bool CheckToggle(CSteamID player0, ulong player1, CSteamID group0, ulong group1)
        {
            return (Provider.isServer && !Dedicator.isDedicated) || player0.m_SteamID == player1 || (group0 != CSteamID.Nil && group0.m_SteamID == group1);
        }
    }
}
