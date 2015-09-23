using System;
using System.Collections.Generic;
using System.Threading;
using Salesforce.Migrations.Assistant.Library.Domain;
using Salesforce.Migrations.Assistant.Library.MetaDataService;
using DeployOptions = Salesforce.Migrations.Assistant.Library.Domain.DeployOptions;
using DeployResult = Salesforce.Migrations.Assistant.Library.MetaDataService.DeployResult;

namespace Salesforce.Migrations.Assistant.Library
{
    public interface ISalesforceDomainServices
    {
        bool IsLoggedIn { get; }
        bool Login(string username, string password, string securityToken = null, SalesforceEnvironmentType environment = SalesforceEnvironmentType.CustomDomain, string environmentUrl = null);
        string RunDeployment(byte[] zipFile, DeployOptions deployOptions);

        double ApiVersion { get; }
        string DownloadLogFile(string id);

        
        OperationResult CreateNewSalesForceFile(SalesforceFileProxy entity);
        SalesforceFileProxy QueryApexFileByName(string fileName, string type);
        OperationResult DeleteFileFromSalesForce(string fileId);
        HashSet<SalesforceItem> QueryToolingItemNamesByType(MetadataType type);
        OperationResult BuildFiles(IEnumerable<ICompilableItem> changedFiles);
        OperationResult UpdateStaticResources(IEnumerable<IStaticResource> changedFiles);
        List<SalesforceFileProxy> DownloadFiles(Dictionary<MetadataType, IEnumerable<string>> filesToDownload);
        List<SalesforceFileProxy> DownloadFiles(MetadataType type, string[] filesToDownload);
        List<SalesforceFileProxy> DownloadAllFilesSynchronously(PackageEntity package, CancellationToken cancellationToken);

        RetrieveRequest[] ConvertPackageToRequests(PackageEntity package);
        RetrieveResult CheckRetrieveResult(string id);
        Dictionary<string, DateTime> GetLastModifiedOnServerForMetadataSet(List<MetadataType> metadataTypes, IEnumerable<string> workflowNames);
        Dictionary<string, DateTime> GetLastModifiedOnServer(Dictionary<MetadataType, IEnumerable<string>> filesToCheck);
        List<string> GetTriggerableObjects();
        List<string> GetQueryableObjects();
        List<LogDescriptor> GetLogs(DateTime fromDate, int limit = 100);
        List<LogDescriptor> GetLogs(DateTime fromDate, string operation, int limit = 100);

        string GetUserId();
        void ResetLogs(string userId);
        List<TestClass> DownloadTests();
        string RunTestClasses(List<TestClass> testClasses);
        List<TestQueueItem> GetTestQueueItemsSinceAndExceptParentJobs(DateTime dateSince, List<string> parentJobIds);
        List<TestQueueItem> GetTestQueueItemsByParentJobId(string parentJobId);
        List<TestQueueItem> GetTestClassStatuses(List<string> testQueueIds);
        List<TestResult> GetTestResult(string testClassId, string testQueueItemId);

        FileProperties[] GetFilesPropertiesDependsOnMetadataTypes(List<string> metadataTypes);
        FileProperties[] GetFilesPropertiesDependsOnMetadataTypes(MetadataType metadataType);
        Dictionary<string, DateTime> GetLastModifiedDateForType(MetadataType type, string[] ids);

        DeployResult DeployComponents(byte[] zipFile, DeployOptions deployOptions, int timeToPollingDeployResultInSeconds, int intervalInSeconds);

        DeployResult GetDeployResult(string asyncResultId);
    }
}