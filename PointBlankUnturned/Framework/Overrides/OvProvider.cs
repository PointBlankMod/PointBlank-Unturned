using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace PointBlank.Framework.Overrides
{
	internal class OvProvider
	{
        #region Reflection
        private static FieldInfo _fiBytesSent = PointBlankReflect.GetField<SDG.Unturned.Provider>("_bytesSent", BindingFlags.NonPublic | BindingFlags.Static);
        private static FieldInfo _fiPacketsSent = PointBlankReflect.GetField<SDG.Unturned.Provider>("_packetsSent", BindingFlags.NonPublic | BindingFlags.Static);

        private static MethodInfo _miReceiveServer = PointBlankReflect.GetMethod<SDG.Unturned.Provider>("receiveServer", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo _miIsUnreliable = PointBlankReflect.GetMethod<SDG.Unturned.Provider>("isUnreliable", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo _miIsInstant = PointBlankReflect.GetMethod<SDG.Unturned.Provider>("isInstant", BindingFlags.NonPublic | BindingFlags.Static);
        #endregion

        [Detour(typeof(SDG.Unturned.Provider), "send", BindingFlags.Public | BindingFlags.Static)]
        public static void Send(CSteamID steamId, ESteamPacket type, byte[] packet, int size, int channel)
        {
            if (!SDG.Unturned.Provider.isConnected)
                return;
            bool cancel = false;

            ServerEvents.RunPacketSent(ref steamId, ref type, ref packet, ref size, ref channel, ref cancel);
            if (cancel)
                return;
            if(steamId == SDG.Unturned.Provider.server)
            {
                _miReceiveServer.Invoke(null, new object[] { SDG.Unturned.Provider.server, packet, 0, size, channel });
                return;
            }
            if(steamId.m_SteamID == 0uL)
            {
                Debug.LogError("Failed to send to invalid steam ID.");
                return;
            }
            if((bool)_miIsUnreliable.Invoke(null, new object[] { type }))
            {
                for(int i = 0; i < 3; i++) // Fix for the queue #1 stuck position
                    if (SteamGameServerNetworking.SendP2PPacket(steamId, packet, (uint)size, (!(bool)_miIsInstant.Invoke(null, new object[] { type })) ? EP2PSend.k_EP2PSendUnreliable : EP2PSend.k_EP2PSendUnreliableNoDelay, channel))
                        break;
                return;
            }
            for(int i = 0; i < 3; i++) // Fix for the queue #1 stuck position
                if (SteamGameServerNetworking.SendP2PPacket(steamId, packet, (uint)size, (!(bool)_miIsInstant.Invoke(null, new object[] { type })) ? EP2PSend.k_EP2PSendReliableWithBuffering : EP2PSend.k_EP2PSendReliable, channel))
                    break;
        }
    }
}
