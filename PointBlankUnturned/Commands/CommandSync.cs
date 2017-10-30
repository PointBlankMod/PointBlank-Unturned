using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandSync : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Sync"
        };

        public override string Help => Translations["Sync_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.sync";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            PlayerSavedata.hasSync = true;
            UnturnedChat.SendMessage(executor, Translations["Sync_Sync"], ConsoleColor.Green);
        }
    }
}
