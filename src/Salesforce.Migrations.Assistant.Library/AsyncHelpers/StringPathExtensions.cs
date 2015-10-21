using System;
using System.IO;

namespace Salesforce.Migrations.Assistant.Library.AsyncHelpers
{
    public static class StringPathExtensions
    {
        public static string RemoveExtension(this string path)
        {
            if (path != null)
                return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));

            return String.Empty;
        }

        public static string SetExtension(this string path, string extension)
        {
            string extension1 = Path.GetExtension(path);
            path = !string.IsNullOrEmpty(extension) ? (!string.IsNullOrEmpty(extension1) ? Path.ChangeExtension(path, extension) : path + "." + extension) : RemoveExtension(path);
            return path;
        }

        public static string ReplaceUnixDirectorySeparator(this string path)
        {
            path = path.Replace('/', Path.DirectorySeparatorChar);
            return path;
        }

        public static string ReplaceWebSeparator(this string path)
        {
            path = path.Replace('\\', '/');
            return path;
        }
    }
}