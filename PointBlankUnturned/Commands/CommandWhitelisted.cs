using System;
using PointBlank.API.Player;
using PointBlank.API.Commands;
using SDG.Unturned;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandWhitelisted : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Whitelisted"
        };

        public override string Help => Translations["Whitelisted_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.whitelisted";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            Provider.isWhitelisted = true;
            PointBlankPlayer.SendMessage(executor, Translations["Whitelisted_Set"], ConsoleColor.Green);
        }
    }
}
