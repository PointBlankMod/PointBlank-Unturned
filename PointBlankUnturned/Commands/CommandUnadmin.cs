using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandUnadmin : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Unadmin"
        };

        public override string Help => Translations["Unadmin_Help"];

        public override string Usage => Commands[0] + Translations["Unadmin_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.unadmin";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            CSteamID steamID = CSteamID.Nil;

            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer player))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }
            if(player == null)
            {
                if (!PlayerTool.tryGetSteamID(args[0], out steamID))
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }
            }
            else
            {
                steamID = player.SteamID;
            }

            SteamAdminlist.unadmin(steamID);
            UnturnedChat.SendMessage(executor, string.Format(Translations["Unadmin_Unadmin"], steamID), ConsoleColor.Green);
        }
    }
}
