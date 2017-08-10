using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandMode : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Mode"
        };

        public override string Help => Translations["Mode_Help"];

        public override string Usage => Commands[0] + Translations["Mode_Usage"];

        public override string DefaultPermission => "unturned.commands.server.mode";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            switch(args[0].ToUpperInvariant())
            {
                case "EASY":
                    Provider.mode = EGameMode.EASY;
                    break;
                case "NORMAL":
                    Provider.mode = EGameMode.NORMAL;
                    break;
                case "HARD":
                    Provider.mode = EGameMode.HARD;
                    break;
                default:
                    UnturnedChat.SendMessage(executor, Translations["Mode_Invalid"], ConsoleColor.Red);
                    break;
            }

            UnturnedChat.SendMessage(executor, string.Format(Translations["Mode_Set"], Provider.mode), ConsoleColor.Green);
        }
    }
}
