namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public interface IDeployableItem
    {
        string Name { get; }
        byte[] FileBody { get; }
        MetadataType Type { get; }
    }
}