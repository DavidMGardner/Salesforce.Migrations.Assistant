using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public interface IEntity
    {
        string Id { get; set; }
    }
}
