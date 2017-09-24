using System;
using System.IO;
using PointBlank.API;
using SDG.Unturned;

namespace PointBlank.Framework.Console
{
	internal static class LinuxConsoleInput
	{
		private static TextReader _inputReader;

		public static void Init()
		{
			string inputFileDirectory = ServerInfo.ServerPath + "/Console/Console.STDIN";
			File.WriteAllText(inputFileDirectory, LinuxConsoleUtils.ConsoleColorToEscapeCode(ConsoleColor.Magenta));
			FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(ServerInfo.ServerPath + "/Console", "Console.STDIN");
			fileSystemWatcher.Changed += OnInput;
			_inputReader = new StreamReader(new FileStream(inputFileDirectory, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite));
			fileSystemWatcher.EnableRaisingEvents = true;
		}

		private static void OnInput(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType != WatcherChangeTypes.Changed)
				return;
			string newline = _inputReader.ReadToEnd();
			if (!string.IsNullOrEmpty(newline))
				LinuxConsoleOutput.WriteLine("> " + newline, ConsoleColor.Magenta);

			CommandWindow.input.onInputText(newline);
		}
	}
}
