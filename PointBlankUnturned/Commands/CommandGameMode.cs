using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    internal class CommandGameMode : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "GameMode"
        };

        public override string Help => Translations["GameMode_Help"];

        public override string Usage => Commands[0] + Translations["GameMode_Usage"];

        public override string DefaultPermission => "unturned.commands.server.gamemode";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            Provider.selectedGameModeName = args[0];
            UnturnedChat.SendMessage(executor, string.Format(Translations["GameMode_Set"], args[0]), ConsoleColor.Green);
        }
    }
}
