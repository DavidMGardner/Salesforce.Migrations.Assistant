using System;
using System.Collections.Generic;

namespace Salesforce.Migrations.Assistant.Library.Configuration
{
    public class SalesforceMigrationsProject
    {
        public string Version { get; set; }
        public DateTime? LastRun { get; set; }

        public List<SalesForceEnvionment> Environments;
        public List<string> PullEnvironments;
        public List<string> ContextTypes;
    }
}