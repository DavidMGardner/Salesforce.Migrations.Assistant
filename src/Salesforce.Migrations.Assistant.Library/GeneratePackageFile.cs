using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace Salesforce.Migrations.Assistant.Library
{
    public static class GeneratePackageFile
    {

        public static XmlDocument GenerateOutputXml(IEnumerable<Change> changes)
        {
            IEnumerable<SalesForceChange> processedChanges = ProcesChanges(changes);

            XmlOutput xo = new XmlOutput()
                .XmlDeclaration()
                .Node("package").Attribute("xmlns", "http://soap.sforce.com/2006/04/metadata").Within();

            foreach (var salesForceChange in processedChanges)
            {
                xo.Node("types").Within()
                       .Node("members").InnerText(salesForceChange.FileName)
                       .Node("name").InnerText(salesForceChange.SalesForceType.ToString())
                       .EndWithin();
            }

            xo.EndWithin()
            .Node("version").InnerText("34.0");

            return xo.GetXmlDocument();
        }

        private static IEnumerable<SalesForceChange> ProcesChanges(IEnumerable<Change> changes)
        {
            return changes.Select(ChangeFactory.BuildSalesForceType).Where(sfchange => sfchange != null).ToList();
        }
    }

    internal class ChangeFactory
    {
        private static readonly Dictionary<string, SalesForceType> MapSalesForceTypes = new Dictionary<string, SalesForceType>
        {
            { ".cls", SalesForceType.ApexClass },
            { ".page", SalesForceType.ApexPage },
            { ".component", SalesForceType.ApexComponent },
            { ".trigger", SalesForceType.ApexTrigger },
            { ".app", SalesForceType.CustomApplication },
            { ".object", SalesForceType.CustomObject },
            { ".tab", SalesForceType.CustomTab },
            { ".resource", SalesForceType.StaticResource },
            { ".workflow", SalesForceType.Workflow },
            { ".remoteSite", SalesForceType.RemoteSiteSettings },
            { ".pagelayout", SalesForceType.Layout}
        };

        public static SalesForceChange BuildSalesForceType(Change change)
        {
            string ext = Path.GetExtension(change.Path);
            string filename = Path.GetFileName(change.Path);

            SalesForceType @type = SalesForceType.Unknown;
            if (ext != null && MapSalesForceTypes.TryGetValue(ext, out @type))
            {
                return new SalesForceChange
                {
                    FileName = filename,
                    ChangeType = change.Status,
                    SalesForceType = @type
                };
            }

            return null;
        }
    }

    public enum SalesForceType
    {
        ApexClass,
        ApexPage,
        ApexComponent,
        ApexTrigger,
        Workflow,
        StaticResource,
        CustomApplication,
        CustomLabels,
        CustomObject,
        CustomTab,
        RemoteSiteSettings,
        Layout,
        Unknown
    }

    public class SalesForceChange
    {
        public string FileName { get; set; }
        public SalesForceType SalesForceType { get; set; }
        public GitChangeKind ChangeType { get; set; }
    }
}

