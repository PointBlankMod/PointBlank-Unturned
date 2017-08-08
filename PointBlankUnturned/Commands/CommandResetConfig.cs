using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    internal class CommandResetConfig : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "ResetConfig"
        };

        public override string Help => Translations["ResetConfig_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.resetconfig";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            Provider.resetConfig();
            UnturnedChat.SendMessage(executor, Translations["ResetConfig_Reset"], ConsoleColor.Green);
        }
    }
}
