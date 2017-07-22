using System;
using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Permits", 0)]
    internal class CommandPermits : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Permits"
        };

        public override string Help => Translations["Permits_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.permits";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedChat.SendMessage(executor, string.Format(Translations["Permits_List"], string.Join(",", SteamWhitelist.list.Select(a => a.steamID.ToString()).ToArray())), ConsoleColor.Green);
        }
    }
}
