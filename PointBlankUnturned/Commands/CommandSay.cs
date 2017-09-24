using PointBlank.API.Commands;
using SDG.Unturned;
using UnityEngine;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandSay : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = PointBlankUnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Say"
        };

        public override string Help => Translations["Say_Help"];

        public override string Usage => Commands[0] + Translations["Say_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.say";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(args.Length < 4 || !byte.TryParse(args[1], out byte r) || !byte.TryParse(args[2], out byte g) || !byte.TryParse(args[3], out byte b))
            {
                ChatManager.say(args[0], Palette.SERVER);
                return;
            }

            ChatManager.say(args[0], new Color((r / 255f), (g / 255f), (b / 255f)));
        }
    }
}
