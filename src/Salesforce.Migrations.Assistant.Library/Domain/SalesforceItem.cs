using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class SalesforceItem : ISalesforceItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
