using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Exceptions
{
    public class AllowsWildcardAttributeMissingException : Exception
    {
        public AllowsWildcardAttributeMissingException(string message)
            : base(message)
        {
        }
    }
}
