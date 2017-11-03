using System.IO;

namespace JARVIS.Shared
{
    public static class IO
    {
        public static void WriteContents(string file, string contents)
        {
            File.WriteAllText(file, contents, System.Text.Encoding.UTF8);
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
