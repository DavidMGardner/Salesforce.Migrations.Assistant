using System;
using System.Collections.Generic;

namespace Salesforce.Migrations.Assistant.Library.Configuration
{
    public class SalesforceMigrationsProject
    {
        public string Version { get; set; }
        public DateTime? LastRun { get; set; }
        
        public List<SalesForceEnvionment> Environments;

        public List<string> PushEnvironments { get; set; }
        public List<string> PullEnvironments { get; set; }
        public List<string> ContextTypes;
    }
}