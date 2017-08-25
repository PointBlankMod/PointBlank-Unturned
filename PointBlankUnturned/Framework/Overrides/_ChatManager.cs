using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using CM = SDG.Unturned.ChatManager;

namespace PointBlank.Framework.Overrides
{
    internal class _ChatManager
    {
        #region Reflection
        private static MethodInfo mi_askChat = PointBlankReflect.GetMethod<CM>("askChat", BindingFlags.Public | BindingFlags.Instance);
        #endregion

        [Detour(typeof(CM), "askChat", BindingFlags.Instance | BindingFlags.Public)]
        [SteamCall]
        public void askChat(CSteamID steamID, byte mode, string text)
        {
            // Set the variables
            bool cancel = false;
            UnturnedPlayer player = UnturnedPlayer.Get(steamID);

            // Run methods
            ChatEvents.RunChatted(ref player, ref mode, ref text, ref cancel);

            // Do checks
            if (player?.SteamID == null || player.SteamID == CSteamID.Nil)
                return;
            if (string.IsNullOrEmpty(text))
                return;
            if (cancel)
                return;

            // Restore original
            PointBlankDetourManager.CallOriginal(mi_askChat, CM.instance, player.SteamID, mode, text);
        }
    }
}
