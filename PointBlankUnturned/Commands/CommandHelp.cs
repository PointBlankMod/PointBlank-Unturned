using System;
using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;
using SDG.Unturned;

namespace PointBlank.Commands
{
    public class CommandHelp : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Help"
        };

        public override string Help => Translations["Help_Help"];

        public override string Usage => Commands[0] + Translations["Help_Usage"];

        public override string DefaultPermission => "unturned.commands.nonadmin.help";
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            PointBlankCommand[] commands = PointBlankCommandManager.Commands.Where(a => a.Enabled).ToArray();

            if (args.Length > 0)
            {
                PointBlankCommand cmd = commands.FirstOrDefault(a => a.Commands.FirstOrDefault(b => b.ToLower() == args[0].ToLower()) != null && a.Enabled);

                if(cmd == null)
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_CommandInvalid"], ConsoleColor.Red);
                    return;
                }
                UnturnedChat.SendMessage(executor, cmd.Help, ConsoleColor.Green);
                return;
            }
            string send = "";
            int pos = 0;
            while (true)
            {
                if(pos >= commands.Length)
                {
                    UnturnedChat.SendMessage(executor, send, ConsoleColor.Green);
                    break;
                }
                string command = commands[pos].Commands[0];
                if ((send.Length + ("," + command).Length) > ChatManager.LENGTH)
                {
                    UnturnedChat.SendMessage(executor, send, ConsoleColor.Green);
                    send = "";
                }

                send += (string.IsNullOrEmpty(send) ? "" : ",") + commands[pos].Commands[0].ToLower();
                pos++;
            }
        }
    }
}
