using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace JARVIS.Shared
{
    public static class Log 
    {
        public static DateTime Date;
        static string OutputFileNameBase = "JARVIS.log";
        static LogWriter Output;
        static Timer PeriodicWriter;
        public static INotifier Notifier;

        public static void WriteCache()
        {
            Output.WriteCache();
        }
        public static void Capture(bool cache = true)
        {
            // Determine if we need to move old log files out of the way
            string logPath = Path.Combine(Platform.GetBaseDirectory(), OutputFileNameBase);

            if (File.Exists(logPath))
            {
                // Get count of log files
                int count = Directory.GetFiles(Platform.GetBaseDirectory(), "JARVIS.log*").Length;

                // Move the latest to its new number
                File.Move(logPath, logPath + "." + count.ToString().PadLeft(3, '0'));
            }

            Output = new LogWriter(
                logPath,
                Console.OutputEncoding,
                Console.Out);

            Output.UseCache = cache;

            Console.SetOut(Output);

            // Setup safety thread
            PeriodicWriter = new Timer(
                e => WriteCache(),
                null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));
        }

        public static void Message(string section, string content, bool notify = false)
        {
            Console.WriteLine(GetCurrentTimeStamp() + "\t" + section.ToUpper() + "\t" + content);

            if ( notify && Notifier != null) {
                Notifier.Notify(section.ToUpper(), content);
            }
        }

        public static void Error(string section, string content, bool notify = false)
        {
            Console.WriteLine(GetCurrentTimeStamp() + "\t" + section.ToUpper() + "\t" + content);
           
            if (notify && Notifier != null)
            {
                Notifier.Notify(section.ToUpper() + " (ERROR)", content);
            }
        }
        public static void Fatal(string section, string content, bool notify = false)
        {
            Console.WriteLine(GetCurrentTimeStamp() + "\t" + section.ToUpper() + "\t" + content);

            if (notify && Notifier != null)
            {
                Notifier.Notify(section.ToUpper() + " (FATAL)", content);
            }
            Environment.Exit(1);
        }

        static string GetCurrentTimeStamp() 
        {
            // Set our date to the current time
            Log.Date = DateTime.Now;

            // Handle Hour
            if ( Date.Hour >= 12 ) {
                return Date.Month.ToString().PadLeft(2, '0') + "-" +
                       Date.Day.ToString().PadLeft(2, '0') + "-" +
                       Date.Year + " " +
                        (Date.Hour - 12).ToString().PadLeft(2, '0') + ":" +
                       Date.Minute.ToString().PadLeft(2, '0') + ":" +
                       Date.Second.ToString().PadLeft(2, '0') + " PM";    
            }

            return Date.Month.ToString().PadLeft(2, '0') + "-" +
                   Date.Day.ToString().PadLeft(2, '0') + "-" +
                   Date.Year + " " +
                   Date.Hour.ToString().PadLeft(2, '0') + ":" +
                   Date.Minute.ToString().PadLeft(2, '0') + ":" +
                   Date.Second.ToString().PadLeft(2, '0') + " AM";
            
        }
    }
}
