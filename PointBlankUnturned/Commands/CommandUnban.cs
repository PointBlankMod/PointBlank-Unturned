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
    [PointBlankCommand("Unban", 1)]
    internal class CommandUnban : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Unban"
        };

        public override string Help => Translations["Unban_Help"];

        public override string Usage => Commands[0] + Translations["Unban_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.unban";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!PlayerTool.tryGetSteamID(args[0], out CSteamID id))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }

            if (!SteamBlacklist.unban(id))
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Unban_NotBanned"], id), ConsoleColor.Red);
                return;
            }
            UnturnedChat.SendMessage(executor, string.Format(Translations["Unban_Unban"], id), ConsoleColor.Green);
        }
    }
}
