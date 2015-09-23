using System;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class MetadataSubTypeParentTypeIsNotPresentException : Exception
    {
        public MetadataSubTypeParentTypeIsNotPresentException(string message) : base(message) { }
    }
}