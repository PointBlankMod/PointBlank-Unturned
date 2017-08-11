using System;
using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandTeleport : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "tp",
            "Teleport"
        };

        public override string Help => Translations["Teleport_Help"];

        public override string Usage => Commands[0] + Translations["Teleport_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.teleport";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)executor;

            if (args.Length > 1)
            {
                if (!UnturnedPlayer.TryGetPlayer(args[1], out player))
                {
                    UnturnedChat.SendMessage(executor, Translate("PlayerNotFound"), ConsoleColor.Red);
                    return;
                }
            }

            if (UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer pTarget))
            {
                player.Teleport(pTarget.Player.transform.position);
                UnturnedChat.SendMessage(executor, string.Format(Translate("Teleport_Teleport"), player.PlayerName, pTarget.PlayerName), ConsoleColor.Green);
            }
            else
            {
                Node nTarget = LevelNodes.nodes.FirstOrDefault(a => a.type == ENodeType.LOCATION && NameTool.checkNames(args[0], ((LocationNode)a).name));

                if (nTarget == null)
                {
                    UnturnedChat.SendMessage(executor, Translate("Teleport_Invalid"), ConsoleColor.Red);
                    return;
                }

                player.Teleport(nTarget.point);
                UnturnedChat.SendMessage(executor, string.Format(Translate("Teleport_Teleport"), player.PlayerName, ((LocationNode)nTarget).name), ConsoleColor.Green);
            }
        }
    }
}
