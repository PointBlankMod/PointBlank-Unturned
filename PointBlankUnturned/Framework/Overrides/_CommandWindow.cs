using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.Framework.Overrides
{
    internal class _CommandWindow
    {
        private static FieldInfo fiOutput = typeof(CommandWindow).GetField("output", BindingFlags.NonPublic | BindingFlags.Static);
        private static FieldInfo fiInput = typeof(CommandWindow).GetField("input", BindingFlags.NonPublic | BindingFlags.Static);

        [Detour(typeof(CommandWindow), "Log", BindingFlags.NonPublic | BindingFlags.Static)]
        public static void Log(object text, ConsoleColor color)
        {
            ConsoleOutput output = (ConsoleOutput)fiOutput.GetValue(null);
            ConsoleInput input = (ConsoleInput)fiInput.GetValue(null);
            bool cancel = false;

            ServerEvents.RunConsoleOutput(ref text, ref color, ref cancel);
            if (cancel)
                return;
            if(output == null)
            {
                Debug.Log(text);
                return;
            }
            Console.ForegroundColor = color;
            if (Console.CursorLeft != 0)
                input.clearLine();
            Console.WriteLine(text);
            input.redrawInputLine();
        }
    }
}
