using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using UnityEngine;
using Steamworks;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandAdmin : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Admin"
        };

        public override string Help => Translations["Admin_Help"];

        public override string Usage => Commands[0] + Translations["Admin_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.admin";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            CSteamID steamID = CSteamID.Nil;

            if (!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer player))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }
            if(player == null)
            {
                if(!PlayerTool.tryGetSteamID(args[0], out steamID))
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }
            }
            else
            {
                steamID = player.SteamID;
            }

            if (UnturnedPlayer.IsServer(executor))
                SteamAdminlist.admin(steamID, CSteamID.Nil);
            else
                SteamAdminlist.admin(steamID, ((UnturnedPlayer)executor).SteamID);
            UnturnedChat.SendMessage(executor, string.Format(Translations["Admin_Set"], player), ConsoleColor.Green);
        }
    }
}
