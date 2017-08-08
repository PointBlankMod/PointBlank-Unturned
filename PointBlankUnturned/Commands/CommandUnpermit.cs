using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    internal class CommandUnpermit : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Unpermit"
        };

        public override string Help => Translations["Unpermit_Help"];

        public override string Usage => Commands[0] + Translations["Unpermit_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.unpermit";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!PlayerTool.tryGetSteamID(args[0], out CSteamID id))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }

            if (!SteamWhitelist.unwhitelist(id))
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Unpermit_NotWhitelisted"], id), ConsoleColor.Red);
                return;
            }
            UnturnedChat.SendMessage(executor, string.Format(Translations["Unpermit_Unpermit"], id), ConsoleColor.Green);
        }
    }
}
