using System;
using System.Collections.Generic;
using System.IO;
using Serilog;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class FlatDirectoryStrategy : IPersistenceStrategy
    {
        public SalesforceContext Context { get; set; }
        public void Save(List<SalesforceFileProxy> filesToSave)
        {
            throw new NotImplementedException();
        }
    }



    public class DateTimeDirectoryStrategy : IPersistenceStrategy
    {
        public SalesforceContext Context { get; set; }

        public DateTime LastRun { get; set; } = DateTime.Now;

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
                    SalesforceFileProcessing.EnsureFolder(objectDirectory);

                    if (File.Exists(objectDirectory))
                    {
                        objectDirectory = Path.Combine(directory, String.Format("{0}-{1}", Guid.NewGuid().ToString(), filepath[1]));
                    }

                    return objectDirectory;
                }

                case 3:
                {
                    string objectDirectory = Path.Combine(directory, filepath[1]);
                    var newPath = Path.Combine(objectDirectory, filepath[2]);
                    SalesforceFileProcessing.EnsureFolder(newPath);

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
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }

            return true;
        }

        public void Save(List<SalesforceFileProxy> filesToSave)
        {
            string directory = String.Format("{0}\\{1}",Context.OutputLocation, LastRun.ToString("M-dd-yyyy-HH-mm-ss"));
            SalesforceFileProcessing.EnsureFolder(directory);

            foreach (SalesforceFileProxy salesforceFileProxy in filesToSave)
            {
                var filename = EnsureFileName(salesforceFileProxy.FileName, directory);

                if (salesforceFileProxy.BinaryBody != null && !SaveData(filename, salesforceFileProxy.BinaryBody))
                {
                    Log.Error("Couldn't write binary file to disk");
                }
            }

            LastRun = DateTime.Now;
        }
    }
}