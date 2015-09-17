using System;
using Salesforce.Migrations.Assistant.Library.Domain;

namespace Salesforce.Migrations.Assistant.Library.DomainAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MetadataSubTypeAttribute : Attribute
    {
        public MetadataType Parent { get; set; }

        public MetadataSubTypeAttribute(MetadataType parent)
        {
            this.Parent = parent;
        }
    }
}