using System.Reflection;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace PointBlank.Framework.Overrides
{
    internal static class _PlayerSkills
    {
        [SteamCall]
        [Detour(typeof(PlayerSkills), "tellSkill", BindingFlags.Instance | BindingFlags.Public)]
        public static void tellSkill(this PlayerSkills Skills, CSteamID steamID, byte speciality, byte index, byte level)
        {
            if (!Skills.channel.checkServer(steamID)) return;
            if (index >= Skills.skills[speciality].Length) return;

            UnturnedPlayer Player = UnturnedPlayer.Get(Skills.player);
            
            PlayerEvents.RunPlayerSkillUpgrade(Player, speciality, index, level);

            DetourManager.CallOriginal(typeof(PlayerSkills).GetMethod("tellSkill", BindingFlags.Instance | BindingFlags.Public),
                Skills, steamID, speciality, index, level);
        }
    }
}