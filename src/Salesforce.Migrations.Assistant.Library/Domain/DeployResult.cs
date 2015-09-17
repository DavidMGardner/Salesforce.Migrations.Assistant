using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class DeployResult
    {
        public OperationResult OperationResult { get; set; }
        public List<DeployTestResult> TestResults { get; set; }
        public bool IsDone { get; set; }
        public int NumberTestsTotal { get; set; }
        public int NumberTestsCompleted { get; set; }
        public int NumberTestErrors { get; set; }
    }
}
