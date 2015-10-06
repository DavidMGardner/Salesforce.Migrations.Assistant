using Salesforce.Migrations.Assistant.Library.Domain;

namespace Salesforce.Migrations.Assistant.Library.Configuration
{
    public class Credential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public SalesforceEnvironmentType EnvironmentType { get; set; }
        public string Url { get; set; }
    }
}