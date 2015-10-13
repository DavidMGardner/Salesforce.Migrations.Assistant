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
        //public static void ProcessFiles(this SalesforceRepository repository, string getProjectLocation)
        //{
        //    // IEnumerable<SalesforceFileProxy> 
        //    string directory = repository.GetContext.OutputLocation;

        //    EnsureFolder(directory);

        //    var proxies = repository.FilteredList;

        //    var output = proxies.Select(s => new 
        //    {
        //        s.FileName,
        //        s.FullName,
        //        s.CreatedByName,
        //        s.ModifiedByName,
        //        s.PathInResource,
        //        s.Type,
        //        LastModifiedDate = s.LastModifiedDateUtcTicks == null && String.IsNullOrWhiteSpace(s.LastModifiedDateUtcTicks) ?
        //                                                                        DateTime.MinValue : DateTime.FromFileTimeUtc(long.Parse(s.LastModifiedDateUtcTicks)),

        //        CreatedDate = s.CreatedDateUtcTicks == null && String.IsNullOrWhiteSpace(s.CreatedDateUtcTicks) ? 
        //                                                                        DateTime.MinValue : DateTime.FromFileTimeUtc(long.Parse(s.CreatedDateUtcTicks))
        //    }).ToList();

        //    output.Dump(directory,"rawresponse");

        //    foreach (SalesforceFileProxy salesforceFileProxy in proxies)
        //    {
        //        WriteFile(salesforceFileProxy, directory);
        //    }
        //}
        
        public static void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
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
                        // this most likely is a share filename with multiple instances of the same file, we should check for duplicate
                        string objectDirectory = Path.Combine(directory, filepath[1]);
                        EnsureFolder(objectDirectory);

                        if (File.Exists(objectDirectory))
                        {
                             objectDirectory = Path.Combine(directory, String.Format("{0}-{1}",Guid.NewGuid().ToString(),filepath[1]));
                        }

                        return objectDirectory;
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

        public static bool SaveData(string fileName, byte[] data)
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
            catch(Exception ex)
            {
                Log.Error(ex.Message);
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
