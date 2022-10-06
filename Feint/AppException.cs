using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feint.Core
{
    public static class AppException
    {
        public static string FileName { get; private set; } = "UnhandledException.log";

        public static void SetFile(string fileName) => FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        public static void Install() => AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        public static void Uninstall() => AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Log(ex, "Unhandled");
            }
        }

        public static void Log(Exception ex, string header)
        {
            var aLines = new string[]
            {
                "================================================================",
                $"Header: {header}",
                $"Time: {DateTime.Now}",
                $"Message: {ex.Message}",
                "StackTrace: ",
                ex.StackTrace ?? "<null>",
                "================================================================",
                Environment.NewLine
            };

            try
            {
                File.AppendAllLines(FileName, aLines);
            }
            catch
            {
                // ...
            }
        }
    }
}
