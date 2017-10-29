using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandHideAdmins : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "HideAdmins"
        };

        public override string Help => Translations["HideAdmins_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.hideadmins";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            Provider.hideAdmins = true;
            UnturnedChat.SendMessage(executor, Translations["HideAdmins_Set"], ConsoleColor.Green);
        }
    }
}
