﻿using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandNight : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Night"
        };

        public override string Help => Translations["Night_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.night";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if (Provider.isServer && Level.info.type == ELevelType.ARENA)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NoArenaTime"], ConsoleColor.Red);
                return;
            }
            if (Provider.isServer && Level.info.type == ELevelType.HORDE)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NoHordeTime"], ConsoleColor.Red);
                return;
            }

            LightingManager.time = (uint)(LightingManager.cycle * (LevelLighting.bias + LevelLighting.transition));
            UnturnedChat.SendMessage(executor, Translations["Night_Set"], ConsoleColor.Green);
        }
    }
}
