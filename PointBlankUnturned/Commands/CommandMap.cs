using System;
using PointBlank.API.Server;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandMap : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = PointBlankUnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Map"
        };

        public override string Help => Translations["Map_Help"];

        public override string Usage => Commands[0] + Translations["Map_Usage"];

        public override string DefaultPermission => "unturned.commands.server.map";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if (!Level.exists(args[0]))
            {
                UnturnedChat.SendMessage(executor, Translations["Map_Invalid"], ConsoleColor.Red);
                return;
            }

            if (PointBlankServer.IsRunning)
                UnturnedServer.ChangeMap(args[0]);
            else
                Provider.map = args[0];
            
            UnturnedChat.SendMessage(executor, string.Format(Translations["Map_Set"], args[0]), ConsoleColor.Green);
        }
    }
}
