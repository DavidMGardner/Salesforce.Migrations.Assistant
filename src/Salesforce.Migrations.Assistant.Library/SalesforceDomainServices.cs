using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using Moo.Extenders;
using Salesforce.Migrations.Assistant.Library.AsyncHelpers;
using Salesforce.Migrations.Assistant.Library.Domain;
using Salesforce.Migrations.Assistant.Library.Exceptions;
using Salesforce.Migrations.Assistant.Library.MetaDataService;
using Salesforce.Migrations.Assistant.Library.Services;
using Salesforce.Migrations.Assistant.Library.Tooling.SforceService;
using Serilog;
using static Salesforce.Migrations.Assistant.Library.Domain.SalesforceQueryExtensions;
using DeployOptions = Salesforce.Migrations.Assistant.Library.Domain.DeployOptions;
using DeployResult = Salesforce.Migrations.Assistant.Library.MetaDataService.DeployResult;
using LoginResult = Salesforce.Migrations.Assistant.Library.Partner.SforceService.LoginResult;
using SaveResult = Salesforce.Migrations.Assistant.Library.Tooling.SforceService.SaveResult;
using ErrorResult = Salesforce.Migrations.Assistant.Library.Tooling.SforceService.Error;
using DeployMessage = Salesforce.Migrations.Assistant.Library.Tooling.SforceService.DeployMessage;

using ApexClass = Salesforce.Migrations.Assistant.Library.Tooling.SforceService.ApexClass1;
using ApexComponent = Salesforce.Migrations.Assistant.Library.Tooling.SforceService.ApexComponent1;

namespace Salesforce.Migrations.Assistant.Library
{
    public class SalesforceDomainServices 
    {
      
        

        //public bool IsLoggedIn
        //{
        //    get { return _isLoggedIn; }
        //    private set { _isLoggedIn = value; }
        //}

        public static string ClientName { get; } = "Sogeti Migrations Assistant";

        public SalesforceDomainServices()
        {
        

          
        }
        
      
        //public string RunDeployment(byte[] zipFile, DeployOptions deployOptions)
        //{
        //    var id = _metadataServiceAdapter.Deploy(zipFile, deployOptions).id;
        //    return id;
        //}

        public double ApiVersion { get; }

        public string DownloadLogFile(string id)
        {
            throw new NotImplementedException();
        }

        public OperationResult CreateNewSalesForceFile(SalesforceFileProxy entity)
        {
            throw new NotImplementedException();
        }


        public OperationResult DeleteFileFromSalesForce(string fileId)
        {
            throw new NotImplementedException();
        }

        public HashSet<SalesforceItem> QueryToolingItemNamesByType(MetadataType type)
        {
            throw new NotImplementedException();
        }

        public OperationResult BuildFiles(IEnumerable<ICompilableItem> changedFiles)
        {
            throw new NotImplementedException();
        }

    
        //private string CreateMetadataContainer()
        //{
        //    return SessionExpirationWrapper(() => _toolingServiceAdapter.Create(new sObject[1]
        //    {
        //        (sObject) new MetadataContainer()
        //        {
        //            Name = Guid.NewGuid().ToString().Substring(0, 32)
        //        }
        //    }))[0].id;
        //}

        //public ContainerAsyncRequest[] CompileApexMetadataContainer(string metadataContainerId, bool noDeploy, string classMemberId = null, IIncrementalSleepPolicy sleepPolicy = null)
        //{
        //    if (sleepPolicy == null)
        //        sleepPolicy = new FixedSleepPolicy();

        //    ContainerAsyncRequest[] compileRequest = { new ContainerAsyncRequest()
        //    {
        //        MetadataContainerId = metadataContainerId,
        //        MetadataContainerMemberId = classMemberId,
        //        IsCheckOnly = noDeploy,
        //        IsCheckOnlySpecified = true
        //    }};

        //    SaveResult[] createCompileResult = SessionExpirationWrapper(() => _toolingServiceAdapter.Create(new sObject[] {compileRequest[0]}));

        //    ContainerAsyncRequest[] containerAsyncRequestArray = TimeoutRunner.RunUntil(() => Compile(createCompileResult), x => x[0].State != "Queued", sleepPolicy, "compiling");
        //    if (containerAsyncRequestArray != null)
        //        return containerAsyncRequestArray;

        //    compileRequest[0] = new ContainerAsyncRequest()
        //    {
        //        MetadataContainerId = metadataContainerId,
        //        MetadataContainerMemberId = classMemberId,
        //        State = "Aborted"
        //    };

        //    SessionExpirationWrapper(() => _toolingServiceAdapter.Create(new sObject[1]
        //    {
        //        compileRequest[0]
        //    }));

