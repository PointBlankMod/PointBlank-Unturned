using System.Reflection;
using System.Linq;
using SDG.Unturned;
using UnityEngine;
using Steamworks;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Vehicle;

namespace PointBlank.Framework.Overrides
{
    internal class _VehicleManager
    {
        #region Reflection
        private static MethodInfo mi_tellVehicleDestroy = PointBlankReflect.GetMethod<VehicleManager>("tellVehicleDestroy", BindingFlags.Instance | BindingFlags.Public);
        #endregion

        [SteamCall]
        [Detour(typeof(VehicleManager), "tellVehicleDestroy", BindingFlags.Public | BindingFlags.Instance)]
        public void tellVehicleDestroy(CSteamID steamID, uint instanceID)
        {
            if (VehicleManager.instance.channel.checkServer(steamID))
            {
                InteractableVehicle vehicle = VehicleManager.vehicles.FirstOrDefault(a => a.instanceID == instanceID);

                if (vehicle == null)
                    return;

                ServerEvents.RunVehicleRemoved(UnturnedVehicle.Create(vehicle));
            }

            PointBlankDetourManager.CallOriginal(mi_tellVehicleDestroy, VehicleManager.instance, steamID, instanceID);
        }
        
        [SteamCall]
        [Detour(typeof(VehicleManager), "askEnterVehicle", BindingFlags.Public | BindingFlags.Instance)]
        public void askEnterVehicle(CSteamID steamID, uint instanceID, byte[] hash, byte engine)
        {
            if (Provider.isServer)
            {
                Player player = PlayerTool.getPlayer(steamID);
                if (player == null)
                    return;
                if (player.life.isDead)
                    return;
                if (player.equipment.isBusy)
                    return;
                if (player.equipment.isSelected && !player.equipment.isEquipped)
                    return;
                if (player.movement.getVehicle() != null)
                    return;
                InteractableVehicle interactableVehicle = null;
                for (int i = 0; i < VehicleManager.vehicles.Count; i++)
                {
                    if (VehicleManager.vehicles[i].instanceID == instanceID)
                    {
                        interactableVehicle = VehicleManager.vehicles[i];
                        break;
                    }
                }
                if (interactableVehicle == null)
                    return;
                if (interactableVehicle.asset.shouldVerifyHash && !Hash.verifyHash(hash, interactableVehicle.asset.hash))
                    return;
                if ((EEngine)engine != interactableVehicle.asset.engine)
                    return;
                if ((interactableVehicle.transform.position - player.transform.position).sqrMagnitude > 100f)
                    return;
                if (!interactableVehicle.checkEnter(player.channel.owner.playerID.steamID, player.quests.groupID))
                    return;
                byte b;
                bool cancel = false;

                VehicleEvents.RunVehicleEnter(UnturnedVehicle.Create(interactableVehicle), ref player, ref cancel);
                if (cancel)
                    return;
                if (interactableVehicle.tryAddPlayer(out b, player))
                {
                    VehicleManager.instance.channel.send("tellEnterVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        instanceID,
                        b,
                        steamID
                    });
                }
            }
        }
        
        [SteamCall]
        [Detour(typeof(VehicleManager), "askExitVehicle", BindingFlags.Public | BindingFlags.Instance)]
        public void askExitVehicle(CSteamID steamID, Vector3 velocity)
        {
            if (Provider.isServer)
            {
                Player player = PlayerTool.getPlayer(steamID);
                if (player == null)
                    return;
                if (player.life.isDead)
                    return;
                if (player.equipment.isBusy)
                    return;
                InteractableVehicle vehicle = player.movement.getVehicle();
                if (vehicle == null)
                    return;
                byte b;
                Vector3 vector;
                byte b2;
                bool cancel = false;

                VehicleEvents.RunVehicleExit(UnturnedVehicle.Create(vehicle), ref player, ref cancel);
                if (cancel)
                    return;
                if (vehicle.tryRemovePlayer(out b, steamID, out vector, out b2))
                {
                    VehicleManager.instance.channel.send("tellExitVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        vehicle.instanceID,
                        b,
                        vector,
                        b2,
                        false
                    });
                    if (b == 0 && Dedicator.isDedicated)
                        vehicle.GetComponent<Rigidbody>().velocity = velocity;
                }
            }
        }
    }
}