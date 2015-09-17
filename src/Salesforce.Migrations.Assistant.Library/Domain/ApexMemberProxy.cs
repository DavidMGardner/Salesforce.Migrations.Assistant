using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Salesforce.Migrations.Assistant.Library.Tooling.SforceService;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class ApexMemberProxy
    {
        public string ContentEntityId { get; set; }
        public string MetadataContainerId { get; set; }
        public string Body { get; set; }
    }
}
