using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using Moo.Extenders;
using Salesforce.Migrations.Assistant.Library.Domain;
using Salesforce.Migrations.Assistant.Library.MetaDataService;
using Salesforce.Migrations.Assistant.Library.Services;
using Salesforce.Migrations.Assistant.Library.Tooling.SforceService;
using Serilog;
using DeployOptions = Salesforce.Migrations.Assistant.Library.Domain.DeployOptions;
using DeployResult = Salesforce.Migrations.Assistant.Library.MetaDataService.DeployResult;
using LoginResult = Salesforce.Migrations.Assistant.Library.Partner.SforceService.LoginResult;
using SaveResult = Salesforce.Migrations.Assistant.Library.Tooling.SforceService.SaveResult;

namespace Salesforce.Migrations.Assistant.Library
{
    public enum SalesforceEnvironmentType
    {
        ProductionOrDeveloper,
        Sandbox,
        CustomDomain,
    }

    public class SalesforceDomainServices : ISalesforceDomainServices
    {
        private string _userName;
        private string _password;
        private string _token;
        private SalesforceEnvironmentType _environmentType;
        private string _url;
        private readonly ISalesforcePartnerServiceAdapter _partnerServiceAdapter;
        private readonly ISalesforceToolingServiceAdapter _toolingServiceAdapter;
        private readonly ISalesforceMetadataServiceAdapter _metadataServiceAdapter;

        private string _sessionId;
        private bool _isLoggedIn;

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            private set { _isLoggedIn = value; }
        }

        public static string ClientName { get; } = "Sogeti Migrations Assistant";

        public SalesforceDomainServices()
        {
            _partnerServiceAdapter = new SalesforcePartnerServiceAdapter();
            _toolingServiceAdapter = new SalesforceToolingServiceAdapter();
            _metadataServiceAdapter = new SalesforceMetadataServiceAdapter();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();
        }


        private void SaveUserCredentials(string username, string password, string securityToken,
            SalesforceEnvironmentType environment, string domain)
        {
            _userName = username;
            _password = password;
            _token = securityToken;
            _environmentType = environment;
            _url = domain;
        }

        public bool Login(string username, string password, string environmentUrl = null, string securityToken = null,
            SalesforceEnvironmentType environment = SalesforceEnvironmentType.CustomDomain)
        {
            string str = environment == SalesforceEnvironmentType.CustomDomain
                ? environmentUrl
                : GetEnvironment(environment);
            this.SaveUserCredentials(username, password, securityToken, environment, str);
            string password1 = password + (securityToken ?? string.Empty);

            LoginResult loginResult;
            try
            {
                _partnerServiceAdapter.SetupClientId(ClientName);
                loginResult = _partnerServiceAdapter.Login(username, password1, str);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }

            if (loginResult == null || loginResult.passwordExpired)
                return false;

            _sessionId = loginResult.sessionId;
            IsLoggedIn = true;

            SetupToolingAdapter(loginResult.serverUrl);
            SetupMetadataAdapter(loginResult.metadataServerUrl);
            SetupPartneraAdapter(loginResult.serverUrl);
            return true;
        }

        private string GetEnvironment(SalesforceEnvironmentType environmentType)
        {
            switch (environmentType)
            {
                case SalesforceEnvironmentType.Sandbox:
                    return "test";
                default:
                    return "login";
            }
        }

        private void SetupToolingAdapter(string serverUrl)
        {
            _toolingServiceAdapter.Url = serverUrl.Replace("/u/", "/T/");
            _toolingServiceAdapter.SessionHeader =
                new Salesforce.Migrations.Assistant.Library.Tooling.SforceService.SessionHeader {sessionId = _sessionId};

            _toolingServiceAdapter.SetupClientId(ClientName);
        }

        private void SetupMetadataAdapter(string serverUrl)
        {
            _metadataServiceAdapter.Url = serverUrl.Replace("/u/", "/m/");
            _metadataServiceAdapter.SessionHeader =
                new Salesforce.Migrations.Assistant.Library.MetaDataService.SessionHeader {sessionId = _sessionId};
            _metadataServiceAdapter.SetupClientId(ClientName);
        }

        private void SetupPartneraAdapter(string serverUrl)
        {
            _partnerServiceAdapter.Url = serverUrl;
            _partnerServiceAdapter.SessionHeader =
                new Salesforce.Migrations.Assistant.Library.Partner.SforceService.SessionHeader {sessionId = _sessionId};
            _partnerServiceAdapter.SetupClientId(ClientName);
        }


        /*
        / Code that creates an instance of the MetadataService with a valid session.
        MetadataService metadataService = ...;

        // Get the bytes for the testing zip file from the file system
        byte[] zipFile = System.IO.File.ReadAllBytes(@"C:\path to sample\package\deleteClass.zip");
        DeployOptions deployOptions = new DeployOptions() { rollbackOnError = true, singlePackage = true };
        AsyncResult ar = metadataService.deploy(zipFile, deployOptions);

        // More code that calls checkStatus using the AsyncResult.Id until done

        // If the AsyncResult.state isn't Error get the DeployResult with checkDeployStatus()

    */



        public string RunDeployment(byte[] zipFile, DeployOptions deployOptions)
        {
            var id = _metadataServiceAdapter.Deploy(zipFile, deployOptions).id;
            return id;
        }

        public double ApiVersion { get; }

        public string DownloadLogFile(string id)
        {
            throw new NotImplementedException();
        }

        public OperationResult CreateNewSalesForceFile(SalesforceFileProxy entity)
        {
            throw new NotImplementedException();
        }

        public SalesforceFileProxy QueryApexFileByName(string fileName, string type)
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

