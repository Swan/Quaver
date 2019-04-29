using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Quaver.Server.Common.Objects;
using Quaver.Shared.Screens;
using Wobble;

namespace Quaver.Tests.Desktop.Screens.Testing
{
    public sealed class TestingScreen : QuaverScreen
    {
        public override QuaverScreenType Type { get; } = QuaverScreenType.None;

        public override UserClientStatus GetClientStatus() => null;

        public bool Recompiling { get; set; } = false;

        public TestingScreen()
        {
            View = new TestingScreenView(this);

            var watcher = new FileSystemWatcher
            {
                Path = $@"../../../../Quaver.Shared/",
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.cs",
                EnableRaisingEvents = true,
                IncludeSubdirectories = true
            };

            watcher.Changed += OnChanged;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (Recompiling)
                return;

            Recompiling = true;

            var output = $"{WobbleGame.WorkingDirectory}/builds";
            Directory.CreateDirectory(output);
            Console.WriteLine(output);
            Console.WriteLine(RunCommand($"dotnet", $@"build ../../../../Quaver.Shared/Quaver.Shared.csproj -o {output}"));


            Recompiling = false;
        }

        /// <summary>
        ///     Runs a CLI command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool RunCommand(string command, string args)
        {
            var psi = new ProcessStartInfo(command, args)
            {
                WorkingDirectory = Environment.CurrentDirectory,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var p = Process.Start(psi);
            if (p == null) return false;

            var output = p.StandardOutput.ReadToEnd();
            output += p.StandardError.ReadToEnd();

            p.WaitForExit();

            if (p.ExitCode == 0) return true;

            Console.WriteLine(output);
            return false;
        }
    }
}