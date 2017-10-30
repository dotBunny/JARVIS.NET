using System.IO;

namespace JARVIS.Shared
{
    public static class IO
    {
        public static void WriteContents(string file, string contents)
        {
            File.WriteAllText(file, contents, System.Text.Encoding.UTF8);
        }
    }
}
