namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public interface ISalesforceMetadataContainer
    {
        string SalesforceMetadata { get; set; }
        string GetSalesforceMetadataValue(string metadataKey);
    }
}