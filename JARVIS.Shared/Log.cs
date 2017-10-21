using System;
namespace JARVIS.Shared
{
    public static class Log
    {
        public static DateTime Date;
        public static void Message(string section, string content)
        {
           
            Console.WriteLine(GetCurrentTimeStamp() + "\t" + section.ToUpper() + "\t" + content);
        }

        public static void Error(string section, string content)
        {
            Console.WriteLine(GetCurrentTimeStamp() + "\t" + section.ToUpper() + "\t" + content);
        }
        public static void Fatal(string section, string content)
        {
            Console.WriteLine(GetCurrentTimeStamp() + "\t" + section.ToUpper() + "\t" + content);
            Environment.Exit(1);
        }

        private static string GetCurrentTimeStamp() 
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
            } else {
                return Date.Month.ToString().PadLeft(2, '0') + "-" +
                       Date.Day.ToString().PadLeft(2, '0') + "-" +
                       Date.Year + " " +
                       Date.Hour.ToString().PadLeft(2, '0') + ":" +
                       Date.Minute.ToString().PadLeft(2, '0') + ":" +
                       Date.Second.ToString().PadLeft(2, '0') + " AM";
            }


        }
    }
}
