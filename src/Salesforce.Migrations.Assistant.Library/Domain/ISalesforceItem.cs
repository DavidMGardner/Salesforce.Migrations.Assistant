namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public interface ISalesforceItem
    {
        string Id { get; set; }
        string Name { get; }
    }
}