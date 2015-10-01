using System;

namespace Salesforce.Migrations.Assistant.Library.Exceptions
{
    public class AttributeMissingException : Exception
    {
        public AttributeMissingException(string message)
            : base(message)
        {
        }
    }
}