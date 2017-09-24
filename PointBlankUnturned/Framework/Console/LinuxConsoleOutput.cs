using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PointBlank.API;
using PointBlank.Framework.Overrides;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.Framework.Console
{
	internal static class LinuxConsoleOutput
	{
		public static ConsoleColorScheme ColorScheme;

		public static void Init()
		{
			if (Environment.GetCommandLineArgs().Any(s => s.Contains("-colorscheme=")))
			{
				switch (Environment.GetCommandLineArgs().First(c => c.Contains("-colorscheme=")).Split('=')[1])
				{
					case "ANSI":
						ColorScheme = ConsoleColorScheme.Ansi;
						break;
					case "XTERM":
						ColorScheme = ConsoleColorScheme.Xterm;
						break;
					case "MONO":
						ColorScheme = ConsoleColorScheme.Mono;
						break;
					default:
						ColorScheme = ConsoleColorScheme.Ansi;
						break;
				}
			}
			else
				ColorScheme = ConsoleColorScheme.Ansi;

			CommandWindow.onCommandWindowOutputted += OnLineWritten;
			Application.logMessageReceivedThreaded += OnLog;
		}

		public static void PostPbInit()
		{
			PointBlankConsoleEvents.OnConsoleLineWritten += OnLineWritten;
			WriteLine("Console PointBlank wrapper initialized.", ConsoleColor.Blue);
		}

		private static void OnLog(string text, string stack, LogType type)
		{
			ConsoleColor c = ConsoleColor.White;
			switch (type)
			{
				case LogType.Assert:
					c = ConsoleColor.Cyan;
					break;
				case LogType.Error:
					c = ConsoleColor.Red;
					break;
				case LogType.Exception:
					c = ConsoleColor.DarkRed;
					break;
				case LogType.Log:
					c = ConsoleColor.White;
					break;
				case LogType.Warning:
					c = ConsoleColor.Yellow;
					break;
			}
			WriteLine(text, c);
			if (!string.IsNullOrEmpty(stack))
				WriteLine(stack, c);
		}

		private static void OnLineWritten(object o, ConsoleColor c) =>
			WriteLine(o.ToString(), c);

		public static void WriteLine(string message, ConsoleColor color)
		{
			string outputFileDirectory = ServerInfo.ServerPath + "/Console/Console.STDOUT";

			using (StreamWriter wr = File.AppendText(outputFileDirectory))
			{
				wr.WriteLine(LinuxConsoleUtils.ConsoleColorToEscapeCode(color) + message);
			}
		}
	}
}
