using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("PvE", 0)]
    internal class CommandPvE : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "PvE"
        };

        public override string Help => Translations["PvE_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.pve";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            Provider.isPvP = false;
            UnturnedChat.SendMessage(executor, Translations["PvE_Set"], ConsoleColor.Green);
        }
    }
}
