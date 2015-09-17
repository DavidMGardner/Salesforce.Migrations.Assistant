using System;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class LogDescriptor
    {
        public string Id { get; set; }
        public string Application { get; set; }
        public int? DurationMilliseconds { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Location { get; set; }
        public int? LogLength { get; set; }
        public string LogUser { get; set; }
        public string Operation { get; set; }
        public string Request { get; set; }
        public DateTime? StartTime { get; set; }
        public string Status { get; set; }
    }
}