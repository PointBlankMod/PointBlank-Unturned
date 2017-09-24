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
    public class CommandUnpermit : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = PointBlankUnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

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
            CSteamID steamId = CSteamID.Nil;
            
            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer player))
            {
                if (!PlayerTool.tryGetSteamID(args[0], out steamId))
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }
            }
            else
            {
                steamId = player.SteamId;
            }

            if (!SteamWhitelist.unwhitelist(steamId))
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Unpermit_NotWhitelisted"], steamId), ConsoleColor.Red);
                return;
            }
            UnturnedChat.SendMessage(executor, string.Format(Translations["Unpermit_Unpermit"], steamId), ConsoleColor.Green);
        }
    }
}
