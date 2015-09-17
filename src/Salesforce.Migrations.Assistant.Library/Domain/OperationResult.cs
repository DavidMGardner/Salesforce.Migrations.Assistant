using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class OperationResult
    {
        [JsonProperty(PropertyName = "CompilerErrors")]
        public List<ErrorDescriptor> Errors { get; set; }

        public bool IsSuccessfull => Errors.Count == 0;

        public OperationResult()
        {
            this.Errors = new List<ErrorDescriptor>();
        }
    }
}
