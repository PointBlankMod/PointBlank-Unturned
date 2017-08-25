using System;
using System.Reflection;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.Framework.Overrides
{
    internal class _CommandWindow
    {
        [Detour(typeof(CommandWindow), "Log", BindingFlags.Public | BindingFlags.Static)]
        public static void Log(object text, ConsoleColor color)
        {
            bool cancel = false;

            ServerEvents.RunConsoleOutput(ref text, ref color, ref cancel);
            if (cancel)
                return;
            CommandWindow.onCommandWindowOutputted?.Invoke(text, color);
            if(CommandWindow.output == null)
            {
                Debug.Log(text);
                return;
            }
            Console.ForegroundColor = color;
            if (Console.CursorLeft != 0)
                CommandWindow.input.clearLine();
            Console.WriteLine(text);
            CommandWindow.input.redrawInputLine();
        }
    }
}
