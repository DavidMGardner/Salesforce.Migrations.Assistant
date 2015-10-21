namespace Salesforce.Migrations.Assistant.Library.Domain
{
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
        //string IDeployableItem.Name => this.FileNameWithoutExtension;

        public int ImageIndex => 62;
        public MetadataType Type => MetadataType.StaticResource;

        public string SalesforceMetadata { get; set; }


    }
}