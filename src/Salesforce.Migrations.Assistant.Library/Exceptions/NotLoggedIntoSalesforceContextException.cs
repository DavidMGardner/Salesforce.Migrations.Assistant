using System;

namespace Salesforce.Migrations.Assistant.Library.Exceptions
{
    internal class NotLoggedIntoSalesforceContextException : Exception
    {
        private const string message =
            "The Current SalesforceContext has not successfully logged in, have you checked the application configuration credentials?";

        public NotLoggedIntoSalesforceContextException()
            : base(message)
        {
        }
    }
}