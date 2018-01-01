using System;
using System.Threading;
using System.IO;

namespace JARVIS.Shared
{
    /// <summary>
    /// A Unified Logging System
    /// </summary>
    public static class Log 
    {
        /// <summary>
        /// The log file base name.
        /// </summary>
        static string _outputFileName = "JARVIS.log";

        /// <summary>
        /// A timer which triggers periodic writing of the log file.
        /// </summary>
        static Timer _periodicWriter;

        /// <summary>
        /// A cachable writer used by the <see cref="T:JARVIS.Shared.Log"/> class.
        /// </summary>
        static Services.LogWriter _writer;

        /// <summary>
        /// The last log events date.
        /// </summary>
        public static DateTime LastTimestamp;

        /// <summary>
        /// An attached notification system that will be pinged when events happen.
        /// </summary>
        public static Services.Notification.INotifier Notifier;

        /// <summary>
        /// Updates the last event timestamp, and provides a custom format response.
        /// </summary>
        /// <returns>The current time stamp.</returns>
        static string GetCurrentTimeStamp()
        {
            // Set the last timestamp
            LastTimestamp = DateTime.Now;

            // Handle Hour
            if (LastTimestamp.Hour >= 12)
            {
                return LastTimestamp.Month.ToString().PadLeft(2, '0') + "-" +
                       LastTimestamp.Day.ToString().PadLeft(2, '0') + "-" +
                       LastTimestamp.Year + " " +
                        (LastTimestamp.Hour - 12).ToString().PadLeft(2, '0') + ":" +
                       LastTimestamp.Minute.ToString().PadLeft(2, '0') + ":" +
                       LastTimestamp.Second.ToString().PadLeft(2, '0') + " PM";
            }

            return LastTimestamp.Month.ToString().PadLeft(2, '0') + "-" +
                   LastTimestamp.Day.ToString().PadLeft(2, '0') + "-" +
                   LastTimestamp.Year + " " +
                   LastTimestamp.Hour.ToString().PadLeft(2, '0') + ":" +
                   LastTimestamp.Minute.ToString().PadLeft(2, '0') + ":" +
                   LastTimestamp.Second.ToString().PadLeft(2, '0') + " AM";

        }

        /// <summary>
        /// Output an error message to the log.
        /// </summary>
        /// <param name="section">The section key to associate the error message with.</param>
        /// <param name="content">The content of the error message.</param>
        /// <param name="notify">Should a notification about this error message be sent to the attached notification system?</param>
        public static void Error(string section, string content, bool notify = true)
        {
            Console.WriteLine(GetCurrentTimeStamp() + "\t" + section.ToUpper() + "\t" + content);

            if (notify && Notifier != null)
            {
                Notifier.Notify(section.ToUpper() + " (ERROR)", content);
            }
        }

        /// <summary>
        /// Output a fatal message to the log.
        /// </summary>
        /// <param name="section">The section key to associate the fatal message with.</param>
        /// <param name="content">The content of the fatal message.</param>
        /// <param name="notify">Should a notification about this fatal message be sent to the attached notification system?</param>
        public static void Fatal(string section, string content, bool notify = true)
        {
            Console.WriteLine(GetCurrentTimeStamp() + "\t" + section.ToUpper() + "\t" + content);

            if (notify && Notifier != null)
            {
                Notifier.Notify(section.ToUpper() + " (FATAL)", content);
            }

            // Output the cache immediately.
            WriteCache();

            // Fatal events must immediately shutdown the program.
            Environment.Exit(1);
        }

        /// <summary>
        /// Output a messsage to the log.
        /// </summary>
        /// <param name="section">The section key to associate the message with.</param>
        /// <param name="content">The content of the message.</param>
        /// <param name="notify">Should a notification about this message be sent to the attached notification system?</param>
        public static void Message(string section, string content, bool notify = false)
        {
            Console.WriteLine(GetCurrentTimeStamp() + "\t" + section.ToUpper() + "\t" + content);

            if (notify && Notifier != null)
            {
                Notifier.Notify(section.ToUpper(), content);
            }
        }

        /// <summary>
        /// Start capturing of the standard output channel.
        /// </summary>
        /// <param name="cache">If set to <c>true</c> output will be cached, and periodically written.</param>
        public static void StartCapture(bool cache = true)
        {
            // Determine if we need to move old log files out of the way
            string logPath = Path.Combine(Platform.GetBaseDirectory(), _outputFileName);

            if (File.Exists(logPath))
            {
                // Get count of log files
                int count = Directory.GetFiles(Platform.GetBaseDirectory(), _outputFileName + "*").Length;

                // Move the latest to its new number
                File.Move(logPath, logPath + "." + count.ToString().PadLeft(3, '0'));
            }

            _writer = new Services.LogWriter(logPath, Console.OutputEncoding, Console.Out)
            {
                UseCache = cache
            };
            Console.SetOut(_writer);

            // Setup safety thread
            _periodicWriter = new Timer(
                e => WriteCache(),
                null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Stop capturing the standard output channel.
        /// </summary>
        public static void StopCapture()
        {
            // Do we have period writing
            if (_periodicWriter != null)
            {
                _periodicWriter.Dispose();
                _periodicWriter = null;
            }

            // Do we have a writer?
            if ( _writer != null ) {
                WriteCache();
                _writer.UseCache = false;
                _writer = null;
            }
        }

        /// <summary>
        /// Ouput the cached logging lines.
        /// </summary>
        public static void WriteCache()
        {
            _writer.WriteCache();
        }
       
    }
}
