using System;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class MetadataTypeIsNotFoundByExtension : Exception
    {
        public MetadataTypeIsNotFoundByExtension(string fileExtension) : base(fileExtension) { }
    }
}