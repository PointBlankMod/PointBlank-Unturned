﻿using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandSlay : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = PointBlankUnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Slay"
        };

        public override string Help => Translations["Slay_Help"];

        public override string Usage => Commands[0] + Translations["Slay_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.slay";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer ply))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }

            SteamBlacklist.ban(ply.SteamId, ply.SteamIp, ((UnturnedPlayer)executor)?.SteamId ?? CSteamID.Nil, (args.Length > 1 ? args[1] : "Undefined"), SteamBlacklist.PERMANENT);
            if (ply.SteamPlayer.player != null)
                ply.Player.life.askDamage(255, Vector3.up * 101f, EDeathCause.KILL, ELimb.SKULL, ((UnturnedPlayer)executor)?.SteamId ?? CSteamID.Nil, out EPlayerKill ePlayerKill);
            UnturnedChat.SendMessage(executor, string.Format(Translations["Slay_Slay"], ply.PlayerName), ConsoleColor.Red);
        }
    }
}
