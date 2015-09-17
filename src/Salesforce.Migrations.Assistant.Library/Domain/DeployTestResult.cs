namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class DeployTestResult   
    {
        public string ClassId { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public DeploymentTestResultType TestResultType { get; set; }
        public string Namespace { get; set; }
    }
}