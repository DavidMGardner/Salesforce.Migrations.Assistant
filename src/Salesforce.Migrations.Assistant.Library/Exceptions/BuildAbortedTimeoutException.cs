using System;

namespace Salesforce.Migrations.Assistant.Library.Exceptions
{
    public class BuildAbortedTimeoutException : Exception
    {
        public BuildAbortedTimeoutException(string message) : base(message) { }
    }
}