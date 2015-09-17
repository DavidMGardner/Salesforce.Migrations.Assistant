namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public interface IStaticResource
    {
        string Id { get; }
        string Name { get; }
        byte[] FileBody { get; }
        MetadataType Type { get; }
    }
}