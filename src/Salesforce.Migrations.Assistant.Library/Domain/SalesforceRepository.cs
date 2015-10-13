using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            IEnumerable<ListMetadataQuery> typeList = types.Select(s => new ListMetadataQuery() { type = s });
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

            return new PackageEntity { Version = "29.0", Types = response };
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

        public void SaveLocal(Func<MetaDataService.FileProperties, int, bool> predicate, CancellationToken cancellationToken)
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

