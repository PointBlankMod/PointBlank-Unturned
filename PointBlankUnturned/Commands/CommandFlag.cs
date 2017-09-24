using System;
using PointBlank.API.Commands;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandFlag : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = PointBlankUnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Flag"
        };

        public override string Help => Translations["Flag_Help"];

        public override string Usage => Commands[0] + Translations["Flag_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.flag";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 2;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedPlayer[] players = new UnturnedPlayer[1];
            players[0] = (UnturnedPlayer)executor;

            if (!ushort.TryParse(args[0], out ushort flag))
            {
                UnturnedChat.SendMessage(executor, Translations["Flag_InvalidFlag"], ConsoleColor.Red);
                return;
            }
            if (!short.TryParse(args[1], out short value))
            {
                UnturnedChat.SendMessage(executor, Translations["Flag_InvalidValue"], ConsoleColor.Red);
                return;
            }
            if (args.Length > 2)
            {
                if (!UnturnedPlayer.TryGetPlayers(args[2], out players))
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }
            }

            players.ForEach((player) =>
            {
                if (UnturnedPlayer.IsServer(player))
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }

                player.Player.quests.sendSetFlag(flag, value);
                UnturnedChat.SendMessage(executor, string.Format(Translations["Flag_Set"], player.PlayerName), ConsoleColor.Green);
            });
        }
    }
}
