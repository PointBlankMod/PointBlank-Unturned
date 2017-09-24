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
	internal class OvStructureManager
	{
        #region Reflection
        private static FieldInfo _fiInstanceCount = PointBlankReflect.GetField<SDG.Unturned.StructureManager>("instanceCount", BindingFlags.NonPublic | BindingFlags.Static);
        #endregion

        [Detour(typeof(SDG.Unturned.StructureManager), "dropStructure", BindingFlags.Public | BindingFlags.Static)]
        public static void DropStructure(SDG.Unturned.Structure structure, Vector3 point, float angleX, float angleY, float angleZ, ulong owner, ulong group)
        {
            ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, structure.id);

            if (itemStructureAsset != null)
            {
                Vector3 eulerAngles = Quaternion.Euler(-90f, angleY, 0f).eulerAngles;
                angleX = (float)(Mathf.RoundToInt(eulerAngles.x / 2f) * 2);
                angleY = (float)(Mathf.RoundToInt(eulerAngles.y / 2f) * 2);
                angleZ = (float)(Mathf.RoundToInt(eulerAngles.z / 2f) * 2);
                byte b;
                byte b2;
                StructureRegion structureRegion;
                bool cancel = false;

                if (Regions.tryGetCoordinate(point, out b, out b2) && SDG.Unturned.StructureManager.tryGetRegion(b, b2, out structureRegion))
                {
                    StructureData structureData = new StructureData(structure, point, MeasurementTool.angleToByte(angleX), MeasurementTool.angleToByte(angleY), MeasurementTool.angleToByte(angleZ), owner, group, SDG.Unturned.Provider.time);
                    ServerEvents.RunStructureCreated(structureData, ref cancel);

                    if (cancel)
                        return;
                    structureRegion.structures.Add(structureData);
                    SDG.Unturned.StructureManager.instance.channel.send("tellStructure", ESteamCall.ALL, b, b2, SDG.Unturned.StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
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
                        _fiInstanceCount.Get<uint>()
                    });
                    _fiInstanceCount.SetValue(null, _fiInstanceCount.Get<uint>() + 1u);
                }
            }
        }

        [SteamCall]
        [Detour(typeof(SDG.Unturned.StructureManager), "askSalvageStructure", BindingFlags.Public | BindingFlags.Instance)]
        public void AskSalvageStructure(CSteamID steamId, byte x, byte y, ushort index)
        {
            StructureRegion structureRegion;
            bool cancel = false;

            if (SDG.Unturned.Provider.isServer && SDG.Unturned.StructureManager.tryGetRegion(x, y, out structureRegion))
            {
                Player player = PlayerTool.getPlayer(steamId);
                StructureEvents.RunSalvageStructure(UnturnedStructure.Create(structureRegion.structures[(int)index]), ref cancel);

                if (player == null)
                    return;
                if (player.life.isDead)
                    return;
                if ((int)index >= structureRegion.drops.Count)
                    return;
                if (!OwnershipTool.CheckToggle(player.channel.owner.playerID.steamID, structureRegion.structures[(int)index].owner, player.quests.groupID, structureRegion.structures[(int)index].group))
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
                SDG.Unturned.StructureManager.instance.channel.send("tellTakeStructure", ESteamCall.ALL, x, y, SDG.Unturned.StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
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
