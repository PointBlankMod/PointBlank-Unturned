using System.Reflection;
using PointBlank.API;
using PointBlank.API.Implements;
using PointBlank.API.Unturned;
using PointBlank.API.Unturned.Structure;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Detour;
using Steamworks;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.Framework.Overrides
{
    internal class _StructureManager
    {
        #region Reflection
        private static FieldInfo fi_instanceCount = PointBlankReflect.GetField<StructureManager>("instanceCount", BindingFlags.NonPublic | BindingFlags.Static);
        #endregion

        [Detour(typeof(StructureManager), "dropStructure", BindingFlags.Public | BindingFlags.Static)]
        public static void dropStructure(Structure structure, Vector3 point, float angle_x, float angle_y, float angle_z, ulong owner, ulong group)
        {
            ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, structure.id);

            if (itemStructureAsset != null)
            {
                Vector3 eulerAngles = Quaternion.Euler(-90f, angle_y, 0f).eulerAngles;
                angle_x = (float)(Mathf.RoundToInt(eulerAngles.x / 2f) * 2);
                angle_y = (float)(Mathf.RoundToInt(eulerAngles.y / 2f) * 2);
                angle_z = (float)(Mathf.RoundToInt(eulerAngles.z / 2f) * 2);
                byte b;
                byte b2;
                StructureRegion structureRegion;
                bool cancel = false;

                if (Regions.tryGetCoordinate(point, out b, out b2) && StructureManager.tryGetRegion(b, b2, out structureRegion))
                {
                    StructureData structureData = new StructureData(structure, point, MeasurementTool.angleToByte(angle_x), MeasurementTool.angleToByte(angle_y), MeasurementTool.angleToByte(angle_z), owner, group, Provider.time);
                    ServerEvents.RunStructureCreated(structureData, ref cancel);

                    if (cancel)
                        return;
                    structureRegion.structures.Add(structureData);
                    StructureManager.instance.channel.send("tellStructure", ESteamCall.ALL, b, b2, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        b,
                        b2,
                        structure.id,
                        structureData.point,
                        structureData.angle_x,
                        structureData.angle_y,
                        structureData.angle_z,
                        owner,
                        group,
                        fi_instanceCount.Get<uint>()
                    });
                    fi_instanceCount.SetValue(null, fi_instanceCount.Get<uint>() + 1u);
                }
            }
        }

        [SteamCall]
        [Detour(typeof(StructureManager), "askSalvageStructure", BindingFlags.Public | BindingFlags.Instance)]
        public void askSalvageStructure(CSteamID steamID, byte x, byte y, ushort index)
        {
            StructureRegion structureRegion;
            bool cancel = false;

            if (Provider.isServer && StructureManager.tryGetRegion(x, y, out structureRegion))
            {
                Player player = PlayerTool.getPlayer(steamID);
                StructureEvents.RunSalvageStructure(UnturnedStructure.Create(structureRegion.structures[(int)index]), ref cancel);

                if (player == null)
                    return;
                if (player.life.isDead)
                    return;
                if ((int)index >= structureRegion.drops.Count)
                    return;
                if (!OwnershipTool.checkToggle(player.channel.owner.playerID.steamID, structureRegion.structures[(int)index].owner, player.quests.groupID, structureRegion.structures[(int)index].group))
                    return;
                if (cancel)
                    return;
                ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, structureRegion.structures[(int)index].structure.id);

                if (itemStructureAsset != null)
                {
                    if (itemStructureAsset.isUnpickupable)
                        return;

                    if (structureRegion.structures[(int)index].structure.health == itemStructureAsset.health)
                    {
                        player.inventory.forceAddItem(new Item(structureRegion.structures[(int)index].structure.id, EItemOrigin.NATURE), true);
                    }
                    else if (itemStructureAsset.isSalvageable)
                    {
                        for (int i = 0; i < itemStructureAsset.blueprints.Count; i++)
                        {
                            Blueprint blueprint = itemStructureAsset.blueprints[i];
                            if (blueprint.outputs.Length == 1 && blueprint.outputs[0].id == itemStructureAsset.id)
                            {
                                ushort id = blueprint.supplies[UnityEngine.Random.Range(0, blueprint.supplies.Length)].id;
                                player.inventory.forceAddItem(new Item(id, EItemOrigin.NATURE), true);
                                break;
                            }
                        }
                    }
                }
                structureRegion.structures.RemoveAt((int)index);
                StructureManager.instance.channel.send("tellTakeStructure", ESteamCall.ALL, x, y, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    x,
                    y,
                    index,
                    (Vector3)((structureRegion.drops[(int)index].model.position - player.transform.position).normalized * 100f)
                });
            }
        }
    }
}
