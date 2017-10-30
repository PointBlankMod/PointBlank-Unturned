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
    public class CommandBans : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Bans"
        };

        public override string Help => Translations["Bans_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.bans";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedChat.SendMessage(executor, Translations["Bans_List"] + string.Join(",", SteamBlacklist.list.Select(a => a.playerID.ToString()).ToArray()), ConsoleColor.Green);
        }
    }
}
