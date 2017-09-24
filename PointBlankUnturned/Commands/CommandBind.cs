﻿using System;
using PointBlank.API.Commands;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandBind : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = PointBlankUnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Bind"
        };

        public override string Help => Translations["Bind_Help"];

        public override string Usage => Commands[0] + Translations["Bind_Usage"];

        public override string DefaultPermission => "unturned.commands.server.bind";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if (!Parser.checkIP(args[0]))
            {
                CommandWindow.Log(Translations["Bind_InvalidIP"], ConsoleColor.Red);
                return;
            }

            Provider.ip = Parser.getUInt32FromIP(args[0]);
        }
    }
}
