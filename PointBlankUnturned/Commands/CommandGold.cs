using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandGold : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Gold"
        };

        public override string Help => Translations["Gold_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.gold";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            Provider.isGold = true;
            UnturnedChat.SendMessage(executor, Translations["Gold_Set"], ConsoleColor.Green);
        }
    }
}
