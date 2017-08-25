using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace PointBlank.Framework.Overrides
{
    internal static class _PlayerSkills
    {
        #region Reflection
        private static MethodInfo mi_tellSkill = PointBlankReflect.GetMethod<PlayerSkills>("tellSkill", BindingFlags.Instance | BindingFlags.Public);
        #endregion

        [SteamCall]
        [Detour(typeof(PlayerSkills), "tellSkill", BindingFlags.Instance | BindingFlags.Public)]
        public static void tellSkill(this PlayerSkills Skills, CSteamID steamID, byte speciality, byte index, byte level)
        {
            if (!Skills.channel.checkServer(steamID)) return;
            if (index >= Skills.skills[speciality].Length) return;

            UnturnedPlayer Player = UnturnedPlayer.Get(Skills.player);
            
            PlayerEvents.RunPlayerSkillUpgrade(Player, speciality, index, level);

            PointBlankDetourManager.CallOriginal(mi_tellSkill, Skills, steamID, speciality, index, level);
        }
    }
}