        //    throw new BuildAbortedTimeoutException("Time of the request to Salesforce expired. Build aborted.");
        //}

        //private ContainerAsyncRequest[] Compile(SaveResult[] createCompileResult)
        //{
        //    return SessionExpirationWrapper(() => _toolingServiceAdapter.Retrieve("Id, ErrorMsg, CompilerErrors, State", "ContainerAsyncRequest", new string[1]
        //    {
        //        createCompileResult[0].id
        //    }).OfType<ContainerAsyncRequest>().ToArray());
        //}

        //public OperationResult BuildFiles(IEnumerable<ICompilableItem> changedFiles)
        //{
        //    OperationResult operationResult = new OperationResult();
        //    string metadataContainer = this.CreateMetadataContainer();
        //    List<sObject> objects = new List<sObject>();
        //    var compilableItems = changedFiles as IList<ICompilableItem> ?? changedFiles.ToList();

        //    objects.AddRange(GetApexMembers<ApexClassMember>(compilableItems, metadataContainer));
        //    objects.AddRange(GetApexMembers<ApexComponentMember>(compilableItems, metadataContainer));
        //    objects.AddRange(GetApexMembers<ApexTriggerMember>(compilableItems, metadataContainer));
        //    objects.AddRange(GetApexMembers<ApexPageMember>(compilableItems, metadataContainer));

          
        //    List<SaveResult> saveList = new List<SaveResult>();
        //    if (objects.Count > 0)
        //    {
        //        saveList.AddRange(SessionExpirationWrapper(() => _toolingServiceAdapter.Create(objects.ToArray())));
        //    }

        //    if (saveList.Any(o => o.errors != null))
        //    {
        //        foreach (var saveResult in saveList)
        //        {
        //            if (saveResult.errors != null)
        //            {
        //                List<ErrorDescriptor> errorList = saveResult.errors.Select(e => new ErrorDescriptor()
        //                    {
        //                        Id = e.statusCode.ToString(),
        //                        Name = e.message,
        //                        Problem = e.fields == null ? string.Empty : string.Join(";", e.fields)
        //                    }).ToList();
        //                operationResult.Errors.AddRange(errorList);
        //            }
                    
        //        }
        //    }

        //    ContainerAsyncRequest[] containerAsyncRequestArray = CompileApexMetadataContainer(metadataContainer, false, null);

        //    if (containerAsyncRequestArray.Any(o => o.DeployDetails.componentFailures.Any()))
        //    {
        //        operationResult.Errors.AddRange(containerAsyncRequestArray.SelectMany(sm => sm.DeployDetails.componentFailures)
        //                                                    .Select(s => new ErrorDescriptor
        //                                                    {
        //                                                        Name = s.fullName,
        //                                                        Id = s.id,
        //                                                        Line = s.lineNumber.ToString(),
        //                                                        Problem = s.problem,
        //                                                        ProblemType = s.problemType
        //                                                    }));
        //    }
                
        //    return operationResult;
        //}

        private static SalesforceFileProxy FileFactory(dynamic sforceObject, string type)
        {
            SalesforceFileProxy salesFileEntity = new SalesforceFileProxy
            {
                CreatedByName = sforceObject.CreatedById,
                Id = sforceObject.Id,
                NamespacePrefix = sforceObject.NamespacePrefix,
                Type = type
            };


            if (sforceObject.CreatedDate != null)
                salesFileEntity.CreatedDateUtcTicks = sforceObject.CreatedDate.ToFileTimeUtc().ToString();
            
            if (sforceObject.LastModifiedDate != null)
                salesFileEntity.LastModifiedDateUtcTicks = sforceObject.LastModifiedDate.ToFileTimeUtc().ToString();

            try
            {
                if (DynamicExtensions.IsPropertyExist(sforceObject, "Body"))
                    salesFileEntity.FileBody = sforceObject.Body;

                if (DynamicExtensions.IsPropertyExist(sforceObject, "Name"))
                    salesFileEntity.FileName = sforceObject.Name;
            }
            catch
            {
                // ignored
            }
            return salesFileEntity;
        }

        //private IEnumerable<SalesforceFileProxy> QueryItems(SalesforceQuery query)
        //{
        //    string queryText = query.ToString();
        //    QueryResult queryResult = SessionExpirationWrapper(() => _toolingServiceAdapter.Query(queryText));
        //    if (queryResult.size <= 0)
        //        return null;

        //    IEnumerable<SalesforceFileProxy> sforceObjects = queryResult.records.Select(s => FileFactory(s, query.Type()));

        //    return sforceObjects.ToList();
        //}

