using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxx
{
    public static class Logger
    {
        private static readonly object lockObject = new object();
        private static readonly string logFilePath = "logFile.txt";

        public static void Log(string message)
        {
            lock (lockObject)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        // Append the log message with date and time to the file
                        writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions while writing to the log file
                    Console.WriteLine($"Error writing to log file: {ex.Message}");
                }
            }
        }
    }
}
