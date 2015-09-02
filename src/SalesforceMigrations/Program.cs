using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLAP;
using CLAP.Validation;
using Salesforce.Migrations.Assistant.Library;

namespace SalesforceMigrations
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Run<CommadParser>(args);
        }
    }

    internal class CommadParser
    {
        [Verb(IsDefault = true)]
        void BuildPackage(
                        [FileExists][Required] string workingdirectory,
                        [FileExists][Required] string buildOutputDirectory,
                        [Required] string gitUrl,
                        [Required][DefaultValue("origin/develop")] string gitBranch,
                        [Required] string gitCommit)
        {
            SalesForceMigrationsAssistantBuild smab = new SalesForceMigrationsAssistantBuild( workingdirectory,
                                                                                              buildOutputDirectory,
                                                                                              gitUrl,
                                                                                              gitBranch,
                                                                                              gitCommit);

            smab.BuildPackageFile();
        }
    }
}
