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

        public XmlDocument BuildPackageFile()
        {
            SMALGit migrationassist = new SMALGit(_workingDirectory, _gitCommit);

            IEnumerable<Change> changes = migrationassist.GetTreeChanges(_gitBranch);

            var enumerable = changes as IList<Change> ?? changes.ToList();
            if (enumerable.Any())
            {
                XmlDocument output = GeneratePackageFile.GenerateOutputXml(enumerable);

                return output;
            }

            return null;
        }

        public string GetChangeDetails
        {
            get
            {
                SMALGit migrationassist = new SMALGit(_workingDirectory, _gitCommit);
                var comment = migrationassist.GetCommitComment(_gitBranch);
                if (comment != null)
                {
                    return String.Format("Commited by: {1} ({2}) Comment: {0} ", comment.Message, comment.AuthorName, comment.AuthorEmail);
                }

                return String.Empty;
            }
        }
    }
}
