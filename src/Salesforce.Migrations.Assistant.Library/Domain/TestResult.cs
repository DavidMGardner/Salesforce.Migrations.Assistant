using Salesforce.Migrations.Assistant.Library.DomainAttributes;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public sealed class TestResult
    {
        public string TestClassId { get; set; }
        public string ApexLogId { get; set; }
        public string ParentJobId { get; set; }
        public string Message { get; set; }
        public string TestMethodName { get; set; }
        public TestResultStatus Status { get; set; }
        public string TestQueueItemId { get; set; }
        public string StackTrace { get; set; }
    }

    public enum TestResultStatus
    {
        [DisplayName("Passed")]
        Pass,
        [DisplayName("Failed")]
        Fail,
        [DisplayName("Compilation Failed")]
        CompileFail,
        [DisplayName("Skipped")]
        Skip,
        [DisplayName("")]
        EmptyStatus,
    }
}
