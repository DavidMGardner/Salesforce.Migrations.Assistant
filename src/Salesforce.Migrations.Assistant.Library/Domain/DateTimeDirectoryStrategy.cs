using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
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

        private string _outputDirectory;
        private string _packageDirectory;

     
        private void ProcessFiles(IEnumerable<SalesforceFileProxy> filesToSave, string directory)
        {
            SalesforceFileProcessing.EnsureFolder(directory);

            foreach (SalesforceFileProxy salesforceFileProxy in filesToSave)
            {
                var filename = SalesforceFileProcessing.EnsureFileName(salesforceFileProxy.FileName, directory);

                if (salesforceFileProxy.BinaryBody != null && !SalesforceFileProcessing.SaveByteArray(filename, salesforceFileProxy.BinaryBody))
                {
                    Log.Error("Couldn't write binary file to disk");
                }
            }
        }

        private void ProcessPackage(List<SalesforceFileProxy> filesToSave, string directory)
        {
            // save package.xml
            XmlDocument packageXml = filesToSave.GetPackageXml();
            var fullXmlFilename = SalesforceFileProcessing.EnsureFileName("package.xml", directory);
            packageXml.Save(fullXmlFilename);
        }


        public void Save(List<SalesforceFileProxy> filesToSave)
        {
            _outputDirectory = String.Format("{0}\\{1}\\package", Context.OutputLocation, LastRun.ToString("M-dd-yyyy-HH-mm-ss"));
            _packageDirectory = String.Format("{0}\\{1}", Context.OutputLocation, LastRun.ToString("M-dd-yyyy-HH-mm-ss"));

            ProcessFiles(filesToSave.Where(w => w.Type.ToLowerInvariant() != "package" && 
                                                w.Type.ToLowerInvariant() != "zip" && 
                                                w.Type.ToLowerInvariant() != "staticresourcezip"), _outputDirectory);

            ProcessFiles(filesToSave.Where(w => w.Type.ToLowerInvariant() == "zip"), _packageDirectory);
            ProcessPackage(filesToSave, _packageDirectory);

            LastRun = DateTime.Now;
        }
    }
}