﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Salesforce.Migrations.Assistant.Library.MetaDataService;
using Serilog;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class SalesforceRepository : IRepository<SalesforceFileProxy, string>
    {
        private readonly SalesforceContext _salesforceContext;
        public double ApiVersion { get; }

        private SalesforceContext GetContext => _salesforceContext;

        public SalesforceRepository(SalesforceContext salesforceContext)
        {
            InitalizeLogger();
            _salesforceContext = salesforceContext;
        }

        public SalesforceRepository()
        {
            InitalizeLogger();
            _salesforceContext = new SalesforceContext();
        }

        public IEnumerable<SalesforceFileProxy> List => GetList(new CancellationToken());
        public IEnumerable<SalesforceFileProxy> FilteredList => GetFilteredList(new CancellationToken());

        private IEnumerable<SalesforceFileProxy> GetList(CancellationToken cancellationToken)
        {
            PackageEntity pe = new PackageEntity().BuildQueryAllCommonTypes();
            return SalesforceRepositoryHelpers.DownloadAllFilesSynchronously(pe, GetContext, cancellationToken);
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


        private IEnumerable<SalesforceFileProxy> GetFilteredList(CancellationToken cancellationToken)
        {
            var pe = GetLatestFilesByDateOffSet(DateTime.Now.AddDays(-1));
            return SalesforceRepositoryHelpers.DownloadAllFilesSynchronously(pe, GetContext, cancellationToken);
        }


        private PackageEntity GetLatestFilesByDateOffSet(DateTime dto)
        {
            var query = BuildTypes(new string[]
            {
                "ApexClass", "ApexComponent", "ApexPage", "ApexTrigger", "Workflow", "RemoteSiteSetting",
                "PermissionSet", "CustomObject", "StaticResource", "Profile"
            });

            var response = query.Batch(3).Select(s => GetContext.MetadataServiceAdapter
                                                        .ListMetadata(s.ToArray(), 34.0))
                                                        .SelectMany(sm => sm)
                                                        .Where(w=>w.fullName.Contains("CP_") && w.lastModifiedDate >= dto)
                                                        .GroupBy(g => g.type)
                                                        .Select(s=> new PackageTypeEntity
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

        public string RunDeployment(byte[] zipFile, DeployOptions deployOptions)
        {
            var id = GetContext.MetadataServiceAdapter.Deploy(zipFile, deployOptions).id;
            return id;
        }
    }
}



//var fileName = String.Format("{0}\\fileproperties.json", ConfigurationManager.AppSettings["salesforce:context:workingdirectory"]);

//EnsureFolder(fileName);

//using (FileStream fs = File.Open(fileName, FileMode.Create))
//using (StreamWriter sw = new StreamWriter(fs))
//using (JsonWriter jw = new JsonTextWriter(sw))
//{
//    jw.Formatting = Formatting.Indented;

//    JsonSerializer serializer = new JsonSerializer();
//    serializer.Serialize(jw, response);
//}
