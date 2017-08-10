using System;
using System.Linq;
using PointBlank.API.Commands;
using UnityEngine;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
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

            if(executor == null)
            {
                CommandWindow.Log(Translations["Admins_List"] + admins, ConsoleColor.Green);
            }
            else
            {
                if(Provider.hideAdmins && !executor.HasPermission("unturned.revealadmins"))
                {
                    executor.SendMessage(Translations["Admins_Hidden"], Color.red);
                    return;
                }

                executor.SendMessage(Translations["Admins_List"] + admins, Color.green);
            }
        }
    }
}
