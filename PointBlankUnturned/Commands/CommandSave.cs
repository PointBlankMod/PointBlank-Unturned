using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandSave : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Save"
        };

        public override string Help => Translations["Save_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.save";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            SaveManager.save();
            UnturnedChat.SendMessage(executor, Translations["Save_Save"], ConsoleColor.Green);
        }
    }
}
