using System;
using PointBlank.API.Commands;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandLog : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Log"
        };

        public override string Help => Translations["Log_Help"];

        public override string Usage => Commands[0] + Translations["Log_Usage"];

        public override string DefaultPermission => "unturned.commands.server.log";

        public override EAllowedCaller AllowedCaller => EAllowedCaller.SERVER;

        public override int MinimumParams => 4;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            CommandWindow.shouldLogChat = (args[0].ToLower() == "y");
            CommandWindow.shouldLogJoinLeave = (args[1].ToLower() == "y");
            CommandWindow.shouldLogDeaths = (args[2].ToLower() == "y");
            CommandWindow.shouldLogAnticheat = (args[3].ToLower() == "y");
            CommandWindow.Log(Translations["Log_Set"], ConsoleColor.Green);
        }
    }
}
