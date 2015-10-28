using System.Collections.Generic;
using System.IO;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public static class MetadataTypeHelper
    {
        private static readonly Dictionary<string, MetadataType> MapMetadataTypes = new Dictionary<string, MetadataType>
        {
            { ".cls", MetadataType.ApexClass },
            { ".page", MetadataType.ApexPage },
            { ".component", MetadataType.ApexComponent },
            { ".trigger", MetadataType.ApexTrigger },
            { ".app", MetadataType.CustomApplication },
            { ".object", MetadataType.CustomObject },
            { ".tab", MetadataType.CustomTab },
            { ".resource", MetadataType.StaticResource },
            { ".workflow", MetadataType.Workflow },
            { ".remoteSite", MetadataType.RemoteSiteSetting },
            { ".pagelayout", MetadataType.Layout}
        };

        static public MetadataType GetType(string filename)
        {
            string ext = Path.GetExtension(filename);

            if (ext != null && MapMetadataTypes.ContainsKey(ext))
            {
                return MapMetadataTypes[ext];
            }

            return MetadataType.Unknown;
        }
    }

    public class SalesForceLocalFileDeployableItem : IDeployableItem
    {
        public string FileName { get; set; }

        public MetadataType Type => MetadataTypeHelper.GetType(FileName);
        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FileName);
        public string Name => Path.GetFileName(FileName);

        public bool AddToPackage { get; set; }
        public byte[] FileBody { get; set; }
    }

    public class StaticResourceDeployableItem : IDeployableItem
    {
        public byte[] FileBody { get; set; }
        public string FileName { get; set; }
        public string FileNameWithoutExtension { get; set; }
        public bool AddToPackage { get; set; }

        public StaticResourceDeployableItem()
        {
            AddToPackage = true;
        }

        string IDeployableItem.Name => this.FileNameWithoutExtension + ".resource";
        
        public int ImageIndex => 62;
        public MetadataType Type => MetadataType.StaticResource;

        public string SalesforceMetadata { get; set; }


    }
}