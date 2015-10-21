using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using CLAP;
using CLAP.Validation;
using Salesforce.Migrations.Assistant.Library;
using Salesforce.Migrations.Assistant.Library.Domain;
using Salesforce.Migrations.Assistant.Library.MetaDataService;
using Serilog;
using DeployOptions = Salesforce.Migrations.Assistant.Library.Domain.DeployOptions;

namespace SalesforceMigrations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            foreach (var s in args)
            {
                Console.WriteLine("Arg: {0}", s);
            }

            try
            {
                Parser.Run<CommadParser>(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            
        }
    }

    internal class CommadParser
    {
        [Verb]
        public static void PullAllEnvironments([Required][DefaultValue("1")] int daysbehind)
        {
            var ph = new ProjectHandler().Initialize();

            int daysBack = System.Math.Abs(daysbehind) * (-1);

            foreach (string salesForcePullEnvionment in ph.GetPullEnviroments())
            {
                var ctx = ph.GetContext(salesForcePullEnvionment);
                if (ctx != null)
                {
                    var resp = new SalesforceRepository(ctx, new DateTimeDirectoryStrategy());
                    resp.SaveLocal(((fp, i) => fp.fullName.Contains("CP_") && fp.lastModifiedDate >= DateTime.Now.AddDays(daysBack)), CancellationToken.None);
                }
            }
        }

        [Verb]
        public static void PushStaticResources([Required] string environment, string packagedirectory)
        {
            var ph = new ProjectHandler().Initialize();

            if(String.IsNullOrWhiteSpace(packagedirectory))
                packagedirectory = System.IO.Directory.GetCurrentDirectory();


            Console.WriteLine("Pushing from: {0}", packagedirectory);

            try
            {
                var resp = new SalesforceRepository(ph.GetContext(environment), new DateTimeDirectoryStrategy());

                var options = new DeployOptions()
                {
                    CheckOnly = false,
                    IgnoreWarnings = false,
                    PerformeRetrive = false,
                    RollbackOnError = true
                };

                var zip = resp.PackageStaticResources(packagedirectory, options);
                var id = resp.RunDeployment(zip, options);

                Salesforce.Migrations.Assistant.Library.MetaDataService.DeployResult result = SalesforceRepositoryHelpers.WaitDeployResult(id, resp.GetContext, new CancellationToken());

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

                if (result.done == true && result.success == true)
                {
                    Console.WriteLine("Deployment completed successfully with no errors!");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [Verb]
        public static void BuildPackage(
                        [DirectoryExists][Required] string workingdirectory,
                        [DirectoryExists][Required] string buildOutputDirectory,
                        [Required] string gitUrl,
                        [Required][DefaultValue("origin/develop")] string gitBranch,
                        [Required] string gitCommit)
        {
            SalesForceMigrationsAssistantBuild smab = new SalesForceMigrationsAssistantBuild( workingdirectory,
                                                                                              buildOutputDirectory,
                                                                                              gitUrl,
                                                                                              gitBranch,
                                                                                              gitCommit);

            XmlDocument document = smab.BuildPackageFile();

            if (document != null)
            {
                if (StructureManagement.BuildFileSystemStructure(buildOutputDirectory))
                {
                    // ReSharper disable once UseStringInterpolation
                    string formattedfilename = string.Format("{0}\\{1}", buildOutputDirectory,
                        StructureManagement.ManifestRelativeLocation);

                    // ReSharper disable once UseStringInterpolation
                    document.Save(string.Format("{0}\\package.xml",formattedfilename));

                    // ReSharper disable once UseStringInterpolation
                    System.IO.File.WriteAllText(string.Format("{0}\\change.log", formattedfilename), smab.GetChangeDetails);
                    

                    Console.WriteLine("XML Output {0}", document.InnerXml);
                }
                
            }
        }
    }
}
