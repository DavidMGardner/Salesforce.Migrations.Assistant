using System;

namespace Salesforce.Migrations.Assistant.Library.DomainAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SalesforceDirectory : Attribute
    {
        public string SfDirectory { get; set; }

        public SalesforceDirectory(string sfDirectory)
        {
            this.SfDirectory = sfDirectory;
        }
    }
}