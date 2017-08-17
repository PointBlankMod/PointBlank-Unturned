using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Groups;
using PointBlank.API.Services;
using PointBlank.API.Plugins;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Structure;
using PointBlank.API.Unturned.Barricade;
using PointBlank.API.Unturned.Item;
using Typ = SDG.Unturned.Types;
using Steamworks;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.Services.APIManager
{
    internal class APIManager : PointBlankService
    {
        #region Variables
        // Thread variables
        private static Thread tGame;
        private static bool RunThread = true;
        #endregion

        #region Properties
        public override int LaunchIndex => 2;
        #endregion

        public override void Load()
        {
            // Setup thread
            tGame = new Thread(new ThreadStart(delegate ()
            {
                while (RunThread)
                    ServerEvents.RunThreadTick();
            }));

            // Setup events
            Provider.onEnemyConnected += ServerEvents.RunPlayerConnected;
            Provider.onEnemyDisconnected += ServerEvents.RunPlayerDisconnected;
            Provider.onServerShutdown += ServerEvents.RunServerShutdown;
            Provider.onServerHosted += ServerEvents.RunServerInitialized;
            Provider.onCheckValid += OnPlayerPreConnect;
            LightingManager.onDayNightUpdated += ServerEvents.RunDayNight;
            LightingManager.onMoonUpdated += ServerEvents.RunFullMoon;
            LightingManager.onRainUpdated += ServerEvents.RunRainUpdated;

            ChatManager.onChatted += OnPlayerChat;
            CommandWindow.onCommandWindowInputted += OnConsoleCommand;
            ChatManager.onCheckPermissions += OnUnturnedCommand;
            ItemManager.onItemDropAdded += OnItemDropAdded;
            ItemManager.onItemDropRemoved += OnItemDropRemoved;

            // Setup pointblank events
            ServerEvents.OnPlayerConnected += OnPlayerJoin;
            ServerEvents.OnPlayerDisconnected += OnPlayerLeave;
            PlayerEvents.OnListPlayerRemoved += OnListPlayerRemove;
            PlayerEvents.OnListPlayerAdded += OnListPlayerAdd;
            ServerEvents.OnServerInitialized += OnServerInitialized;
            ServerEvents.OnPacketSent += OnPacketSend;
            PlayerEvents.OnPrefixAdded += OnPrefixChange;
            PlayerEvents.OnPrefixRemoved += OnPrefixChange;
            PlayerEvents.OnSuffixAdded += OnSuffixChange;
            PlayerEvents.OnSuffixRemoved += OnSuffixChange;
            PointBlankPlayerEvents.OnGroupAdded += OnGroupChange;
            PointBlankPlayerEvents.OnGroupRemoved += OnGroupChange;
            PlayerEvents.OnPlayerDied += OnPlayerDie;
            PlayerEvents.OnPlayerKill += OnPlayerKill;

            StructureEvents.OnDestroyStructure += ServerEvents.RunStructureRemoved;
            StructureEvents.OnSalvageStructure += ServerEvents.RunStructureRemoved;
            BarricadeEvents.OnBarricadeDestroy += ServerEvents.RunBarricadeRemoved;
            BarricadeEvents.OnBarricadeSalvage += ServerEvents.RunBarricadeRemoved;

            // Run code
            tGame.Start();
        }

        public override void Unload()
        {
            // Unload events
            Provider.onEnemyConnected -= ServerEvents.RunPlayerConnected;
            Provider.onEnemyDisconnected -= ServerEvents.RunPlayerDisconnected;
            Provider.onServerShutdown -= ServerEvents.RunServerShutdown;
            Provider.onServerHosted -= ServerEvents.RunServerInitialized;
            Provider.onCheckValid -= OnPlayerPreConnect;
            LightingManager.onDayNightUpdated -= ServerEvents.RunDayNight;
            LightingManager.onMoonUpdated -= ServerEvents.RunFullMoon;
            LightingManager.onRainUpdated -= ServerEvents.RunRainUpdated;

            ChatManager.onChatted -= OnPlayerChat;
            CommandWindow.onCommandWindowInputted -= OnConsoleCommand;
            ChatManager.onCheckPermissions -= OnUnturnedCommand;
            ItemManager.onItemDropAdded -= OnItemDropAdded;
            ItemManager.onItemDropRemoved -= OnItemDropRemoved;

            // Unload pointblank events
            ServerEvents.OnPlayerConnected -= OnPlayerJoin;
            ServerEvents.OnPlayerDisconnected -= OnPlayerLeave;
            PlayerEvents.OnListPlayerRemoved -= OnListPlayerRemove;
            PlayerEvents.OnListPlayerAdded -= OnListPlayerAdd;
            ServerEvents.OnServerInitialized -= OnServerInitialized;
            ServerEvents.OnPacketSent -= OnPacketSend;
            PlayerEvents.OnPrefixAdded -= OnPrefixChange;
            PlayerEvents.OnPrefixRemoved -= OnPrefixChange;
            PlayerEvents.OnSuffixAdded -= OnSuffixChange;
            PlayerEvents.OnSuffixRemoved -= OnSuffixChange;
            PointBlankPlayerEvents.OnGroupAdded -= OnGroupChange;
            PointBlankPlayerEvents.OnGroupRemoved -= OnGroupChange;
            PlayerEvents.OnPlayerDied -= OnPlayerDie;
            PlayerEvents.OnPlayerKill -= OnPlayerKill;

            StructureEvents.OnDestroyStructure -= ServerEvents.RunStructureRemoved;
            StructureEvents.OnSalvageStructure -= ServerEvents.RunStructureRemoved;
            BarricadeEvents.OnBarricadeDestroy -= ServerEvents.RunBarricadeRemoved;
            BarricadeEvents.OnBarricadeSalvage -= ServerEvents.RunBarricadeRemoved;

            // Stop the thread
            RunThread = false;
            tGame.Abort();
        }

        #region Mono Functions
        void Update() => ServerEvents.RunGameTick();
        #endregion

        #region Event Functions
        private void OnItemDropAdded(Transform model, InteractableItem item)
        {
            UnturnedItem itm = UnturnedItem.Create(item);

            ServerEvents.RunItemCreated(itm);
        }
        private void OnItemDropRemoved(Transform model, InteractableItem item)
        {
            UnturnedItem itm = UnturnedItem.Create(item);

            ServerEvents.RunItemRemoved(itm);
        }

        private void OnPlayerPreConnect(ValidateAuthTicketResponse_t AuthTicket, ref bool valid)
        {
            ESteamRejection? reject = null;

            try
            {
                ServerEvents.RunPlayerPreConnect(AuthTicket.m_SteamID, ref reject);
                if (reject.HasValue)
                {
                    Provider.reject(AuthTicket.m_SteamID, reject.Value);
                    valid = false;
                }
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error in pre-connect event!", ex);
            }
        }
        private void OnPlayerJoin(UnturnedPlayer player)
        {
            PointBlankGroup[] groups = PointBlankGroupManager.Groups.Where(a => a.Default).ToArray();

            player.Loaded = false;
            foreach (PointBlankGroup g in groups)
                if (!player.Groups.Contains(g))
                    player.AddGroup(g);
            player.Loaded = true;

            UnturnedServer.Players.ForEach((ply) =>
            {
                if (!player.PlayerList.Contains(ply))
                    player.AddPlayer(ply, false);
                if (!ply.PlayerList.Contains(player))
                    ply.AddPlayer(player, false);
            });
        }
        private void OnPlayerLeave(UnturnedPlayer player)
        {
            UnturnedServer.Players.ForEach((ply) =>
            {
                if (ply == player)
                    return;

                if (ply.PlayerList.Contains(player))
                    ply.RemovePlayer(player, false);
            });
            UnturnedServer.RemovePlayer(player);
        }
        private void OnPlayerChat(SteamPlayer player, EChatMode mode, ref UnityEngine.Color color, string text, ref bool visible)
        {
            UnturnedPlayer ply = UnturnedPlayer.Get(player);
            UnityEngine.Color c = ply.GetColor();

            if (c != UnityEngine.Color.clear)
                color = c;
        }
        private void OnListPlayerRemove(UnturnedPlayer player, UnturnedPlayer target)
        {
            Provider.send(player.SteamID, ESteamPacket.DISCONNECTED, new byte[]
            {
                (byte)12,
                (byte)(Provider.clients.FindIndex(a => a == target.SteamPlayer))
            }, 2, 0);
        }
        private void OnListPlayerAdd(UnturnedPlayer player, UnturnedPlayer target)
        {
            if (!UnturnedPlayer.IsInServer(target))
                return;

            byte[] bytes = SteamPacker.getBytes(0, out int size, new object[]
            {
                (byte)11,
                target.SteamPlayerID.steamID,
                target.SteamPlayerID.characterID,
                target.PlayerName,
                target.CharacterName,
                target.SteamPlayer.model.transform.position,
                (byte)(target.SteamPlayer.model.transform.rotation.eulerAngles.y / 2f),
                target.SteamPlayer.isPro,
                target.SteamPlayer.isAdmin && !Provider.hideAdmins,
                target.SteamPlayer.channel,
                target.SteamPlayer.playerID.group,
                target.NickName,
                target.SteamPlayer.face,
                target.SteamPlayer.hair,
                target.SteamPlayer.beard,
                target.SteamPlayer.skin,
                target.SteamPlayer.color,
                target.SteamPlayer.hand,
                target.SteamPlayer.shirtItem,
                target.SteamPlayer.pantsItem,
                target.SteamPlayer.hatItem,
                target.SteamPlayer.backpackItem,
                target.SteamPlayer.vestItem,
                target.SteamPlayer.maskItem,
                target.SteamPlayer.glassesItem,
                target.SteamPlayer.skinItems,
                (byte)target.SteamPlayer.skillset,
                target.SteamPlayer.language
            });

            Provider.send(player.SteamID, ESteamPacket.CONNECTED, bytes, size, 0);
        }
        private void OnPrefixChange(UnturnedPlayer player, string prefix)
        {
            player.CharacterName = player.GetPrefix() + player.UnturnedCharacterName + player.GetSuffix();
            player.NickName = player.GetPrefix() + player.UnturnedNickName + player.GetSuffix();

            for (int i = 0; i < UnturnedServer.Players.Length; i++)
            {
                if (UnturnedServer.Players[i].SteamID == player.SteamID)
                    continue;

                UnturnedServer.Players[i].RemovePlayer(player);
                UnturnedServer.Players[i].AddPlayer(player);
            }
        }
        private void OnSuffixChange(UnturnedPlayer player, string suffix)
        {
            player.CharacterName = player.GetPrefix() + player.UnturnedCharacterName + player.GetSuffix();
            player.NickName = player.GetPrefix() + player.UnturnedNickName + player.GetSuffix();

            for (int i = 0; i < UnturnedServer.Players.Length; i++)
            {
                if (UnturnedServer.Players[i].SteamID == player.SteamID)
                    continue;

                UnturnedServer.Players[i].RemovePlayer(player);
                UnturnedServer.Players[i].AddPlayer(player);
            }
        }
        private void OnGroupChange(PointBlankPlayer pbPlayer, PointBlankGroup group)
        {
            UnturnedPlayer player = (UnturnedPlayer)pbPlayer;
            player.CharacterName = player.GetPrefix() + player.UnturnedCharacterName + player.GetSuffix();
            player.NickName = player.GetPrefix() + player.UnturnedNickName + player.GetSuffix();

            for (int i = 0; i < UnturnedServer.Players.Length; i++)
            {
                if (UnturnedServer.Players[i].SteamID == player.SteamID)
                    continue;

                UnturnedServer.Players[i].RemovePlayer(player);
                UnturnedServer.Players[i].AddPlayer(player);
            }
        }
        private void OnPlayerDie(UnturnedPlayer player, ref EDeathCause cause, ref UnturnedPlayer killer)
        {
            player.Deaths++;
            player.TotalDeaths++;
        }
        private void OnPlayerKill(UnturnedPlayer player, ref EDeathCause cause, ref UnturnedPlayer victim)
        {
            if (player == null)
                return;

            player.Kills++;
            player.TotalKills++;
        }

        private void OnConsoleCommand(string text, ref bool shouldExecute)
        {
            shouldExecute = false;
            if (text.StartsWith("/") || text.StartsWith("@"))
                text = text.Remove(0, 1);

            PointBlankCommandManager.ExecuteCommand(text, null);
        }
        private void OnUnturnedCommand(SteamPlayer player, string text, ref bool shouldExecuteCommand, ref bool shouldList)
        {
            shouldExecuteCommand = false;
            if (!text.StartsWith("/") && !text.StartsWith("@")) return;
            shouldList = false;

            PointBlankCommandManager.ExecuteCommand(text, UnturnedPlayer.Get(player));
        }

        private void OnServerInitialized()
        {
            SteamGameServer.SetKeyValue("untured", Provider.APP_VERSION);
            SteamGameServer.SetKeyValue("pointblank", PointBlankInfo.Version);
            string plugins = string.Join(",", PointBlankPluginManager.LoadedPlugins.Select(a => PointBlankPluginManager.GetPluginName(a)).ToArray());
            string pluginsRocket = string.Join(",", PointBlankPluginManager.LoadedPlugins.Select(a => PointBlankPluginManager.GetPluginName(a) + " - PointBlank").ToArray());
            SteamGameServer.SetKeyValue("pointblankplugins", plugins);
            SteamGameServer.SetKeyValue("rocketplugins", "PointBlank - not really a Rocket plugin but a mod loader," + pluginsRocket); // Since unturned only supports the rocket plugin list I need to use this
            PointBlankServer.IsRunning = true;
        }
        private void OnPacketSend(ref CSteamID steamID, ref ESteamPacket type, ref byte[] packet, ref int size, ref int channel, ref bool cancel)
        {
            if (type == ESteamPacket.CONNECTED)
            {
                object[] info = SteamPacker.getObjects(steamID, 0, 0, packet, new Type[]
                {
                    Typ.BYTE_TYPE,
                    Typ.STEAM_ID_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.STRING_TYPE,
                    Typ.STRING_TYPE,
                    Typ.VECTOR3_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.BOOLEAN_TYPE,
                    Typ.BOOLEAN_TYPE,
                    Typ.INT32_TYPE,
                    Typ.STEAM_ID_TYPE,
                    Typ.STRING_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.COLOR_TYPE,
                    Typ.COLOR_TYPE,
                    Typ.BOOLEAN_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_ARRAY_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.STRING_TYPE
                });
                UnturnedPlayer player = UnturnedPlayer.Get((CSteamID)info[1]);

                if (player.SteamID != steamID)
                {
                    info[3] = player.PlayerName;
                    info[4] = player.CharacterName;
                    info[11] = player.NickName;
                }
                else
                {
                    info[3] = player.PlayerName;
                    info[4] = player.UnturnedCharacterName;
                    info[11] = player.UnturnedNickName;
                }

                packet = SteamPacker.getBytes(0, out size, info);
            }
            else if(type == ESteamPacket.DISCONNECTED)
            {
                UnturnedPlayer player = UnturnedPlayer.Get(steamID);
                SteamPlayer target = Provider.clients[packet[1]];

                packet[1] = (byte)Array.FindIndex(player.PlayerList, a => a.SteamPlayer == target);
            }
        }
        #endregion
    }
}
