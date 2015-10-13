using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Salesforce.Migrations.Assistant.Library.Configuration;
using Salesforce.Migrations.Assistant.Library.Domain;
using Salesforce.Migrations.Assistant.Library.MetaDataService;
using Shouldly;
using ApexClass = Salesforce.Migrations.Assistant.Library.Tooling.SforceService.ApexClass1;

namespace Salesforce.Migrations.Assistant.Library.Tests
{
    [TestClass]
    public class SalesforceDomainServicesTests
    {
        [TestInitialize]
        public void Setup()
        {
            SalesforceMigrationsProject project = new SalesforceMigrationsProject
            {
                ContextTypes = new[]
                {
                    "ApexClass", "ApexComponent", "ApexPage", "ApexTrigger", "Workflow", "RemoteSiteSetting",
                    "PermissionSet", "CustomObject", "StaticResource", "Profile"
                }.ToList(),
                Environments = new List<SalesForceEnvionment>
                {
                    new SalesForceEnvionment
                    {
                        Name = "Dev54 SandBox",
                        AuthorizationCredential = new Credential
                        {
                            UserName = "admin@dtxdc.net.dev54",
                            Password = "@Sogeti11!",
                            Token = "UBB2jcTNepy81bfDhy1duZt4I",
                            EnvironmentType = SalesforceEnvironmentType.Sandbox
                        }
                    },
                     new SalesForceEnvionment
                    {
                        Name = "Dev49 SandBox",
                        AuthorizationCredential = new Credential
                        {
                            UserName = "admin@dtxdc.net.dev49",
                            Password = "@Sogeti11!",
                            Token = "Up5Iauh1187WdUDLB519PVoN9",
                            EnvironmentType = SalesforceEnvironmentType.Sandbox
                        }
                    }
                },
                PullEnvironments = new List<string>
                {
                    "Dev54 SandBox",
                    "Dev49 SandBox",
                }
            };

            ProjectHandler projectHandler = new ProjectHandler()
                .Initialize(project);

            projectHandler.SaveProject();
        }

        [TestMethod]
        public void RunSingle()
        {
            var ph = new ProjectHandler().Initialize();

            var resp = new SalesforceRepository(ph.GetContext("Dev54 SandBox"), new DateTimeDirectoryStrategy());
            var files = resp.DownloadFiles(resp.GetLatestFiles((properties, i) => 
                                    properties.fullName.Contains("CP_") && properties.lastModifiedDate >= DateTime.Now.AddDays(-1)), CancellationToken.None);

            resp.SaveLocal(files);
        }

        [TestMethod]
        public void RunAllEnvironments()
        {
            var ph = new ProjectHandler().Initialize();

            foreach (string salesForcePullEnvionment in ph.GetPullEnviroments())
            {
                var ctx = ph.GetContext(salesForcePullEnvionment);
                if (ctx != null)
                {
                    var resp = new SalesforceRepository(ctx, new DateTimeDirectoryStrategy());
                    resp.SaveLocal(((fp, i) => fp.fullName.Contains("CP_") && fp.lastModifiedDate >= DateTime.Now.AddDays(-1)), CancellationToken.None);
                }
            }
        }
    }
}