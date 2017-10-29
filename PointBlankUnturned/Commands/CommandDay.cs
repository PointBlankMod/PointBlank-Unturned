using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandDay : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Day"
        };

        public override string Help => Translations["Day_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.day";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if (Provider.isServer && Level.info.type == ELevelType.ARENA)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NoArenaTime"], ConsoleColor.Red);
                return;
            }
            if (Provider.isServer && Level.info.type == ELevelType.HORDE)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NoHordeTime"], ConsoleColor.Red);
                return;
            }

            LightingManager.time = (uint)(LightingManager.cycle * LevelLighting.transition);
            UnturnedChat.SendMessage(executor, Translations["Day_Set"], ConsoleColor.Green);
        }
    }
}