        private T SessionExpirationWrapper<T>(Func<T> adapterRequest)
        {
            try
            {
                return adapterRequest();
            }
            catch (SoapException ex)
            {
                if ((ex.Message.Contains("INVALID_SESSION_ID:") ||
                     ex.Message.Contains("UNKNOWN_EXCEPTION: Destination URL not reset.")) && this.ReLogin())
                    return adapterRequest();
                Log.Information(((Exception) ex).Message);
                throw;
            }
        }

        private bool ReLogin()
        {
            return this.Login(this._userName, this._password, this._url, this._token, this._environmentType);
        }

        private string CreateMetadataContainer()
        {
            return SessionExpirationWrapper(() => _toolingServiceAdapter.Create(new sObject[1]
            {
                (sObject) new MetadataContainer()
                {
                    Name = Guid.NewGuid().ToString().Substring(0, 32)
                }
            }))[0].id;
        }

        public OperationResult BuildFiles(IEnumerable<ICompilableItem> changedFiles)
        {
            OperationResult operationResult = new OperationResult();
            string metadataContainer = this.CreateMetadataContainer();
            List<sObject> objects = new List<sObject>();
            var compilableItems = changedFiles as IList<ICompilableItem> ?? changedFiles.ToList();

            objects.AddRange(GetApexMembers<ApexClassMember>(compilableItems, metadataContainer));

            return operationResult;

            //objects.AddRange((IEnumerable<sObject>) GetApexComponentMembers(compilableItems, metadataContainer));
            //objects.AddRange((IEnumerable<sObject>) GetApexPageMembers(compilableItems, metadataContainer));
            //objects.AddRange((IEnumerable<sObject>) GetApexTriggerMembers(compilableItems, metadataContainer));
            //List<WelkinSuite.Salesforce.Gateway.Tooling.SaveResult> list1 =
            //    new List<WelkinSuite.Salesforce.Gateway.Tooling.SaveResult>();
            //if (objects.Count > 0)
            //    list1.AddRange(
            //        (IEnumerable<WelkinSuite.Salesforce.Gateway.Tooling.SaveResult>)
            //            this.SessionExpirationWrapper<WelkinSuite.Salesforce.Gateway.Tooling.SaveResult[]>(
            //                (Func<WelkinSuite.Salesforce.Gateway.Tooling.SaveResult[]>)
            //                    (() => this._toolingGateway.Create(objects.ToArray()))));
            //if (
            //    Enumerable.Any<WelkinSuite.Salesforce.Gateway.Tooling.SaveResult>(
            //        (IEnumerable<WelkinSuite.Salesforce.Gateway.Tooling.SaveResult>) list1,
            //        (Func<WelkinSuite.Salesforce.Gateway.Tooling.SaveResult, bool>) (o => o.errors != null)))
            //{
            //    foreach (WelkinSuite.Salesforce.Gateway.Tooling.SaveResult saveResult in list1)
            //    {
            //        if (saveResult.errors != null)
            //        {
            //            List<ErrorDescriptor> list2 =
            //                Enumerable.ToList<ErrorDescriptor>(
            //                    Enumerable.Select<WelkinSuite.Salesforce.Gateway.Tooling.Error, ErrorDescriptor>(
            //                        (IEnumerable<WelkinSuite.Salesforce.Gateway.Tooling.Error>) saveResult.errors,
            //                        (Func<WelkinSuite.Salesforce.Gateway.Tooling.Error, ErrorDescriptor>)
            //                            (e => new ErrorDescriptor()
            //                            {
            //                                Id = e.statusCode.ToString(),
            //                                Name = e.message,
            //                                Problem = e.fields == null ? string.Empty : string.Join(";", e.fields)
            //                            })));
            //            operationResult.Errors.AddRange((IEnumerable<ErrorDescriptor>) list2);
            //        }
            //  }
            // }
        }

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

        public List<SalesforceFileProxy> DownloadAllFilesSynchronously(PackageEntity package,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public RetrieveRequest[] ConvertPackageToRequests(PackageEntity package)
        {
            throw new NotImplementedException();
        }

        public RetrieveResult CheckRetrieveResult(string id)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, DateTime> GetLastModifiedOnServerForMetadataSet(List<MetadataType> metadataTypes,
            IEnumerable<string> workflowNames)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, DateTime> GetLastModifiedOnServer(
            Dictionary<MetadataType, IEnumerable<string>> filesToCheck)
        {
            throw new NotImplementedException();
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

        private static IEnumerable<sObject> GetApexClassMembers(IEnumerable<ICompilableItem> files, string metadataContainerId)
        {
            return files.Where(s => s.Type == MetadataType.ApexClass).Select(s => new ApexClassMember()
            {
                ContentEntityId = s.Id,
                MetadataContainerId = metadataContainerId,
                Body = s.FileBody
            }).ToArray();
        }

        private static IEnumerable<sObject> GetApexTriggerMembers(IEnumerable<ICompilableItem> files, string metadataContainerId)
        {
            return files.Where(s => s.Type == MetadataType.ApexTrigger).Select(s => new ApexTriggerMember()
            {
                ContentEntityId = s.Id,
                MetadataContainerId = metadataContainerId,
                Body = s.FileBody
            }).ToArray();
        }

        private static sObject Cast<T>(object typeHolder)
        {
            return (sObject)typeHolder;
        }

        private static IEnumerable<sObject> GetApexMembers<T>(IEnumerable<ICompilableItem> files, string metadataContainerId) where T : new()
        {
            return files.Where(s => s.Type == MetadataType.ApexTrigger).Select(s => Cast<sObject>(new 
                    {
                        ContentEntityId = s.Id,
                        MetadataContainerId = metadataContainerId,
                        Body = s.FileBody
                    }.MapTo<T>())).ToArray();
        }
    }
}
