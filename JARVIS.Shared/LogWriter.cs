using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JARVIS.Shared
{
    public class LogWriter : TextWriter
    {
        public int CacheLimit = 25;
        public bool UseCache;


        List<string> CachedLines = new List<string>();
        Encoding CurrentEncoding = Encoding.UTF8;
        readonly TextWriter ConsoleOutput;
        readonly StreamWriter Writer;

        public LogWriter(string path, Encoding encoding, TextWriter standardOutput)
        {
            CurrentEncoding = encoding;
            ConsoleOutput = standardOutput;

            // Create our writer
            Writer = new StreamWriter(
                Path.Combine(path),
                false,
                encoding);

        }
        public override Encoding Encoding
        {
            get
            {
                return CurrentEncoding;
            }
        }

        public override void WriteLine(string value)
        {
            ConsoleOutput.WriteLine(value);

            if (UseCache)
            {
                CachedLines.Add(value);
            }
            else
            {

                Writer.WriteLine(value);
                Writer.Flush();
            }
        }

        public void WriteCache()
        {
            if (CachedLines.Count == 0) return;

#if DEBUG
            ConsoleOutput.WriteLine("[CACHE] Writing " + CachedLines.Count + " lines to LOG file.");
#endif
                         
            // Loop over cached items and send them to the stream to be written
            foreach(string s in CachedLines)
            {
                Writer.WriteLine(s);
            }

            // Flush the writer to make sure they actually were outputted
            Writer.Flush();

            // Clear our cached lines
            CachedLines.Clear();
        }
    }
}
