using System;

namespace Salesforce.Migrations.Assistant.Library.DomainAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FolderAttribute : Attribute
    {
        public string Name { get; set; }

        public FolderAttribute(string name)
        {
            this.Name = name;
        }
    }
}