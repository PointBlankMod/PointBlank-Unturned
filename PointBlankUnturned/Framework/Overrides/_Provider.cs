using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace PointBlank.Framework.Overrides
{
    internal class _Provider
    {
        #region Reflection
        private static FieldInfo fi_bytesSent = PointBlankReflect.GetField<Provider>("_bytesSent", BindingFlags.NonPublic | BindingFlags.Static);
        private static FieldInfo fi_packetsSent = PointBlankReflect.GetField<Provider>("_packetsSent", BindingFlags.NonPublic | BindingFlags.Static);

        private static MethodInfo mi_receiveServer = PointBlankReflect.GetMethod<Provider>("receiveServer", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo mi_isUnreliable = PointBlankReflect.GetMethod<Provider>("isUnreliable", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo mi_isInstant = PointBlankReflect.GetMethod<Provider>("isInstant", BindingFlags.NonPublic | BindingFlags.Static);
        #endregion

        [Detour(typeof(Provider), "send", BindingFlags.Public | BindingFlags.Static)]
        public static void send(CSteamID steamID, ESteamPacket type, byte[] packet, int size, int channel)
        {
            if (!Provider.isConnected)
                return;
            bool cancel = false;

            ServerEvents.RunPacketSent(ref steamID, ref type, ref packet, ref size, ref channel, ref cancel);
            if (cancel)
                return;
            if(steamID == Provider.server)
            {
                mi_receiveServer.Invoke(null, new object[] { Provider.server, packet, 0, size, channel });
                return;
            }
            if(steamID.m_SteamID == 0uL)
            {
                Debug.LogError("Failed to send to invalid steam ID.");
                return;
            }
            if((bool)mi_isUnreliable.Invoke(null, new object[] { type }))
            {
                for(int i = 0; i < 3; i++) // Fix for the queue #1 stuck position
                    if (SteamGameServerNetworking.SendP2PPacket(steamID, packet, (uint)size, (!(bool)mi_isInstant.Invoke(null, new object[] { type })) ? EP2PSend.k_EP2PSendUnreliable : EP2PSend.k_EP2PSendUnreliableNoDelay, channel))
                        break;
                return;
            }
            for(int i = 0; i < 3; i++) // Fix for the queue #1 stuck position
                if (SteamGameServerNetworking.SendP2PPacket(steamID, packet, (uint)size, (!(bool)mi_isInstant.Invoke(null, new object[] { type })) ? EP2PSend.k_EP2PSendReliableWithBuffering : EP2PSend.k_EP2PSendReliable, channel))
                    break;
        }
    }
}
