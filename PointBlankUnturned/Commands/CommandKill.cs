using System;
using PointBlank.API.Commands;
using PointBlank.API.Implements;
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
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

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
            UnturnedPlayer[] players = new UnturnedPlayer[1];
            players[0] = (UnturnedPlayer)executor;

            if(args.Length > 0)
            {
                if(!UnturnedPlayer.TryGetPlayers(args[0], out players))
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

                player.Player.life.askDamage(255, Vector3.up * 10f, EDeathCause.KILL, ELimb.SKULL, ((UnturnedPlayer)executor)?.SteamID ?? CSteamID.Nil, out EPlayerKill kill);
                UnturnedChat.SendMessage(executor, string.Format(Translations["Kill_Killed"], player.PlayerName), ConsoleColor.Green);
            });
        }
    }
}
