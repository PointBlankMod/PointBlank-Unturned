﻿using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandKill : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Kill"
        };

        public override string Help => Translations["Kill_Help"];

        public override string Usage => Commands[0] + Translations["Kill_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.kill";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(args.Length < 1 || UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer ply))
            {
                if(executor == null)
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }

                ply = (UnturnedPlayer)executor;
            }

            ply.Player.life.askDamage(255, Vector3.up * 10f, EDeathCause.KILL, ELimb.SKULL, ((UnturnedPlayer)executor)?.SteamID ?? CSteamID.Nil, out EPlayerKill kill);
            UnturnedChat.SendMessage(executor, string.Format(Translations["Kill_Killed"], ply.PlayerName), ConsoleColor.Green);
        }
    }
}
