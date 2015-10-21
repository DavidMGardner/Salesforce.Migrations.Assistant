using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Serilog;
using Formatting = Newtonsoft.Json.Formatting;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    static public class SalesforceFileProcessing
    {
        public static XmlDocument GetPackageXml(this List<SalesforceFileProxy> fileProxies)
        {
            // save a mater package xml with all types
            var grouped = fileProxies
                            .Where(w => !String.IsNullOrWhiteSpace(w.FullName))
                            .GroupBy(g => g.Type)
                            .Where(gw => gw.Key.ToLowerInvariant() != "package" && gw.Key.ToLowerInvariant() != "zip")
                            .Select(s => new PackageTypeEntity
                            {
                                Name = s.Key,
                                Members = s.Select(sm => sm.FullName).ToArray()
                            }).ToArray();

            var pe = new PackageEntity { Version = "29.0", Types = grouped };

            return pe.GetXml();
        }

        public static string EnsureResourceFileName(string file, string directory)
        {
            if (string.IsNullOrWhiteSpace(file))
                return string.Empty;

            var filepath = file.Split('\\');

            if (filepath.Length > 3)
            {
                StringBuilder filePathBuilder = new StringBuilder();
                for (int i = 1; i < filepath.Length; i++)
                {
                    filePathBuilder.AppendFormat(String.Format("\\{0}", filepath[i]));
                }

                string objectDirectory = Path.Combine(String.Format("{0}\\{1}", directory, filePathBuilder.ToString()));
                SalesforceFileProcessing.EnsureFolder(objectDirectory);

                return objectDirectory;
            }


            return string.Empty;
        }

        public static string EnsureFileName(string file, string directory)
        {
            if (string.IsNullOrWhiteSpace(file))
                return string.Empty;

            var filepath = file.Split('/');

            if (filepath.Length >= 3)
            {
                StringBuilder filePathBuilder = new StringBuilder();
                for (int i = 1; i < filepath.Length; i++)
                {
                    filePathBuilder.AppendFormat(String.Format("\\{0}", filepath[i]));
                }

                string objectDirectory = Path.Combine(String.Format("{0}\\{1}", directory, filePathBuilder.ToString()));
                SalesforceFileProcessing.EnsureFolder(objectDirectory);

                return objectDirectory;
            }

            // lame special handling
            switch (filepath.Length)
            {
                case 1:
                    {
                        string objectDirectory = Path.Combine(directory, file);
                        SalesforceFileProcessing.EnsureFolder(objectDirectory);

                        return objectDirectory;
                    }
                case 2:
                    {
                        // this most likely is a share filename with multiple instances of the same file, we should check for duplicate
                        string objectDirectory = Path.Combine(directory, filepath[1]);
                        SalesforceFileProcessing.EnsureFolder(objectDirectory);

                        if (File.Exists(objectDirectory))
                        {
                            objectDirectory = Path.Combine(directory, String.Format("{0}-{1}", Guid.NewGuid().ToString(), filepath[1]));
                        }

                        return objectDirectory;
                    }
            }

            return String.Empty;
        }


        public static void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        public static bool SaveByteArray(string fileName, byte[] data)
        {
            try
            {
                // Create a new stream to write to the file
                var writer = new BinaryWriter(File.OpenWrite(fileName));

                // Writer raw data                
                writer.Write(data);
                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }

            return true;
        }

        private static void Dump(this object o, string directory, string fileName)
        {
            var outFileName = String.Format("{0}\\{1}.json", directory, fileName);

            EnsureFolder(outFileName);

            using (FileStream fs = File.Open(outFileName, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, o);
            }
        }
    }
}
