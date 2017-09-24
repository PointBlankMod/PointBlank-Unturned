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
	internal class OvChatManager
	{
        #region Reflection
        private static MethodInfo _miAskChat = PointBlankReflect.GetMethod<CM>("askChat", BindingFlags.Public | BindingFlags.Instance);
        #endregion

        [Detour(typeof(CM), "askChat", BindingFlags.Instance | BindingFlags.Public)]
        [SteamCall]
        public void AskChat(CSteamID steamId, byte mode, string text)
        {
            // Set the variables
            bool cancel = false;
            UnturnedPlayer player = UnturnedPlayer.Get(steamId);

            // Run methods
            ChatEvents.RunChatted(ref player, ref mode, ref text, ref cancel);

            // Do checks
            if (player?.SteamId == null || player.SteamId == CSteamID.Nil)
                return;
            if (string.IsNullOrEmpty(text))
                return;
            if (cancel)
                return;

            // Restore original
            PointBlankDetourManager.CallOriginal(_miAskChat, CM.instance, player.SteamId, mode, text);
        }
    }
}
