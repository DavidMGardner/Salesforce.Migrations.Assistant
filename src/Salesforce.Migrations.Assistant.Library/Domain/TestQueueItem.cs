using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public sealed class TestQueueItem
    {
        public string TestQueueItemId { get; set; }
        public string ParentJobId { get; set; }
        public string TestClassId { get; set; }
        public string TestClassName { get; set; }
        public string ExtendedStatus { get; set; }
        public TestQueueItemStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public enum TestQueueItemStatus
    {
        Holding,
        Queued,
        Preparing,
        Processing,
        Aborted,
        Completed,
        Failed,
    }
}
