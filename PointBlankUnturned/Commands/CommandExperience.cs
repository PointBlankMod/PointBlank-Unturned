﻿using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    internal class CommandExperience : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "xp",
            "Experience"
        };

        public override string Help => Translations["Experience_Help"];

        public override string Usage => Commands[0] + Translations["Experience_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.experience";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!uint.TryParse(args[0], out uint xp))
            {
                UnturnedChat.SendMessage(executor, Translations["Experience_Invalid"], ConsoleColor.Red);
                return;
            }
            if(args.Length < 2 || !UnturnedPlayer.TryGetPlayer(args[1], out UnturnedPlayer player))
            {
                if(executor == null)
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }

                player = (UnturnedPlayer)executor;
            }

            player.Player.skills.askAward(xp);
            UnturnedChat.SendMessage(executor, string.Format(Translations["Experience_Give"], player.PlayerName, xp), ConsoleColor.Green);
        }
    }
}