        private static byte[] ToByteArray(string str)
        {
            byte[] numArray = new byte[str.Length * 2];
            Buffer.BlockCopy(str.ToCharArray(), 0, numArray, 0, numArray.Length);
            return numArray;
        }

        //private SalesforceFileProxy QueryByName<T>(string type, string name, string @operator) where T : class
        //{
        //    QueryResult queryResult = SessionExpirationWrapper(() =>
        //       _toolingServiceAdapter.Query(string.Format("select Id, Name, CreatedDate, CreatedById, NamespacePrefix, LastModifiedDate, Body from {0} where Name {1} '{2}'", type, @operator, name)));
        //    if (queryResult.size <= 0)
        //        return null;

        //    SalesforceFileProxy salesFileEntity = new SalesforceFileProxy();

        //    dynamic sforceObject = queryResult.records[0];

        //    salesFileEntity.CreatedByName = sforceObject.CreatedById;
        //    if (sforceObject.CreatedDate != null)
        //        salesFileEntity.CreatedDateUtcTicks = sforceObject.CreatedDate.Value.ToFileTimeUtc().ToString();

        //    salesFileEntity.Id = sforceObject.Id;
        //    salesFileEntity.NamespacePrefix = sforceObject.NamespacePrefix;
        //    salesFileEntity.Type = type;

        //    if (sforceObject.LastModifiedDate != null)
        //        salesFileEntity.LastModifiedDateUtcTicks = sforceObject.LastModifiedDate.Value.ToFileTimeUtc().ToString();

        //    salesFileEntity.FileBody = sforceObject.Body;
        //    salesFileEntity.FileName = sforceObject.Name;
        //    return salesFileEntity;
        //}
      

        public OperationResult UpdateStaticResources(IEnumerable<IStaticResource> changedFiles)
        {
            throw new NotImplementedException();
        }

        public List<SalesforceFileProxy> DownloadFiles(Dictionary<MetadataType, IEnumerable<string>> filesToDownload)
        {
            throw new NotImplementedException();
        }

        public List<SalesforceFileProxy> DownloadFiles(MetadataType type, string[] filesToDownload)
        {
            throw new NotImplementedException();
        }

        public RetrieveRequest[] ConvertPackageToRequests(PackageEntity package)
        {
            return package.Types.Select(t =>
            {
                RetrieveRequest retrieveRequest = new RetrieveRequest
                {
                    apiVersion = double.Parse(package.Version, CultureInfo.InvariantCulture),
                    unpackaged = new Package()
                    {
                        types = new[]
                        {
                            new PackageTypeMembers()
                            {
                                name = t.Name,
                                members = t.Members
                            }
                        }
                    }
                };
                return retrieveRequest;
            }).ToArray();
        }

       

        //public IEnumerable<SalesforceFileProxy> GetFilesChangedSince(DateTimeOffset dto)
        //{
        //    List<SalesforceFileProxy> result = new List<SalesforceFileProxy>();

        //    QueryItems(QueryApexClassesByOffest(dto)).NullSafeAddRange(result);
        //    QueryItems(QueryApexComponentsByOffest(dto)).NullSafeAddRange(result);
        //    QueryItems(QueryApexTriggersByOffest(dto)).NullSafeAddRange(result);
        //    QueryItems(QueryApexPagesByOffest(dto)).NullSafeAddRange(result);
        //    QueryItems(QueryStaticResourcesByOffest(dto)).NullSafeAddRange(result);

        //    return result;
        //}

