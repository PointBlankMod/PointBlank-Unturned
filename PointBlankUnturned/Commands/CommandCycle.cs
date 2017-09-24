using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandCycle : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = PointBlankUnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Cycle"
        };

        public override string Help => Translations["Cycle_Help"];

        public override string Usage => Commands[0] + Translations["Cycle_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.cycle";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(Provider.isServer && Level.info.type == ELevelType.ARENA)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NoArenaTime"], ConsoleColor.Red);
                return;
            }
            if (Provider.isServer && Level.info.type == ELevelType.HORDE)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NoHordeTime"], ConsoleColor.Red);
                return;
            }
            if (!uint.TryParse(args[0], out uint cycle))
            {
                UnturnedChat.SendMessage(executor, Translations["Cycle_Invalid"], ConsoleColor.Red);
                return;
            }

            LightingManager.cycle = cycle;
            UnturnedChat.SendMessage(executor, string.Format(Translations["Cycle_SetTo"], cycle), ConsoleColor.Green);
        }
    }
}
