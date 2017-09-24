using System;
using System.Reflection;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using PointBlank.Framework.Console;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.Framework.Overrides
{
	internal class OvCommandWindow
	{
        [Detour(typeof(CommandWindow), "Log", BindingFlags.Public | BindingFlags.Static)]
        public static void Log(object text, ConsoleColor color)
        {
			Debug.Log("Faggit");
            bool cancel = false;

            ServerEvents.RunConsoleOutput(ref text, ref color, ref cancel);

            if (cancel)
                return;

	        if (LinuxConsoleUtils.IsLinux)
		        CommandWindow.onCommandWindowOutputted?.Invoke(text, color);
	        else
		        CommandWindow.Log(text, color);
        }
    }
}
