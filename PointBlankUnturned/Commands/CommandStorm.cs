using System;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Storm", 0)]
    internal class CommandStorm : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Storm"
        };

        public override string Help => Translations["Storm_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.storm";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            switch (LevelLighting.rainyness)
            {
                case ELightingRain.NONE:
                    LightingManager.rainFrequency = 0u;
                    break;
                case ELightingRain.DRIZZLE:
                    LightingManager.rainDuration = 0u;
                    break;
            }
            UnturnedChat.SendMessage(executor, Translations["Storm_Change"], ConsoleColor.Green);
        }
    }
}
