using System;
using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using UnityEngine;
using SDG.Unturned;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandAdmins : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Admins"
        };

        public override string Help => Translations["Admins_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.nonadmin.admins";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            string admins = string.Join(",", Provider.clients.Where(a => a.isAdmin).Select(a => a.playerID.playerName).ToArray());

            if(!UnturnedPlayer.IsServer(executor) && Provider.hideAdmins && !executor.HasPermission("unturned.revealadmins"))
            {
                executor.SendMessage(Translations["Admins_Hidden"], Color.red);
                return;
            }

            UnturnedChat.SendMessage(executor, Translations["Admins_List"] + admins, ConsoleColor.Green);
        }
    }
}
