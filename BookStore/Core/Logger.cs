using System;
using System.IO;

namespace AutomationFramework.Core
{
    /// <summary>
    /// Singleton Logger - Ensures only one instance throughout the application
    /// OOPS: Encapsulation, Single Responsibility Principle
    /// </summary>
    public sealed class Logger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
        private readonly string _logFile;
        private readonly object _lock = new object();

        public static Logger Instance => _instance.Value;

        private Logger()
        {
            string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            _logFile = Path.Combine(logDir, $"log_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.txt");
        }

        /// <summary>
        /// Log information message
        /// </summary>
        public void Info(string message)
        {
            LogMessage("INFO", message);
        }

        /// <summary>
        /// Log error message
        /// </summary>
        public void Error(string message, Exception ex = null)
        {
            string errorMsg = ex != null ? $"{message} | Exception: {ex.Message}" : message;
            LogMessage("ERROR", errorMsg);
        }

        /// <summary>
        /// Log warning message
        /// </summary>
        public void Warning(string message)
        {
            LogMessage("WARNING", message);
        }

        private void LogMessage(string level, string message)
        {
            lock (_lock)
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string logEntry = $"[{timestamp}] [{level}] {message}";

                try
                {
                    File.AppendAllText(_logFile, logEntry + Environment.NewLine);
                    Console.WriteLine(logEntry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Logging error: {ex.Message}");
                }
            }
        }
    }
}