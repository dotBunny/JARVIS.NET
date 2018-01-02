using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace JARVIS.Shared.Services
{
    /// <summary>
    /// Unified Log Writer
    /// </summary>
    public class LogWriter : TextWriter
    {
        /// <summary>
        /// A cache of lines that need to be written to the log file.
        /// </summary>
        readonly ConcurrentQueue<string> _cachedLines = new ConcurrentQueue<string>();

        /// <summary>
        /// The standard output channel.
        /// </summary>
        readonly TextWriter _consoleOutput;

        /// <summary>
        /// The log file writing stream.
        /// </summary>
        readonly StreamWriter _writer;

        /// <summary>
        /// The current encoding setting of the log file.
        /// </summary>
        Encoding _encoding = Encoding.UTF8;

        /// <summary>
        /// The maximum number of lines allowed to reside in the cache before a forced output occurs.
        /// </summary>
        public int CacheLimit = 25;

        /// <summary>
        /// Should the log be cached?
        /// </summary>
        /// <remarks>
        /// Improves performance, if there was ever a need.
        /// </remarks>
        public bool UseCache;

        /// <summary>
        /// Gets the currently defined <see cref="T:System.Text.Encoding" />.
        /// </summary>
        /// <value>The <see cref="T:System.Text.Encoding" />.</value>
        public override Encoding Encoding
        {
            get
            {
                return _encoding;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.LogWriter"/> class.
        /// </summary>
        /// <param name="filePath">The path to where the log should be stored.</param>
        /// <param name="encoding">The <see cref="T:System.Text.Encoding" /> to use for the log file.</param>
        /// <param name="standardOutput">Standard output.</param>
        public LogWriter(string filePath, Encoding encoding, TextWriter standardOutput)
        {
            _encoding = encoding;
            _consoleOutput = standardOutput;

            // Create our writer
            _writer = new StreamWriter(filePath, false, encoding);
        }

        /// <summary>
        /// Outputs the cached lines to the log file.
        /// </summary>
        public void WriteCache()
        {
            if (_cachedLines == null || _cachedLines.Count == 0) return;

#if DEBUG
            _consoleOutput.WriteLine("[CACHE]\tWriting " + _cachedLines.Count + " lines to LOG file.");
#endif

            // Loop over cached items and send them to the stream to be written
            string output = string.Empty;
            while (_cachedLines.Count > 0)
            {
                if (_cachedLines.TryDequeue(out output))
                {
                    _writer.WriteLine(output.Replace("\n\r", "\n"));
                }
            }


            // Flush the writer to make sure they actually were outputted
            _writer.Flush();
        }

        /// <summary>
        /// Override the WriteLine function with one that allows for caching.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void WriteLine(string value)
        {
            _consoleOutput.WriteLine(value);

            if (UseCache)
            {
                _cachedLines.Enqueue(value);
            }
            else
            {
                _writer.WriteLine(value.Replace("\n\r", "\n"));
                _writer.Flush();
            }
        }
    }
}