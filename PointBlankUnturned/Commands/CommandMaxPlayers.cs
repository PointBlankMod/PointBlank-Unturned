﻿using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandMaxPlayers : PointBlankCommand
    {
        #region Info
        public static readonly byte MinNumber = 1;
        public static readonly byte MaxNumber = 48;
        #endregion

        #region Properties
        public TranslationList Translations = PointBlankUnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "MaxPlayers"
        };

        public override string Help => Translations["MaxPlayers_Help"];

        public override string Usage => Commands[0] + Translations["MaxPlayers_Usage"];

        public override string DefaultPermission => "unturned.commands.server.maxplayers";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!byte.TryParse(args[0], out byte max))
            {
                UnturnedChat.SendMessage(executor, Translations["MaxPlayers_Invalid"], ConsoleColor.Red);
                return;
            }

            if(max > MaxNumber)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["MaxPlayers_TooHigh"], MaxNumber), ConsoleColor.Red);
                return;
            }
            else if(max < MinNumber)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["MaxPlayers_TooLow"], MinNumber), ConsoleColor.Red);
                return;
            }

            Provider.maxPlayers = max;
            UnturnedChat.SendMessage(executor, string.Format(Translations["MaxPlayers_Set"], max), ConsoleColor.Green);
        }
    }
}
