using System;

namespace Salesforce.Migrations.Assistant.Library.DomainAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FileExtensionAttribute : Attribute
    {
        public string Extension { get; set; }

        public FileExtensionAttribute(string extension)
        {
            this.Extension = extension;
        }
    }
}