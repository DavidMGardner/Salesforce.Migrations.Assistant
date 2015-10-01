using System;

namespace Salesforce.Migrations.Assistant.Library.Exceptions
{
    public class TaskTimeoutException : Exception
    {
        public TaskTimeoutException(string message) : base(message) { }
    }
}