using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public interface ICompilableItem
    {
        string Id { get; }
        MetadataType Type { get; }
        string FileBody { get; }
    }
}
