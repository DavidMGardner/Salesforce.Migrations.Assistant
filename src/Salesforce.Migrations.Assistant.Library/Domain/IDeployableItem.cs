namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public interface IDeployableItem
    {
        string Name { get; }
        byte[] FileBody { get; }
        MetadataType Type { get; }
        bool AddToPackage { get; set; }
        string FileNameWithoutExtension { get; }
    }
}