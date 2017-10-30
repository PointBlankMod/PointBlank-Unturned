using System;
using System.Linq;
using PointBlank.API.Groups;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using UnityEngine;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandGroup : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Group"
        };

        public override string Help => Translations["Group_Help"];

        public override string Usage => Commands[0] + string.Format(Translations["Group_Usage"], Translations["Group_Commands_Help"]);

        public override string DefaultPermission => "pointblank.commands.admin.group";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_List"], args[0]) == 0)
            {
                UnturnedChat.SendMessage(executor, string.Join(",", PointBlankGroupManager.Groups.Select(a => a.Name).ToArray()), ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_IDs"], args[0]) == 0)
            {
                UnturnedChat.SendMessage(executor, string.Join(",", PointBlankGroupManager.Groups.Select(a => a.ID).ToArray()), ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_Help"], args[0]) == 0)
            {
                UnturnedChat.SendMessage(executor, Commands[0] + " " + Translations["Group_Commands_Reload"], ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Group_Usage_Add"], Translations["Group_Commands_Add"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Group_Usage_IDs"], Translations["Group_Commands_IDs"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Group_Usage_List"], Translations["Group_Commands_List"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Group_Usage_Permissions"], Translations["Group_Commands_Permissions"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Group_Usage_Remove"], Translations["Group_Commands_Remove"]), ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_Permissions"], args[0]) == 0)
            {
                Permissions(executor, args);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_Add"], args[0]) == 0)
            {
                Add(executor, args);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_Remove"], args[0]) == 0)
            {
                Remove(executor, args);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_Reload"], args[0]) == 0)
            {
                PointBlankGroupManager.Reload();
                UnturnedChat.SendMessage(executor, Translations["Group_Reloaded"], ConsoleColor.Green);
            }
        }

        #region Functions
        private void Permissions(PointBlankPlayer executor, string[] args)
        {
            if(args.Length < 2)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            PointBlankGroup group = PointBlankGroup.Find(args[1]);
            if (group == null)
            {
                UnturnedChat.SendMessage(executor, Translations["Group_NotFound"], ConsoleColor.Red);
                return;
            }

            UnturnedChat.SendMessage(executor, string.Join(",", group.Permissions.Select(a => a.Permission).ToArray()), ConsoleColor.Green);
        }

        private void Add(PointBlankPlayer executor, string[] args)
        {
            if(args.Length < 3)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            if (PointBlankGroup.Exists(args[1]))
            {
                UnturnedChat.SendMessage(executor, Translations["Group_Exists"], ConsoleColor.Red);
                return;
            }

            PointBlankGroupManager.AddGroup(args[1], args[2], false, Color.clear);
            UnturnedChat.SendMessage(executor, Translations["Group_Added"], ConsoleColor.Green);
        }

        private void Remove(PointBlankPlayer executor, string[] args)
        {
            if(args.Length < 2)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            PointBlankGroup group = PointBlankGroup.Find(args[1]);
            if (group == null)
            {
                UnturnedChat.SendMessage(executor, Translations["Group_NotFound"], ConsoleColor.Red);
                return;
            }

            PointBlankGroupManager.RemoveGroup(group);
            UnturnedChat.SendMessage(executor, Translations["Group_Removed"], ConsoleColor.Green);
        }
        #endregion
    }
}
