using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    internal class CommandTimeout : PointBlankCommand
    {
        #region Info
        private static readonly ushort MIN_NUMBER = 50;
        private static readonly ushort MAX_NUMBER = 10000;
        #endregion

        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Timeout"
        };

        public override string Help => Translations["Timeout_Help"];

        public override string Usage => Commands[0] + Translations["Timeout_Usage"];

        public override string DefaultPermission => "unturned.commands.server.timeout";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!uint.TryParse(args[0], out uint timeout))
            {
                UnturnedChat.SendMessage(executor, Translations["Timeout_Invalid"], ConsoleColor.Red);
                return;
            }
            if(timeout > MAX_NUMBER)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Timeout_TooHigh"], MAX_NUMBER), ConsoleColor.Red);
                return;
            }
            else if(timeout < MIN_NUMBER)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Timeout_TooLow"], MIN_NUMBER), ConsoleColor.Red);
                return;
            }

            if (Provider.configData != null)
                Provider.configData.Server.Max_Ping_Milliseconds = timeout;
            UnturnedChat.SendMessage(executor, string.Format(Translations["Timeout_Set"], timeout), ConsoleColor.Green);
        }
    }
}
