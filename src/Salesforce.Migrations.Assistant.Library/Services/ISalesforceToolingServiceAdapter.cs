using Salesforce.Migrations.Assistant.Library.Tooling.SforceService;

namespace Salesforce.Migrations.Assistant.Library.Services
{
    public interface ISalesforceToolingServiceAdapter 
    {
        string Url { get; set; }

        SessionHeader SessionHeader { get; set; }
        ExecuteAnonymousResult ExecuteAnonymous(string apexToExecute);
        SaveResult[] Create(sObject[] sObjects);
        sObject[] Retrieve(string select, string type, string[] ids);
        DescribeGlobalResult DescribeGlobal();
        DescribeSObjectResult DescribeObject(string type);
        QueryResult Query(string query);
        SaveResult[] Update(sObject[] sObjects);
        DeleteResult[] Delete(string[] ids);

        string GetLogBody(string id);
        SaveResult[] StartLogging(string id);
        DeleteResult[] StopLogging(string id);

        void SetupClientId(string clientId);
    }
}