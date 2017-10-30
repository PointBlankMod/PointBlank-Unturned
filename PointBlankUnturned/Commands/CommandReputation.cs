﻿using System;
using PointBlank.API.Commands;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandReputation : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Rep",
            "Reputation"
        };

        public override string Help => Translations["Reputation_Help"];

        public override string Usage => Commands[0] + Translations["Reputation_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.reputation";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedPlayer[] players = new UnturnedPlayer[1];
            players[0] = (UnturnedPlayer)executor;

            if(!int.TryParse(args[0], out int rep))
            {
                UnturnedChat.SendMessage(executor, Translations["Reputation_Invalid"], ConsoleColor.Red);
                return;
            }
            if(args.Length > 1)
            {
                if(!UnturnedPlayer.TryGetPlayers(args[1], out players))
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }
            }

            players.ForEach((player) =>
            {
                if (UnturnedPlayer.IsServer(player))
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }

                player.Player.skills.askRep(rep);
                UnturnedChat.SendMessage(executor, string.Format(Translations["Reputation_Give"], player.PlayerName, rep), ConsoleColor.Green);
            });
        }
    }
}
