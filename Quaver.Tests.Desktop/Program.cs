using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Quaver.Shared;
using Quaver.Shared.Config;
using Quaver.Shared.Online;
using Wobble;
using Wobble.Logging;

namespace Quaver.Tests.Desktop
{
    internal static class Program
    {
        /// <summary>
        ///     The current working directory of the executable.
        /// </summary>
        public static string WorkingDirectory => WobbleGame.WorkingDirectory;

        [STAThread]
        public static void Main()
        {
            // Log all unhandled exceptions.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var exception = args.ExceptionObject as Exception;
                Logger.Error(exception, LogType.Runtime);
            };

            // Change the working directory to where the executable is.
            Directory.SetCurrentDirectory(WorkingDirectory);
            Environment.CurrentDirectory = WorkingDirectory;

            try
            {
                using (var p = Process.GetCurrentProcess())
                    p.PriorityClass = ProcessPriorityClass.High;
            }
            catch (Win32Exception) { /* do nothing */ }

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            ConfigManager.Initialize();
            SteamManager.Initialize();

            using (var game = new QuaverTestsGame())
                game.Run();
        }
    }
}
