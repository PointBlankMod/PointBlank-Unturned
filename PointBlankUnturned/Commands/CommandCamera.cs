using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandCamera : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Camera"
        };

        public override string Help => Translations["Camera_Help"];

        public override string Usage => Commands[0] + Translations["Camera_Usage"];

        public override string DefaultPermission => "unturned.commands.server.camera";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            string pos = args[0].ToUpperInvariant();
            ECameraMode mode;

            switch (pos)
            {
                case "FIRST":
                    mode = ECameraMode.FIRST;
                    break;
                case "THIRD":
                    mode = ECameraMode.THIRD;
                    break;
                case "VEHICLE":
                    mode = ECameraMode.VEHICLE;
                    break;
                case "BOTH":
                    mode = ECameraMode.BOTH;
                    break;
                default:
                    mode = ECameraMode.BOTH;
                    break;
            }
            Provider.cameraMode = mode;
            UnturnedChat.SendMessage(executor, string.Format(Translations["Camera_SetTo"], mode.ToString()), ConsoleColor.Green);
        }
    }
}
