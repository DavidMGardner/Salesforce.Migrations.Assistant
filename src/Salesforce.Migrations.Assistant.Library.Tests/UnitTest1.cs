using System;
using System.Globalization;
using System.Linq;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace Salesforce.Migrations.Assistant.Library.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            var RFC2822Format = "ddd dd MMM HH:mm:ss yyyy K";
            SMALGit git = new SMALGit(@"D:\code\SalesforceDevelopment\GitHub");

            foreach (Commit c in git.Log())
            {
                Console.WriteLine("commit {0}", c.Id);

                if (c.Parents.Count() > 1)
                {
                    Console.WriteLine("Merge: {0}",
                        string.Join(" ", c.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray()));
                }

                Console.WriteLine("Author: {0} <{1}>", c.Author.Name, c.Author.Email);
                Console.WriteLine("Date:   {0}", c.Author.When.ToString(RFC2822Format, CultureInfo.InvariantCulture));
                Console.WriteLine();
                Console.WriteLine(c.Message);
                Console.WriteLine();
            }
        }

        [Test]
        public void GetDiffWithOnlySpecifyingCurrentCommit()
        {
            // arrange
            SMALGit migrationassist = new SMALGit(@"D:\code\SalesforceDevelopment\GitHub", "c22310bd4e6f7a82060283ed63c9eccfb6621fa4");

            // act
            var listChanges = migrationassist.GetTreeChanges("origin/develop");

            //assert
            listChanges.ShouldNotBeEmpty();



            using (var repo = new Repository(@"D:\code\SalesforceDevelopment\GitHub"))
            {
                foreach (TreeEntryChanges c in repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree,
                    DiffTargets.Index | DiffTargets.WorkingDirectory))
                {
                    Console.WriteLine(c);
                }
            }
        }

        [Test]
        public void GetDiffTest2()
        {
            using (var repo = new Repository(@"D:\code\SalesforceDevelopment\GitHub"))
            {
                Tree headTree = repo.Head.Tip.Tree;

                //Tree headTree2 = repo.Head.C;
                var remoteBranches = repo.Branches.Where(p => p.IsRemote == true).ToList();


                //foreach (var c in .Commits) { }
                var branch = repo.Branches["origin/develop"];


                var currentCommit = branch.Commits.FirstOrDefault();
                if (currentCommit != null)
                    Console.WriteLine("Current Commit == {0}-{1}", currentCommit.Id, currentCommit.Message);


                var previousCommit = branch.Commits.Skip(1).FirstOrDefault();
                if (previousCommit != null)
                    Console.WriteLine("Previous Commit == {0}-{1}", previousCommit.Id, previousCommit.Message);


                //Tree remoteMasterTree = repo.Branches["origin/develop"].Tip.Tree;

                if (previousCommit != null && currentCommit != null)
                {
                    TreeChanges tc = repo.Diff.Compare<TreeChanges>(previousCommit.Tree, currentCommit.Tree);
                    foreach (TreeEntryChanges c in tc)
                    {
                        Console.WriteLine(c.Path);
                    }
                }
            }
        }

        [Test]
        public void GetPackaging()
        {
             GeneratePackageFile.WithXMLOutput();

        }
}
}
