﻿using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandName : PointBlankCommand
    {
        #region Info
        private static readonly byte MIN_LENGTH = 5;
        private static readonly byte MAX_LENGTH = 50;
        #endregion

        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Name"
        };

        public override string Help => Translations["Name_Help"];

        public override string Usage => Commands[0] + Translations["Name_Usage"];

        public override string DefaultPermission => "unturned.commands.server.name";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            string name = string.Join(" ", args);

            if(name.Length > MAX_LENGTH)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Name_TooLong"], MAX_LENGTH), ConsoleColor.Red);
                return;
            }
            else if(name.Length < MIN_LENGTH)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Name_TooShort"], MIN_LENGTH), ConsoleColor.Red);
                return;
            }

            Provider.serverName = name;
            UnturnedChat.SendMessage(executor, string.Format(Translations["Name_Set"], name), ConsoleColor.Green);
        }
    }
}
