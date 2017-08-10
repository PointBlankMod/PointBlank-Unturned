using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using Steamworks;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandBan : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Ban"
        };

        public override string Help => Translations["Ban_Help"];

        public override string Usage => Commands[0] + Translations["Ban_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.ban";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            uint ip;
            CSteamID steamID = CSteamID.Nil;

            if(UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer player))
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
            ip = SteamGameServerNetworking.GetP2PSessionState(steamID, out P2PSessionState_t p2PSessionState_t) ? p2PSessionState_t.m_nRemoteIP : 0u;

            if (args.Length < 2 || uint.TryParse(args[1], out uint duration))
                duration = SteamBlacklist.PERMANENT;
            string reason = args.Length < 3 ? Translations["Ban_Reason"] : args[2];

            SteamBlacklist.ban(steamID, ip, ((UnturnedPlayer)executor)?.SteamID ?? CSteamID.Nil, reason, duration);
            UnturnedChat.SendMessage(executor, string.Format(Translations["Ban_Success"], player), ConsoleColor.Green);
        }
    }
}
