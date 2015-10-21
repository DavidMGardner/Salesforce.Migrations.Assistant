using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zip;
using Newtonsoft.Json;
using Salesforce.Migrations.Assistant.Library.Configuration;
using Salesforce.Migrations.Assistant.Library.Exceptions;
using Salesforce.Migrations.Assistant.Library.MetaDataService;
using Serilog;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public interface IPersistenceStrategy
    {
        SalesforceContext Context { get; set; }
        void Save(List<SalesforceFileProxy> filesToSave);
    }

    public class SalesforceRepository : IRepository<SalesforceFileProxy, string>
    {
        public double ApiVersion { get; }

        public SalesforceContext GetContext { get; }
        private readonly IPersistenceStrategy _persistenceStrategy;

        public SalesforceRepository(SalesforceContext salesforceContext, IPersistenceStrategy persistenceStrategy)
        {
            if (salesforceContext == null) throw new InvalidSalesforceContextException();

            InitalizeLogger();
            GetContext = salesforceContext;
            _persistenceStrategy = persistenceStrategy;
            _persistenceStrategy.Context = GetContext;
            ApiVersion = 34.0;
        }

        private void InitalizeLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();
        }

        private ListMetadataQuery[] BuildTypes(string[] types)
        {
            IEnumerable<ListMetadataQuery> typeList = types.Select(s => new ListMetadataQuery() {type = s});
            return typeList.ToArray();
        }

        public IEnumerable<SalesforceFileProxy> DownloadFiles(PackageEntity pe, CancellationToken cancellationToken)
        {
            return SalesforceRepositoryHelpers.DownloadAllFilesSynchronously(pe, GetContext, cancellationToken);
        }

        public PackageEntity GetLatestFiles(Func<MetaDataService.FileProperties, int, bool> predicate)
        {
            var query = BuildTypes(new string[]
            {
                "ApexClass", "ApexComponent", "ApexPage", "ApexTrigger", "Workflow", "RemoteSiteSetting",
                "PermissionSet", "CustomObject", "StaticResource", "Profile"
            });

            var response = query.Batch(3).Select(s => GetContext.MetadataServiceAdapter
                .ListMetadata(s.ToArray(), 34.0))
                .SelectMany(sm => sm)
                .Where(predicate)
                .GroupBy(g => g.type)
                .Select(s => new PackageTypeEntity
                {
                    Name = s.Key,
                    Members = s.Select(sm => sm.fullName).ToArray()
                })
                .ToArray();

            return new PackageEntity {Version = "29.0", Types = response};
        }


        public void Add(SalesforceFileProxy entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(SalesforceFileProxy entity)
        {
            throw new NotImplementedException();
        }

        public void Update(SalesforceFileProxy entity)
        {
            throw new NotImplementedException();
        }

        public SalesforceFileProxy FindById(string id)
        {
            throw new NotImplementedException();
        }

        public void SaveLocal(IEnumerable<SalesforceFileProxy> entities)
        {
            if (_persistenceStrategy == null) throw new InvalidPersistenceStrategyException();

            _persistenceStrategy.Save(entities.ToList());
        }

        public void SaveLocal(Func<MetaDataService.FileProperties, int, bool> predicate,
            CancellationToken cancellationToken)
        {
            if (_persistenceStrategy == null) throw new InvalidPersistenceStrategyException();

            var entity = GetLatestFiles(predicate);
            var pe = DownloadFiles(entity, cancellationToken);

            SaveLocal(pe);
        }

        public string RunDeployment(byte[] zipFile, DeployOptions deployOptions)
        {
            var id = GetContext.MetadataServiceAdapter.Deploy(zipFile, deployOptions).id;
            return id;
        }

        
        public byte[] PackageStaticResources(string folder, DeployOptions deployOptions)
        {
            //var id = GetContext.MetadataServiceAdapter.Deploy(zipFile, deployOptions).id;
            //return id;
            if (!Directory.Exists(folder)) throw new DirectoryNotFoundException();

            var staticResourceFolder = Path.Combine(folder, "package\\staticresources");

            if (!Directory.Exists(staticResourceFolder))
                throw new DirectoryNotFoundException("Couldn't find static resources subfolder under package folder.");

            var directories = Directory.EnumerateDirectories(staticResourceFolder);

            List<IDeployableItem> deployableItems = new List<IDeployableItem>();
            IList<IDeployableItem> members = new List<IDeployableItem>();

            foreach (var directory in directories)
            {
                var splits = directory.Split('\\');
                var zipName = splits[splits.Length - 1];
                string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

                var directoryPlusZipName = Path.Combine(staticResourceFolder, zipName);

                using (ZipFile zip = new ZipFile())
                {
                    foreach (var file in files)
                    {
                        var zipFileNamePlusDirectory = file.Replace(directoryPlusZipName, String.Empty);
                        var zipDirectory = Path.GetDirectoryName(zipFileNamePlusDirectory);
                        zip.AddFile(file, zipDirectory);
                    }

                    MemoryStream memoryStream = new MemoryStream();
                    zip.Save(memoryStream);
                    
                    members.Add(new StaticResourceDeployableItem
                    {
                        FileBody = memoryStream.ToArray(),
                        FileName = String.Format("{0}\\{1}.resource", directory, zipName),
                        FileNameWithoutExtension = String.Format("{0}", zipName)
                    });

                    XmlOutput xo = new XmlOutput()
                        .XmlDeclaration()
                        .Node("StaticResource").Attribute("xmlns", "http://soap.sforce.com/2006/04/metadata").Within()
                        .Node("cacheControl").InnerText("Public")
                        .Node("contentType").InnerText("application/zip").EndWithin();

                    members.Add(new StaticResourceDeployableItem
                    {
                        FileBody = System.Text.Encoding.Default.GetBytes(xo.GetOuterXml()),
                        FileName = String.Format("{0}\\{1}.resource-meta.xml", directory, zipName),
                        FileNameWithoutExtension = String.Format("{0}.resource-meta.xml", zipName),
                        AddToPackage = false
                    });
                }
            }

            PackageEntity pe = new PackageEntity
            {
                Types = new[]
                {
                    new PackageTypeEntity
                    {
                        Members = members.Where(w=>w.AddToPackage).Select(s => s.FileNameWithoutExtension).ToArray(),
                        Name = "StaticResource"
                    }
                },
                Version = "29.0"
            };

            var zipFile = UnzipPackageFilesHelper.ZipObjectsForDeploy(members, pe.GetXml().OuterXml);
            SalesforceFileProcessing.SaveByteArray(String.Format("{0}\\package_{1}.zip", folder, Guid.NewGuid()), zipFile);

            return zipFile;
        }
    }
}

/*

        private IEnumerable<SalesforceFileProxy> GetFilteredList(CancellationToken cancellationToken)
        {
            var pe = GetLatestFilesByDateOffSet(DateTime.Now.AddDays(-1));
            return SalesforceRepositoryHelpers.DownloadAllFilesSynchronously(pe, GetContext, cancellationToken);
        }


        private PackageEntity GetLatestFilesByDateOffSet(DateTime dto)
        {
            return GetLatestFiles((properties, i) => properties.fullName.Contains("CP_") && properties.lastModifiedDate >= dto);
        }

    */

