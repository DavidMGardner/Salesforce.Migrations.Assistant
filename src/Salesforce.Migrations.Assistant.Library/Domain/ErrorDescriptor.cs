using Salesforce.Migrations.Assistant.Library.Tooling.SforceService;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class ErrorDescriptor
    {
        public string Extent { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Line { get; set; }
        public string Problem { get; set; }
        public DeployProblemType ProblemType { get; set; }
    }
}