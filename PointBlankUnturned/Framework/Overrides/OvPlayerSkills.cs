using System.Reflection;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace PointBlank.Framework.Overrides
{
	internal static class OvPlayerSkills
	{
        #region Reflection
        private static MethodInfo _miTellSkill = PointBlankReflect.GetMethod<SDG.Unturned.PlayerSkills>("tellSkill", BindingFlags.Instance | BindingFlags.Public);
        #endregion

        [SteamCall]
        [Detour(typeof(SDG.Unturned.PlayerSkills), "tellSkill", BindingFlags.Instance | BindingFlags.Public)]
        public static void TellSkill(this SDG.Unturned.PlayerSkills skills, CSteamID steamId, byte speciality, byte index, byte level)
        {
            if (!skills.channel.checkServer(steamId)) return;
            if (index >= skills.skills[speciality].Length) return;

            UnturnedPlayer player = UnturnedPlayer.Get(skills.player);
            
            PlayerEvents.RunPlayerSkillUpgrade(player, speciality, index, level);

            PointBlankDetourManager.CallOriginal(_miTellSkill, skills, steamId, speciality, index, level);
        }
    }
}