﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Salesforce.Migrations.Assistant.Library.DomainAttributes;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public enum MetadataType
    {
        [AllowsWildcard(true)]
        AccountSettings,
        [AllowsWildcard(true)]
        ActionLinkGroupTemplate,
        [AllowsWildcard(false)]
        ActionOverride,
        [AllowsWildcard(true)]
        ActivitiesSettings,
        [AllowsWildcard(true)]
        AddressSettings,
        [AllowsWildcard(false)]
        AnalyticSnapshot,
        [IncludedByDefault, AllowsWildcard(true), DisplayName("Apex Class"), SalesforceDirectory("Classes"), FileExtension(".cls"), Supported(true)]
        ApexClass,
        [DisplayName("Apex Component"), Supported(true), SalesforceDirectory("Components"), FileExtension(".component"), AllowsWildcard(true), IncludedByDefault]
        ApexComponent,
        [FileExtension(".page"), SalesforceDirectory("Pages"), Supported(true), AllowsWildcard(true), DisplayName("Apex Page"), IncludedByDefault]
        ApexPage,
        [AllowsWildcard(true), SalesforceDirectory("Triggers"), DisplayName("Apex Trigger"), FileExtension(".trigger"), Supported(true), IncludedByDefault]
        ApexTrigger,
        [AllowsWildcard(true)]
        AppMenu,
        [AllowsWildcard(true), SalesforceDirectory("ApprovalProcesses"), DisplayName("Approval Process"), FileExtension(".approvalProcess"), Folder("approvalProcesses"), Supported(true)]
        ApprovalProcess,
        [AllowsWildcard(true)]
        ArticleType,
        [AllowsWildcard(true)]
        AssignmentRules,
        [AllowsWildcard(true)]
        AuthProvider,
        [AllowsWildcard(true)]
        AuraDefinitionBundle,
        [AllowsWildcard(true)]
        AutoResponseRules,
        [AllowsWildcard(true)]
        BaseSharingRule,
        [AllowsWildcard(true)]
        BusinessHoursSettings,
        [AllowsWildcard(false)]
        BusinessProcess,
        [AllowsWildcard(true)]
        CallCenter,
        [AllowsWildcard(true)]
        CaseSettings,
        [AllowsWildcard(true)]
        ChatterAnswersSettings,
        [AllowsWildcard(true)]
        CompanySettings,
        [AllowsWildcard(true)]
        Community,
        [AllowsWildcard(true)]
        CompactLayout,
        [AllowsWildcard(true)]
        ConnectedApp,
        [AllowsWildcard(true)]
        ContractSettings,
        [AllowsWildcard(true)]
        CorsWhitelistOrigin,
        [AllowsWildcard(true)]
        CriteriaBasedSharingRule,
        [AllowsWildcard(true)]
        CustomApplication,
        [AllowsWildcard(true)]
        CustomApplicationComponent,
        [AllowsWildcard(false), MetadataSubType(MetadataType.CustomObject)]
        CustomField,
        [MetadataSubType(MetadataType.CustomLabels), AllowsWildcard(false)]
        CustomLabel,
        [DisplayName("Custom Labels"), AllowsWildcard(true), SalesforceDirectory("Labels"), Supported(true), FileExtension(".labels")]
        CustomLabels,
        [FileExtension(".object"), AllowsWildcard(true), DisplayName("Object"), SalesforceDirectory("Objects"), Supported(true), IncludedByDefault]
        CustomObject,
        [AllowsWildcard(true)]
        CustomObjectTranslation,
        [AllowsWildcard(true)]
        CustomPageWebLink,
        [AllowsWildcard(true)]
        CustomPermission,
        [Supported(true), FileExtension(".site"), Folder("sites"), AllowsWildcard(true), SalesforceDirectory("Sites"), DisplayName("Custom Site")]
        CustomSite,
        [AllowsWildcard(true)]
        CustomTab,
        [AllowsWildcard(false)]
        Dashboard,
        [AllowsWildcard(true)]
        DataCategoryGroup,
        [AllowsWildcard(false)]
        Document,
        [DisplayName("Email Template"), AllowsWildcard(false), SalesforceDirectory("EmailTemplates"), FileExtension(".email"), Folder("email")]
        EmailTemplate,
        [AllowsWildcard(true)]
        EntitlementProcess,
        [AllowsWildcard(true)]
        EntitlementSettings,
        [AllowsWildcard(true)]
        EntitlementTemplate,
        [AllowsWildcard(true)]
        ExternalDataSource,
        [AllowsWildcard(true)]
        FieldSet,
        [AllowsWildcard(true)]
        FlexiPage,
        [SalesforceDirectory("Flows"), AllowsWildcard(true), FileExtension(".flow"), DisplayName("Flow"), Folder("Flows")]
        Flow,
        [AllowsWildcard(false)]
        Folder,
        [AllowsWildcard(false)]
        FolderShare,
        [AllowsWildcard(true)]
        ForecastingSettings,
        [AllowsWildcard(true)]
        Group,
        [AllowsWildcard(true)]
        HomePageComponent,
        [AllowsWildcard(true)]
        HomePageLayout,
        [AllowsWildcard(true)]
        IdeasSettings,
        [AllowsWildcard(true)]
        InstalledPackage,
        [AllowsWildcard(true)]
        KnowledgeSettings,
        [AllowsWildcard(true), Folder("Layouts"), Supported(true), SalesforceDirectory("Layouts"), FileExtension(".layout"), DisplayName("Layout")]
        Layout,
        [AllowsWildcard(false)]
        Letterhead,
        [AllowsWildcard(false)]
        ListView,
        [AllowsWildcard(true)]
        LiveAgentSettings,
        [AllowsWildcard(true)]
        LiveChatAgentConfig,
        [AllowsWildcard(true)]
        LiveChatButton,
        [AllowsWildcard(true)]
        LiveChatDeployment,
        [AllowsWildcard(true)]
        ManagedTopics,
        [AllowsWildcard(true)]
        MatchingRule,
        [AllowsWildcard(true)]
        MilestoneType,
        [AllowsWildcard(true)]
        MobileSettings,
        [AllowsWildcard(true)]
        NamedCredential,
        [AllowsWildcard(false)]
        NamedFilter,
        [AllowsWildcard(true)]
        NameSettings,
        [AllowsWildcard(true)]
        Network,
        [AllowsWildcard(false)]
        OpportunitySettings,
        [AllowsWildcard(true)]
        OrderSettings,
        [AllowsWildcard(true)]
        OwnerSharingRule,
        [AllowsWildcard(false)]
        Package,
        [FileExtension(".permissionset"), DisplayName("Permission Set"), SalesforceDirectory("PermissionSets"), Folder("permissionsets"), Supported(true), AllowsWildcard(true)]
        PermissionSet,
        [AllowsWildcard(false)]
        Picklist,
        [AllowsWildcard(true)]
        Portal,
        [AllowsWildcard(true)]
        PostTemplate,
        [AllowsWildcard(false)]
        ProductSettings,
        [DisplayName("Profile"), Supported(true), Folder("profiles"), FileExtension(".profile"), AllowsWildcard(true), SalesforceDirectory("Profiles")]
        Profile,
        [AllowsWildcard(true)]
        Queue,
        [AllowsWildcard(true)]
        QuickAction,
        [AllowsWildcard(false)]
        QuoteSettings,
        [AllowsWildcard(true)]
        RecordType,
        [DisplayName("Remote Site Settings"), Folder("remoteSiteSettings"), FileExtension(".remoteSite"), Supported(true), AllowsWildcard(true), SalesforceDirectory("RemoteSiteSettings")]
        RemoteSiteSetting,
        [AllowsWildcard(false)]
        Report,
        [AllowsWildcard(true)]
        ReportType,
        [AllowsWildcard(true)]
        Role,
        [AllowsWildcard(true)]
        SamlSsoConfig,
        [AllowsWildcard(true)]
        Scontrol,
        [AllowsWildcard(false)]
        SearchLayouts,
        [AllowsWildcard(true)]
        SecuritySettings,
        [AllowsWildcard(false)]
        SharingBaseRule,
        [AllowsWildcard(false)]
        SharingReason,
        [AllowsWildcard(false)]
        SharingRecalculation,
        [AllowsWildcard(true)]
        SharingRules,
        [AllowsWildcard(true)]
        SharingSet,
        [AllowsWildcard(true)]
        SiteDotCom,
        [AllowsWildcard(true)]
        Skill,
        [Supported(true), SalesforceDirectory("StaticResources"), AllowsWildcard(true), IncludedByDefault, FileExtension(".resource"), DisplayName("Static Resource")]
        StaticResource,
        [AllowsWildcard(true)]
        SynonymDictionary,
        [AllowsWildcard(true)]
        Territory,
        [AllowsWildcard(true)]
        Territory2,
        [AllowsWildcard(true)]
        Territory2Model,
        [AllowsWildcard(true)]
        Territory2Rule,
        [AllowsWildcard(false)]
        Territory2Settings,
        [AllowsWildcard(true)]
        Territory2Type,
        [AllowsWildcard(true)]
        Translations,
        [AllowsWildcard(false)]
        ValidationRule,
        [AllowsWildcard(false)]
        Weblink,
        [Folder("workflows"), SalesforceDirectory("Workflows"), Supported(true), AllowsWildcard(true), FileExtension(".workflow"), DisplayName("Workflow")]
        Workflow,
        [MetadataSubType(MetadataType.Workflow)]
        WorkflowAlert,
        [MetadataSubType(MetadataType.Workflow)]
        WorkflowFieldUpdate,
        [MetadataSubType(MetadataType.Workflow)]
        WorkflowOutboundMessage,
        [MetadataSubType(MetadataType.Workflow)]
        WorkflowRule,
        [MetadataSubType(MetadataType.Workflow)]
        WorkflowTask,
        [AllowsWildcard(true)]
        XOrgHub,
        [AllowsWildcard(true)]
        XOrgHubSharedObject,
    }
}
