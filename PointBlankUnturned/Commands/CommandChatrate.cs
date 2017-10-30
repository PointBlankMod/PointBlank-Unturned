﻿using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandChatrate : PointBlankCommand
    {
        #region Info
        private static readonly float MIN_NUMBER = 1f;
        private static readonly float MAX_NUMBER = 60f;
        #endregion

        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Chatrate"
        };

        public override string Help => Translations["Chatrate_Help"];

        public override string Usage => Commands[0] + Translations["Chatrate_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.chatrate";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!float.TryParse(args[0], out float rate))
            {
                UnturnedChat.SendMessage(executor, Translations["Chatrate_Invalid"], ConsoleColor.Red);
                return;
            }
            if(rate < MIN_NUMBER)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Chatrate_TooLow"], MIN_NUMBER), ConsoleColor.Red);
                return;
            }
            else if(rate > MAX_NUMBER)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Chatrate_TooHigh"], MAX_NUMBER), ConsoleColor.Red);
                return;
            }

            ChatManager.chatrate = rate;
            UnturnedChat.SendMessage(executor, string.Format(Translations["Chatrate_SetTo"], rate), ConsoleColor.Green);
        }
    }
}
