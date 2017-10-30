using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandCheats : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Cheats"
        };

        public override string Help => Translations["Cheats_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.cheats";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            Provider.hasCheats = true;
            UnturnedChat.SendMessage(executor, Translations["Cheats_Enabled"], ConsoleColor.Green);
        }
    }
}
