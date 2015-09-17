using System;

namespace Salesforce.Migrations.Assistant.Library.DomainAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SupportedAttribute : Attribute
    {
        public bool IsSupported { get; set; }

        public SupportedAttribute(bool isSupported = true)
        {
            this.IsSupported = isSupported;
        }
    }
}