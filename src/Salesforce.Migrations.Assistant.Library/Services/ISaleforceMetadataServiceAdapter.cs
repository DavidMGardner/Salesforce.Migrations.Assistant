using System;
using Salesforce.Migrations.Assistant.Library.MetaDataService;

namespace Salesforce.Migrations.Assistant.Library.Services
{
    public interface ISalesforceMetadataServiceAdapter
    {
        string Url { get; set; }
        SessionHeader SessionHeader { get; set; }
        AsyncResult Retrieve(RetrieveRequest retrieveRequest);
        RetrieveResult CheckRetrieveStatus(string asyncProcessId, bool includeZip);
        MetaDataService.DeployResult CheckDeployResult(string asyncProcessId);
        AsyncResult Deploy(byte[] zipFile, Domain.DeployOptions options);
        FileProperties[] ListMetadata(ListMetadataQuery[] queries, double asOfVersion);
        DescribeMetadataResult Describe();
        void SetupClientId(string clientId);
    }
}