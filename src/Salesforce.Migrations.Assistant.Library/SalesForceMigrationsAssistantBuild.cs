using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Salesforce.Migrations.Assistant.Library
{
    public class SalesForceMigrationsAssistantBuild
    {
        private readonly string _workingDirectory;
        private readonly string _buildOutputDirectory;
        private readonly string _gitUrl;
        private readonly string _gitBranch;
        private readonly string _gitCommit;

        public SalesForceMigrationsAssistantBuild(  string workingDirectory, 
                                                    string buildOutputDirectory,
                                                    string gitUrl,
                                                    string gitBranch,
                                                    string gitCommit )
        {
            _workingDirectory = workingDirectory;
            _buildOutputDirectory = buildOutputDirectory;
            _gitUrl = gitUrl;
            _gitBranch = gitBranch;
            _gitCommit = gitCommit;
        }

        private XmlDocument BuildPackageFile()
        {
            SMALGit migrationassist = new SMALGit(_workingDirectory, _gitCommit);

            var changes = migrationassist.GetTreeChanges(_gitBranch);



            GeneratePackageFile.WithXMLOutput();

            return null;
        }
    }
}
