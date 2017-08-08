using System;
using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    internal class CommandUsage : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Usage"
        };

        public override string Help => Translations["Usage_Help"];

        public override string Usage => Translations["Usage_Usage"];

        public override string DefaultPermission => "pointblank.commands.nonadmin.usage";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            PointBlankCommand cmd = CommandManager.Commands.FirstOrDefault(a => a.Commands.FirstOrDefault(b => b.ToLower() == args[0].ToLower()) != null && a.Enabled);

            if (cmd == null)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_CommandInvalid"], ConsoleColor.Red);
                return;
            }
            UnturnedChat.SendMessage(executor, (executor == null ? "" : "/") + cmd.Usage, ConsoleColor.Green);
        }
    }
}
