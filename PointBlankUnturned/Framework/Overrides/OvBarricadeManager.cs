using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Barricade;
using PointBlank.API.Unturned;
using SDG.Unturned;
using UnityEngine;
using Steamworks;

namespace PointBlank.Framework.Overrides
{
	internal class OvBarricadeManager
	{
        #region Reflection
        private static FieldInfo _fiInstanceCount = PointBlankReflect.GetField<SDG.Unturned.BarricadeManager>("instanceCount", BindingFlags.NonPublic | BindingFlags.Static);

        private static MethodInfo _miSpawnBarricade = PointBlankReflect.GetMethod<SDG.Unturned.BarricadeManager>("spawnBarricade", BindingFlags.NonPublic | BindingFlags.Instance);
        #endregion

        [Detour(typeof(SDG.Unturned.BarricadeManager), "dropBarricade", BindingFlags.Public | BindingFlags.Static)]
        public static Transform DropBarricade(SDG.Unturned.Barricade barricade, Transform hit, Vector3 point, float angleX, float angleY, float angleZ, ulong owner, ulong group)
        {
            ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, barricade.id);
            Transform result = null;

            if (itemBarricadeAsset != null)
            {
                Vector3 eulerAngles = SDG.Unturned.BarricadeManager.getRotation(itemBarricadeAsset, angleX, angleY, angleZ).eulerAngles;
                angleX = (float)(Mathf.RoundToInt(eulerAngles.x / 2f) * 2);
                angleY = (float)(Mathf.RoundToInt(eulerAngles.y / 2f) * 2);
                angleZ = (float)(Mathf.RoundToInt(eulerAngles.z / 2f) * 2);
                byte b3;
                byte b4;
                BarricadeRegion barricadeRegion2;

                if (hit != null && hit.transform.CompareTag("Vehicle"))
                {
                    byte b;
                    byte b2;
                    ushort num;
                    BarricadeRegion barricadeRegion;

                    if (SDG.Unturned.BarricadeManager.tryGetPlant(hit, out b, out b2, out num, out barricadeRegion))
                    {
                        BarricadeData barricadeData = new BarricadeData(barricade, point, MeasurementTool.angleToByte(angleX), MeasurementTool.angleToByte(angleY), MeasurementTool.angleToByte(angleZ), owner, group, SDG.Unturned.Provider.time);
                        bool cancel = false;

                        ServerEvents.RunBarricadeCreated(barricadeData, ref cancel);
                        if (cancel)
                            return null;
                        barricadeRegion.barricades.Add(barricadeData);

                        uint num2 = _fiInstanceCount.GetValue<uint>(null) + 1u;
                        _fiInstanceCount.SetValue(null, num2);
                        result = _miSpawnBarricade.RunMethod<Transform>(SDG.Unturned.BarricadeManager.instance, barricadeRegion, barricade.id, barricade.state, barricadeData.point, barricadeData.angle_x, barricadeData.angle_y, barricadeData.angle_z, (byte)100, barricadeData.owner, barricadeData.group, num2);

                        SDG.Unturned.BarricadeManager.instance.channel.send("tellBarricade", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                        {
                            b,
                            b2,
                            num,
                            barricade.id,
                            barricade.state,
                            barricadeData.point,
                            barricadeData.angle_x,
                            barricadeData.angle_y,
                            barricadeData.angle_z,
                            barricadeData.owner,
                            barricadeData.group,
                            num2
                        });
                    }
                }
                else if (Regions.tryGetCoordinate(point, out b3, out b4) && SDG.Unturned.BarricadeManager.tryGetRegion(b3, b4, 65535, out barricadeRegion2))
                {
                    BarricadeData barricadeData2 = new BarricadeData(barricade, point, MeasurementTool.angleToByte(angleX), MeasurementTool.angleToByte(angleY), MeasurementTool.angleToByte(angleZ), owner, group, SDG.Unturned.Provider.time);
                    bool cancel = false;

                    ServerEvents.RunBarricadeCreated(barricadeData2, ref cancel);
                    if (cancel)
                        return null;
                    barricadeRegion2.barricades.Add(barricadeData2);

                    uint num3 = _fiInstanceCount.GetValue<uint>(null) + 1u;
                    _fiInstanceCount.SetValue(null, num3);
                    result = _miSpawnBarricade.RunMethod<Transform>(SDG.Unturned.BarricadeManager.instance, barricadeRegion2, barricade.id, barricade.state, barricadeData2.point, barricadeData2.angle_x, barricadeData2.angle_y, barricadeData2.angle_z, (byte)100, barricadeData2.owner, barricadeData2.group, num3);

                    SDG.Unturned.BarricadeManager.instance.channel.send("tellBarricade", ESteamCall.OTHERS, b3, b4, SDG.Unturned.BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        b3,
                        b4,
                        (ushort)65535,
                        barricade.id,
                        barricade.state,
                        barricadeData2.point,
                        barricadeData2.angle_x,
                        barricadeData2.angle_y,
                        barricadeData2.angle_z,
                        barricadeData2.owner,
                        barricadeData2.group,
                        num3
                    });
                }
            }
            return result;
        }

        [SteamCall]
        [Detour(typeof(SDG.Unturned.BarricadeManager), "askSalvageBarricade", BindingFlags.Public | BindingFlags.Instance)]
        public void AskSalvageBarricade(CSteamID steamId, byte x, byte y, ushort plant, ushort index)
        {
            BarricadeRegion barricadeRegion;
            bool cancel = false;

            if (SDG.Unturned.Provider.isServer && SDG.Unturned.BarricadeManager.tryGetRegion(x, y, plant, out barricadeRegion))
            {
                Player player = PlayerTool.getPlayer(steamId);
                BarricadeEvents.RunBarricadeSalvage(UnturnedBarricade.Create(barricadeRegion.barricades[(int)index]), ref cancel);

                if (player == null)
                    return;
                if (player.life.isDead)
                    return;
                if ((int)index >= barricadeRegion.drops.Count)
                    return;
                if (!OwnershipTool.CheckToggle(player.channel.owner.playerID.steamID, barricadeRegion.barricades[(int)index].owner, player.quests.groupID, barricadeRegion.barricades[(int)index].group))
                    return;
                if (cancel)
                    return;
                ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, barricadeRegion.barricades[(int)index].barricade.id);

                if (itemBarricadeAsset != null)
                {
                    if (itemBarricadeAsset.isUnpickupable)
                        return;

                    if (barricadeRegion.barricades[(int)index].barricade.health == itemBarricadeAsset.health)
                    {
                        player.inventory.forceAddItem(new Item(barricadeRegion.barricades[(int)index].barricade.id, EItemOrigin.NATURE), true);
                    }
                    else if (itemBarricadeAsset.isSalvageable)
                    {
                        for (int i = 0; i < itemBarricadeAsset.blueprints.Count; i++)
                        {
                            Blueprint blueprint = itemBarricadeAsset.blueprints[i];
                            if (blueprint.outputs.Length == 1 && blueprint.outputs[0].id == itemBarricadeAsset.id)
                            {
                                ushort id = blueprint.supplies[UnityEngine.Random.Range(0, blueprint.supplies.Length)].id;

                                player.inventory.forceAddItem(new Item(id, EItemOrigin.NATURE), true);
                                break;
                            }
                        }
                    }
                }
                barricadeRegion.barricades.RemoveAt((int)index);
                if (plant == 65535)
                {
                    SDG.Unturned.BarricadeManager.instance.channel.send("tellTakeBarricade", ESteamCall.ALL, x, y, SDG.Unturned.BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        x,
                        y,
                        plant,
                        index
                    });
                }
                else
                {
                    SDG.Unturned.BarricadeManager.instance.channel.send("tellTakeBarricade", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        x,
                        y,
                        plant,
                        index
                    });
                }
            }
        }
    }
}
