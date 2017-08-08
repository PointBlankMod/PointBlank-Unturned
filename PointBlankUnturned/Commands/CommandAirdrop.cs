using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    internal class CommandAirdrop : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Airdrop"
        };

        public override string Help => Translations["Airdrop_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.airdrop";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if (LevelManager.hasAirdrop)
                return;

            LevelManager.airdropFrequency = 0u;
            UnturnedChat.SendMessage(executor, Translations["Airdrop_Success"], ConsoleColor.Green);
        }
    }
}
