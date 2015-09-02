using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SalesforceMigrations;

namespace Salesforce.Migrations.Assistant.Library.Tests
{
    [TestFixture]
    public class CommandLineTests
    {
        /*
        Git Url: https://github.com/DavidMGardner/DFS-SalesForce.git
        Git Branch: origin/develop
        Git Commit: c22310bd4e6f7a82060283ed63c9eccfb6621fa4
        Workspace: C:\Program Files (x86)\Jenkins\jobs\Salesforce Dev54  Environment\workspace
        */


        [Test]
        public void CommandLineIn()
        {
            Program.Main(new string[]
            {
                @"-workingdirectory=D:\code\SalesforceDevelopment\GitHub",
                @"-buildOutputDirectory=D:\code\SalesforceDevelopment\temp",
                @"-gitUrl=https://github.com/DavidMGardner/DFS-SalesForce.git",
                @"-gitBranch=origin/develop",
                @"-gitCommit=c22310bd4e6f7a82060283ed63c9eccfb6621fa4"
            });
        }
    }
}
