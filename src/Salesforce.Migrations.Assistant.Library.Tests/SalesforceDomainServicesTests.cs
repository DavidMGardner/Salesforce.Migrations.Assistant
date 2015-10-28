using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Salesforce.Migrations.Assistant.Library.Configuration;
using Salesforce.Migrations.Assistant.Library.Domain;
using Salesforce.Migrations.Assistant.Library.MetaDataService;
using Shouldly;
using ApexClass = Salesforce.Migrations.Assistant.Library.Tooling.SforceService.ApexClass1;
using DeployOptions = Salesforce.Migrations.Assistant.Library.Domain.DeployOptions;
using DeployResult = Salesforce.Migrations.Assistant.Library.Domain.DeployResult;

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
                    },
                    new SalesForceEnvionment
                    {
                        Name = "Dev58 SandBox",
                        AuthorizationCredential = new Credential
                        {
                            UserName = "admin@dtxdc.net.dev58",
                            Password = "@Sogeti11!",
                            Token = "68CsU07xwwEcgUbC6guo1Oix",
                            EnvironmentType = SalesforceEnvironmentType.Sandbox
                        }
                    },
                    new SalesForceEnvionment
                    {
                        Name = "Dev56 SandBox",
                        AuthorizationCredential = new Credential
                        {
                            UserName = "admin@dtxdc.net.dev56",
                            Password = "@Sogeti11!",
                            Token = "YBm64kT2z18rGOnTUuTJmAf1",
                            EnvironmentType = SalesforceEnvironmentType.Sandbox
                        }
                    },
                    new SalesForceEnvionment
                    {
                        Name = "Dev43 SandBox",
                        AuthorizationCredential = new Credential
                        {
                            UserName = "admin@dtxdc.net.dev43",
                            Password = "@Sogeti11!",
                            Token = "Dcv65G0PfeMbDnVCYKsz50OZf",
                            EnvironmentType = SalesforceEnvironmentType.Sandbox
                        }
                    },
                },
                PullEnvironments = new List<string>
                {
                    "Dev54 SandBox",
                    "Dev49 SandBox",
                    "Dev58 SandBox",
                    "Dev56 SandBox",
                    "Dev43 SandBox"
                },
                PushEnvironments = new List<string>
                {
                    "Dev58 SandBox"
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

            var resp = new SalesforceRepository(ph.GetContext("Dev43 SandBox"), new DateTimeDirectoryStrategy());
            var files = resp.DownloadFiles(resp.GetLatestFiles((properties, i) =>
                properties.fullName.Contains("CP_") && properties.lastModifiedDate >= DateTime.Now.AddDays(-15)),
                CancellationToken.None);

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
                    resp.SaveLocal(
                        ((fp, i) => fp.fullName.Contains("CP_") && fp.lastModifiedDate >= DateTime.Now.AddDays(-15)),
                        CancellationToken.None);
                }
            }
        }

        [TestMethod]
        public void DeploymentTest()
        {
            var ph = new ProjectHandler().Initialize();

            var resp = new SalesforceRepository(ph.GetContext("Dev58 SandBox"), new NoPullStrategy());

            var options = new DeployOptions()
            {
                CheckOnly = false,
                IgnoreWarnings = false,
                PerformeRetrive = false,
                RollbackOnError = true
            };

            var id = resp.Deploy(@"D:\salesforce.migrations\solution\Dev49 SandBox\10-13-2015-16-08-55", options);

            MetaDataService.DeployResult result = SalesforceRepositoryHelpers.WaitDeployResult(id, resp.GetContext, new CancellationToken());

            if (result.details.componentFailures != null)
            {
                foreach (DeployMessage item in result.details.componentFailures)
                {
                    if (!string.IsNullOrWhiteSpace(item.problem))
                    {
                        Console.WriteLine(item.problem);
                    }
                }
            }

            Assert.IsTrue(result.done);
        }

        public void PushDeployAllFiles()
        {
            var ph = new ProjectHandler().Initialize();

            var resp = new SalesforceRepository(ph.GetContext("Dev58 SandBox"),null, new VisualForceDeploymentStrategy());

            var options = new DeployOptions()
            {
                CheckOnly = false,
                IgnoreWarnings = false,
                PerformeRetrive = false,
                RollbackOnError = true
            };


            resp.Deploy(@"D:\\salesforce.migrations\\solution\\Dev54 SandBox\\_current", options);
        }


        //[TestMethod]
        //public void DeploymentZipFileTest()
        //{
        //    var ph = new ProjectHandler().Initialize();

        //    var resp = new SalesforceRepository(ph.GetContext("Dev58 SandBox"), new DateTimeDirectoryStrategy());

        //    var options = new DeployOptions()
        //    {
        //        CheckOnly = false,
        //        IgnoreWarnings = false,
        //        PerformeRetrive = false,
        //        RollbackOnError = true
        //    };

        //    using (ZipFile zip = ZipFile.Read(@"D:\salesforce.migrations\solution\Dev49 SandBox\10-13-2015-16-08-55\package_13add8bf-8deb-4561-bea1-2554b537d577\unpackaged.zip"))
        //    {
        //        MemoryStream memoryStream = new MemoryStream();
        //        zip.Save((Stream)memoryStream);

        //        var id = resp.RunDeployment(memoryStream.ToArray(), options);
        //        MetaDataService.DeployResult result = SalesforceRepositoryHelpers.WaitDeployResult(id, resp.GetContext, new CancellationToken());

        //        if (result.details.componentFailures != null)
        //        {
        //            foreach (DeployMessage item in result.details.componentFailures)
        //            {
        //                if (!string.IsNullOrWhiteSpace(item.problem))
        //                {
        //                    Console.WriteLine(item.problem);
        //                }
        //            }
        //        }

        //        Assert.IsTrue(result.done);
        //    }
        //}
    }

    
}