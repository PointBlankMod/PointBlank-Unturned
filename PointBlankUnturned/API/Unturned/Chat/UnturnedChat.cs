using System;
using UnityEngine;
using Steamworks;
using SDG.Unturned;
using PointBlank.API.Player;
using CM = SDG.Unturned.ChatManager;

namespace PointBlank.API.Unturned.Chat
{
    /// <summary>
    /// The unturned chat manager
    /// </summary>
    public static class UnturnedChat
    {
        #region Public Functions
        /// <summary>
        /// Broadcasts a message to all the players
        /// </summary>
        /// <param name="text">The text to broadcast</param>
        /// <param name="color">The color of the broadcast</param>
        public static void Broadcast(object text, Color color) => CM.say(text.ToString(), color);

        /// <summary>
        /// Sends a fake global message as a user
        /// </summary>
        /// <param name="sender">The person that "sent" the message</param>
        /// <param name="text">The text he sent</param>
        /// <param name="color">The color of the message</param>
        public static void FakeMessage(CSteamID sender, string text, Color color)
        {
            if (text.Length > CM.LENGTH)
                text = text.Substring(0, CM.LENGTH);

            CM.instance.channel.send("tellChat", ESteamCall.CLIENTS, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
            {
                sender,
                0,
                color,
                text
            });
        }

        /// <summary>
        /// Sends a message to the player or the console
        /// </summary>
        /// <param name="player">The player(null if the console)</param>
        /// <param name="text">The message</param>
        /// <param name="color">The message color</param>
        public static void SendMessage(PointBlankPlayer player, object text, ConsoleColor color = ConsoleColor.White) => PointBlankPlayer.SendMessage(player, text, color);

        /// <summary>
        /// Broadcast a message to the entire server
        /// </summary>
        /// <param name="message">The message to broadcast</param>
        public static void Say(object message) => Broadcast(message, Color.magenta);
        /// <summary>
        /// Broadcast a message to the entire server
        /// </summary>
        /// <param name="message">The message to broadcast</param>
        /// <param name="color">The color of the message</param>
        public static void Say(object message, Color color) => Broadcast(message, color);
        /// <summary>
        /// Sends a message to a specific player or to the server
        /// </summary>
        /// <param name="player">The player to send the message to(null if server)</param>
        /// <param name="message">The message to send to the player/server</param>
        /// <param name="color">The color of the message</param>
        public static void Say(PointBlankPlayer player, object message, ConsoleColor color = ConsoleColor.White) => SendMessage(player, message, color);
        #endregion
    }
}
