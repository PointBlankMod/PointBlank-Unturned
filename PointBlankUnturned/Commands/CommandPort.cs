using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandPort : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Port"
        };

        public override string Help => Translations["Port_Help"];

        public override string Usage => Commands[0] + Translations["Port_Usage"];

        public override string DefaultPermission => "unturned.commands.server.port";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!ushort.TryParse(args[0], out ushort port))
            {
                UnturnedChat.SendMessage(executor, Translations["Port_Invalid"], ConsoleColor.Red);
                return;
            }

            Provider.port = port;
            UnturnedChat.SendMessage(executor, string.Format(Translations["Port_Set"], port), ConsoleColor.Green);
        }
    }
}
