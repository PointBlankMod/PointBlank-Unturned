using System;
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
    public class CommandVehicle : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = UnturnedEnvironment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "v",
            "Vehicle"
        };

        public override string Help => Translations["Vehicle_Help"];

        public override string Usage => Commands[0] + Translations["Vehicle_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.vehicle";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            VehicleAsset vehicle;
            UnturnedPlayer[] players = new UnturnedPlayer[1];
            players[0] = (UnturnedPlayer)executor;

            if(!ushort.TryParse(args[0], out ushort id))
            {
                VehicleAsset[] vehicles = Assets.find(EAssetType.VEHICLE) as VehicleAsset[];

                vehicle = vehicles.Where(a => a != null).OrderBy(a => a.vehicleName.Length).FirstOrDefault(a => a.vehicleName.ToLower().Contains(args[0].ToLower()));
            }
            else
                vehicle = Assets.find(EAssetType.VEHICLE, id) as VehicleAsset;
            if(vehicle == null)
            {
                UnturnedChat.SendMessage(executor, Translations["Vehicle_Invalid"], ConsoleColor.Red);
                return;
            }

            if (args.Length > 1)
            {
                if (!UnturnedPlayer.TryGetPlayers(args[1], out players))
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

                if (!VehicleTool.giveVehicle(player.Player, vehicle.id))
                {
                    UnturnedChat.SendMessage(executor, string.Format(Translations["Vehicle_Fail"], vehicle.vehicleName), ConsoleColor.Red);
                    return;
                }
                UnturnedChat.SendMessage(executor, string.Format(Translations["Vehicle_Spawn"], vehicle.vehicleName), ConsoleColor.Green);
            });
        }
    }
}
