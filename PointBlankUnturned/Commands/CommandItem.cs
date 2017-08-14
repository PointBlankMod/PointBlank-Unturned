﻿using System;
using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    public class CommandItem : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "i",
            "Item"
        };

        public override string Help => Translations["Item_Help"];

        public override string Usage => Commands[0] + Translations["Item_Usage"];

        public override string DefaultPermission => "pointblank.commands.admin.item";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            ItemAsset item;
            UnturnedPlayer[] players = new UnturnedPlayer[1];
            players[0] = (UnturnedPlayer)executor;

            if(!ushort.TryParse(args[0], out ushort id))
            {
                ItemAsset[] items = Assets.find(EAssetType.ITEM) as ItemAsset[];

                UnturnedChat.SendMessage(executor, "Test", ConsoleColor.Blue);
                item = items.Where(a => a != null).OrderBy(a => a.itemName.Length).FirstOrDefault(a => a.itemName.ToLower().Contains(args[0].ToLower()));
                UnturnedChat.SendMessage(executor, "Test1", ConsoleColor.Blue);
            }
            else
            {
                item = Assets.find(EAssetType.ITEM, id) as ItemAsset;
            }
            if (item == null)
            {
                UnturnedChat.SendMessage(executor, Translations["Item_Invalid"], ConsoleColor.Red);
                return;
            }

            if(args.Length < 2 || !byte.TryParse(args[1], out byte amount))
                amount = 1;

            if(args.Length > 2)
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

                if (!ItemTool.tryForceGiveItem(player.Player, item.id, amount))
                {
                    UnturnedChat.SendMessage(executor, Translations["Item_Fail"], ConsoleColor.Red);
                    return;
                }
                UnturnedChat.SendMessage(executor, string.Format(Translations["Item_Give"], item.itemName, player.PlayerName), ConsoleColor.Green);
            });
        }
    }
}
