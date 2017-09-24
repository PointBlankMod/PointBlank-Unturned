using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandWelcome : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = PointBlankUnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Welcome"
        };

        public override string Help => Translations["Welcome_Help"];

        public override string Usage => Commands[0] + Translations["Welcome_Usage"];

        public override string DefaultPermission => "unturned.commands.server.welcome";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            ChatManager.welcomeText = string.Join(" ", args);
            UnturnedChat.SendMessage(executor, Translations["Welcome_Set"], ConsoleColor.Green);
        }
    }
}
