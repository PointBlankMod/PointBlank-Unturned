﻿using System;
using PointBlank.API.Commands;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using Steamworks;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandSpy : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Spy"
        };

        public override string Help => Translations["Spy_Help"];

        public override string Usage => Commands[0] + Translations["Spy_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.spy";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!UnturnedPlayer.TryGetPlayers(args[0], out UnturnedPlayer[] players))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }

            players.ForEach((player) =>
            {
                if (UnturnedPlayer.IsServer(player))
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }
                if(player == (UnturnedPlayer)executor)
                    return;

                player.Player.sendScreenshot(((UnturnedPlayer)executor)?.SteamID ?? CSteamID.Nil, null);
                UnturnedChat.SendMessage(executor, string.Format(Translations["Spy_Spy"], player.PlayerName), ConsoleColor.Red);
            });
        }
    }
}
