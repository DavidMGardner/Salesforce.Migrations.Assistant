using System.IO;
using System.Xml.Linq;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class StaticResourceContainerNode : SalesforceFileNode, ISalesforceMetadataContainer, IDeployableItem, IStaticResource
    {
        public StaticResourceContainerNode(PackageEntity root) : base(root)
        {
            
        }

        string IDeployableItem.Name => this.FileNameWithoutExtension + ".resource";

        public int ImageIndex => 62;

        public override MetadataType Type
        {
            get
            {
                return MetadataType.StaticResource;
            }
        }

        public string SalesforceMetadata { get; set; }

        
 
        public string GetSalesforceMetadataValue(string metadataKey)
        {
            XNamespace xnamespace = (XNamespace)"http://soap.sforce.com/2006/04/metadata";
            XElement xelement = XElement.Load((TextReader)new StringReader(this.SalesforceMetadata));
            return xelement.Element(xnamespace + metadataKey) != null ? xelement.Element(xnamespace + metadataKey).Value : string.Empty;
        }
    }
}