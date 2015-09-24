using Salesforce.Migrations.Assistant.Library.Domain;

static public class PackageEntityExtensions
{
    static public PackageEntity BuildQueryAllCommonTypes(this PackageEntity entity)
    {
        var pe = new PackageEntity()
            .SetVersion("29.0")
            .AddGlobal("ApexClass")
            .AddGlobal("ApexPage")
            .AddGlobal("ApexComponent")
            .AddGlobal("ApexTrigger")
            .AddGlobal("ApprovalProcess")
            .AddGlobal("CustomLabels")
            .AddGlobal("StaticResource")
            .AddGlobal("Layouts")
            .AddGlobal("CustomObject")
            .AddGlobal("Workflow");

        return pe;
    }
}