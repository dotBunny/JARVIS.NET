using System;
using System.Collections.Generic;
namespace JARVIS.Shared
{
    public static class Web
    {
        private static char[] ParameterStart = { '?' };
        private static char[] ParameterDelimiter = { ',' };
        public static string[] GetParameters(string rawUrl, string pathToRemove)
        {
            return rawUrl.Replace(pathToRemove, string.Empty).TrimStart(ParameterStart).Split(ParameterDelimiter);
        }
    }
}