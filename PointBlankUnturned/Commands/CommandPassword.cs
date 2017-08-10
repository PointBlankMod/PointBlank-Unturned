using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandPassword : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Password"
        };

        public override string Help => Translations["Password_Help"];

        public override string Usage => Commands[0] + Translations["Password_Usage"];

        public override string DefaultPermission => "unturned.commands.server.password";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(args.Length < 1 || args[0].Length < 1)
            {
                Provider.serverPassword = string.Empty;
                UnturnedChat.SendMessage(executor, Translations["Password_Removed"], ConsoleColor.Green);
                return;
            }

            Provider.serverPassword = args[0];
            UnturnedChat.SendMessage(executor, string.Format(Translations["Password_Set"], args[0]), ConsoleColor.Green);
        }
    }
}
