using System;
using System.Collections.Generic;
using System.Linq;
using Moo.Extenders;
using Salesforce.Migrations.Assistant.Library.MetaDataService;

namespace Salesforce.Migrations.Assistant.Library.Services
{
    public class SalesforceMetadataServiceAdapter : ISalesforceMetadataServiceAdapter, IDisposable
    {
        private readonly MetadataService _service;

        public SalesforceMetadataServiceAdapter(MetadataService service)
        {
            _service = service;
        }

        public SalesforceMetadataServiceAdapter() : this (new MetadataService())
        {
            
        }

        public void Dispose()
        {
            _service.Dispose();
        }

        public string Url
        {
            get { return this._service.Url; }
            set { this._service.Url = value; }
        }

        public SessionHeader SessionHeader {
            get { return _service.SessionHeaderValue; }
            set { _service.SessionHeaderValue = value; }
        }


        public AsyncResult Retrieve(RetrieveRequest retrieveRequest)
        {
            if (retrieveRequest.unpackaged != null)
                string.Format("apiAccessLevel={0}, apiAccessLevelSpecified={1}, fullName={2}, namespacePrefix={3}, uninstallClass={4}, version={5}, types={6}", 
                    retrieveRequest.unpackaged.apiAccessLevel as object, 
                    retrieveRequest.unpackaged.apiAccessLevelSpecified, 
                    retrieveRequest.unpackaged.fullName, 
                    retrieveRequest.unpackaged.namespacePrefix as object, 
                    retrieveRequest.unpackaged.uninstallClass as object, 
                    retrieveRequest.unpackaged.version as object, 
                    string.Join("|", retrieveRequest.unpackaged.types.Select(type => type.name + ":" + string.Join(",", type.members))) as object);

            return _service.retrieve(retrieveRequest);
        }

        public RetrieveResult CheckRetrieveStatus(string asyncProcessId, bool includeZip)
        {
            RetrieveResult retrieveResult = null;

            retrieveResult = _service.checkRetrieveStatus(asyncProcessId, includeZip);

            return retrieveResult;
        }

        public MetaDataService.DeployResult CheckDeployResult(string asyncProcessId)
        {
            var result = _service.checkDeployStatus(asyncProcessId, true);

            return result;
        }

        public AsyncResult Deploy(byte[] zipFile, Domain.DeployOptions options)
        {
            DeployOptions optionMap = new DeployOptions
            {
                performRetrieve = options.PerformeRetrive,
                rollbackOnError = options.RollbackOnError,
                ignoreWarnings = options.IgnoreWarnings,
                checkOnly = options.CheckOnly,
                runTests = options.TestsForRun
            };

            return _service.deploy(zipFile, optionMap);
        }

        public FileProperties[] ListMetadata(ListMetadataQuery[] queries, double asOfVersion)
        {
            if (queries == null || queries.Length == 0)
                throw new ArgumentNullException(nameof(queries));

            FileProperties[] filePropertiesArray = _service.listMetadata(queries, asOfVersion);

            if (filePropertiesArray == null || filePropertiesArray.Length == 0)
                filePropertiesArray = new FileProperties[0];

            return filePropertiesArray;
        }

        public DescribeMetadataResult Describe()
        {
            return _service.describeMetadata(30.0);
        }

        public void SetupClientId(string clientId)
        {
            CallOptions callOptions = _service.CallOptionsValue ?? new CallOptions();
            callOptions.client = clientId;
            this._service.CallOptionsValue = callOptions;
        }

        //private bool IsRequestCompleted(string asyncProcessId)
        //{
        //    AsyncResult[] asyncResultArray = _service.checkRetrieveStatus(new { asyncProcessId });

        //    if (asyncResultArray == null && asyncResultArray.Length != 1)
        //        return false;
        //    return asyncResultArray[0].done;
        //}
    }
}