        public Dictionary<string, DateTime> GetLastModifiedOnServerForMetadataSet(List<MetadataType> metadataTypes, IEnumerable<string> workflowNames)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, DateTime> GetLastModifiedOnServer(Dictionary<MetadataType, IEnumerable<string>> filesToCheck)
        {
            var query = new SalesforceQuery().Select("Id, Name, CreatedDate, CreatedById, NamespacePrefix, LastModifiedDate, Body").From("{0}")
                                                    .Where("LastModifiedDate")
                                                    .GreaterThanDateTime(DateTimeOffset.Parse("09/21/2015").ToString("o"));

            //Dictionary<string, DateTime> result = new Dictionary<string, DateTime>();
            //foreach (MetadataType index in filesToCheck.Keys)
            //{
            //    if (!Enumerable.Any<string>(filesToCheck[index]))
            //    {
            //        MetadataType type = index;
            //        sObject[] sObjectArray = this.SessionExpirationWrapper((Func<sObject[]>)(() => _toolingServiceAdapter.Retrieve("Id, LastModifiedDate", type.ToString(), Enumerable.ToArray<string>(filesToCheck[type]))));
            //        if (index == MetadataType.ApexClass)
            //            LinqExtensions.ForEach<WelkinSuite.Salesforce.Gateway.Tooling.ApexClass>(Enumerable.OfType<WelkinSuite.Salesforce.Gateway.Tooling.ApexClass>((IEnumerable)sObjectArray), (Action<WelkinSuite.Salesforce.Gateway.Tooling.ApexClass>)(element => result[element.Id] = element.LastModifiedDate.Value));
            //        if (index == MetadataType.ApexComponent)
            //            LinqExtensions.ForEach<WelkinSuite.Salesforce.Gateway.Tooling.ApexComponent>(Enumerable.OfType<WelkinSuite.Salesforce.Gateway.Tooling.ApexComponent>((IEnumerable)sObjectArray), (Action<WelkinSuite.Salesforce.Gateway.Tooling.ApexComponent>)(element => result[element.Id] = element.LastModifiedDate.Value));
            //        if (index == MetadataType.ApexTrigger)
            //            LinqExtensions.ForEach<WelkinSuite.Salesforce.Gateway.Tooling.ApexTrigger>(Enumerable.OfType<WelkinSuite.Salesforce.Gateway.Tooling.ApexTrigger>((IEnumerable)sObjectArray), (Action<WelkinSuite.Salesforce.Gateway.Tooling.ApexTrigger>)(element => result[element.Id] = element.LastModifiedDate.Value));
            //        if (index == MetadataType.ApexPage)
            //            LinqExtensions.ForEach<WelkinSuite.Salesforce.Gateway.Tooling.ApexPage>(Enumerable.OfType<WelkinSuite.Salesforce.Gateway.Tooling.ApexPage>((IEnumerable)sObjectArray), (Action<WelkinSuite.Salesforce.Gateway.Tooling.ApexPage>)(element => result[element.Id] = element.LastModifiedDate.Value));
            //        if (index == MetadataType.StaticResource)
            //            LinqExtensions.ForEach<WelkinSuite.Salesforce.Gateway.Tooling.StaticResource>(Enumerable.OfType<WelkinSuite.Salesforce.Gateway.Tooling.StaticResource>((IEnumerable)sObjectArray), (Action<WelkinSuite.Salesforce.Gateway.Tooling.StaticResource>)(element => result[element.Id] = element.LastModifiedDate.Value));
            //    }
            //}
            //return result;

            return new Dictionary<string, DateTime>();
        }

        public List<string> GetTriggerableObjects()
        {
            throw new NotImplementedException();
        }

        public List<string> GetQueryableObjects()
        {
            throw new NotImplementedException();
        }

        public List<LogDescriptor> GetLogs(DateTime fromDate, int limit = 100)
        {
            throw new NotImplementedException();
        }

        public List<LogDescriptor> GetLogs(DateTime fromDate, string operation, int limit = 100)
        {
            throw new NotImplementedException();
        }

        public string GetUserId()
        {
            throw new NotImplementedException();
        }

        public void ResetLogs(string userId)
        {
            throw new NotImplementedException();
        }

        public List<TestClass> DownloadTests()
        {
            throw new NotImplementedException();
        }

        public string RunTestClasses(List<TestClass> testClasses)
        {
            throw new NotImplementedException();
        }

        public List<TestQueueItem> GetTestQueueItemsSinceAndExceptParentJobs(DateTime dateSince,
            List<string> parentJobIds)
        {
            throw new NotImplementedException();
        }

        public List<TestQueueItem> GetTestQueueItemsByParentJobId(string parentJobId)
        {
            throw new NotImplementedException();
        }

        public List<TestQueueItem> GetTestClassStatuses(List<string> testQueueIds)
        {
            throw new NotImplementedException();
        }

        public List<TestResult> GetTestResult(string testClassId, string testQueueItemId)
        {
            throw new NotImplementedException();
        }

        public FileProperties[] GetFilesPropertiesDependsOnMetadataTypes(List<string> metadataTypes)
        {
            throw new NotImplementedException();
        }

        public FileProperties[] GetFilesPropertiesDependsOnMetadataTypes(MetadataType metadataType)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, DateTime> GetLastModifiedDateForType(MetadataType type, string[] ids)
        {
            throw new NotImplementedException();
        }

        public DeployResult DeployComponents(byte[] zipFile, DeployOptions deployOptions,
            int timeToPollingDeployResultInSeconds,
            int intervalInSeconds)
        {
            throw new NotImplementedException();
        }

        public DeployResult GetDeployResult(string asyncResultId)
        {
            throw new NotImplementedException();
        }
    }
}

static public class DateTimeProvider 
{
    static public DateTime UtcNow => DateTime.UtcNow;
}
