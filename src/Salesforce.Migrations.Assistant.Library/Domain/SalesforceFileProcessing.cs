using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    static public class SalesforceFileProcessing
    {
        public static void ProcessFiles(this IEnumerable<SalesforceFileProxy> proxies)
        {
            string directory = String.Format("{0}\\{1}", ConfigurationManager.AppSettings["salesforce:context:workingdirectory"], DateTime.Now.ToString("M-dd-yyyy-HH-mm-ss"));

            EnsureFolder(directory);

            proxies.Dump();

            foreach (SalesforceFileProxy salesforceFileProxy in proxies)
            {
                WriteFile(salesforceFileProxy, directory);
            }
        }

        private static void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        private static void Test()
        {
            IEnumerable<SalesforceFileProxy> fileProxies =
                JsonConvert.DeserializeObject<IEnumerable<SalesforceFileProxy>>(File.ReadAllText(@"D:\\salesforce.migrations\\Salesforce.Migrations.Assistant.Library.Domain.SalesforceFileProxy[].json"));

            if (fileProxies.Any())
            {
                string directory = ConfigurationManager.AppSettings["salesforce:context:workingdirectory"];
                ProcessFiles(fileProxies);
            }
        }

        private static string EnsureFileName(string file, string directory)
        {
            if (string.IsNullOrWhiteSpace(file))
                return string.Empty;

            var filepath = file.Split('/');

            switch (filepath.Length)
            {
                case 2:
                    {
                        break;
                    }

                case 3:
                    {
                        string objectDirectory = Path.Combine(directory, filepath[1]);
                        var newPath = Path.Combine(objectDirectory, filepath[2]);
                        EnsureFolder(newPath);

                        return newPath;
                    }
            }

            return String.Empty;
        }

        private static bool SaveData(string fileName, byte[] data)
        {
            BinaryWriter Writer = null;
            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.OpenWrite(fileName));

                // Writer raw data                
                Writer.Write(data);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                //...
                return false;
            }

            return true;
        }

        private static void WriteFile(SalesforceFileProxy salesforceFileProxy, string directory)
        {
            var filename = EnsureFileName(salesforceFileProxy.FileName, directory);

            if (salesforceFileProxy.BinaryBody != null && !SaveData(filename, salesforceFileProxy.BinaryBody))
            {
                Log.Error("Couldn't write binary file to disk");
            }
        }


        private static void Dump(this object o)
        {
            var fileName = String.Format("{0}\\{1}.json", ConfigurationManager.AppSettings["salesforce:context:workingdirectory"], o.GetType().ToString());

            EnsureFolder(fileName);

            using (FileStream fs = File.Open(fileName, FileMode.Create))
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
