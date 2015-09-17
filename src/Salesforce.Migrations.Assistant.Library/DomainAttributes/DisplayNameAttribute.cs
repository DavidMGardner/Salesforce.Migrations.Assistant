using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.DomainAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public DisplayNameAttribute(string displayName)
        {
            this.DisplayName = displayName;
        }
    }
}
