using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CLAP;
using CLAP.Validation;
using Salesforce.Migrations.Assistant.Library;

namespace SalesforceMigrations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Run<CommadParser>(args);
        }
    }

    internal class CommadParser
    {
        [Verb(IsDefault = true)]
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

            // ReSharper disable once UseStringInterpolation
            document.Save(string.Format("{0}\\package.xml", buildOutputDirectory));

        }
    }
}
