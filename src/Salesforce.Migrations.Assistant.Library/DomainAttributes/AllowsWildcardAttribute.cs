using System;

namespace Salesforce.Migrations.Assistant.Library.DomainAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AllowsWildcardAttribute : Attribute
    {
        public bool IsSupported { get; set; }

        public AllowsWildcardAttribute(bool isSupported = true)
        {
            this.IsSupported = isSupported;
        }
    }
}