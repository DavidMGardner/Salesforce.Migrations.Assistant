namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class DeployOptions
    {
        public bool PerformeRetrive { get; set; }
        public bool RollbackOnError { get; set; }
        public bool IgnoreWarnings { get; set; }
        public bool CheckOnly { get; set; }
        public string[] TestsForRun { get; set; }
    }
}