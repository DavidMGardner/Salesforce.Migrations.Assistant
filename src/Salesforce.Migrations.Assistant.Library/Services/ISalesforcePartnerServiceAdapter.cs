using Salesforce.Migrations.Assistant.Library.Partner.SforceService;

namespace Salesforce.Migrations.Assistant.Library.Services
{
    public interface ISalesforcePartnerServiceAdapter
    {
        string Url { get; set; }
        SessionHeader SessionHeader { get; set; }
        DescribeGlobalResult DescribeGlobal();
        GetUserInfoResult GetUserInfo();
        QueryResult Query(string query);
        QueryResult QueryMore(string queryLocator);
        LoginResult Login(string username, string password, string environmentUrl);
        void SetupClientId(string clientId);
    }
}