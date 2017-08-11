﻿using System;
using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandHelp : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Help"
        };

        public override string Help => Translations["Help_Help"];

        public override string Usage => Commands[0] + Translations["Help_Usage"];

        public override string DefaultPermission => "unturned.commands.nonadmin.help";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(args.Length > 0)
            {
                PointBlankCommand cmd = PointBlankCommandManager.Commands.FirstOrDefault(a => a.Commands.FirstOrDefault(b => b.ToLower() == args[0].ToLower()) != null && a.Enabled);

                if(cmd == null)
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_CommandInvalid"], ConsoleColor.Red);
                    return;
                }
                UnturnedChat.SendMessage(executor, cmd.Help, ConsoleColor.Green);
                return;
            }
            int pos = 0;
            int prevPos = 0;
            string send = "";
            while (true)
            {
                if(pos >= PointBlankCommandManager.Commands.Length)
                {
                    UnturnedChat.SendMessage(executor, send, ConsoleColor.Green);
                    break;
                }
                if(pos - prevPos >= 3)
                {
                    UnturnedChat.SendMessage(executor, send, ConsoleColor.Green);
                    send = "";
                    prevPos = pos;
                }

                send += (string.IsNullOrEmpty(send) ? "" : ",") + PointBlankCommandManager.Commands[pos].Commands[0];
                pos++;
            }
        }
    }
}
