using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandKick : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Kick"
        };

        public override string Help => Translations["Kick_Help"];

        public override string Usage => Commands[0] + Translations["Kick_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.kick";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            string reason;

            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer ply))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }
            reason = args.Length < 2 ? Translations["Kick_Reason"] : args[1];

            Provider.kick(ply.SteamID, reason);
            UnturnedChat.SendMessage(executor, string.Format(Translations["Kick_Kicked"], ply.PlayerName), ConsoleColor.Green);
        }
    }
}
