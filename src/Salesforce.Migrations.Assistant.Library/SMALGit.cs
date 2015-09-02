using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace Salesforce.Migrations.Assistant.Library
{
    public class SMALGit
    {
        private readonly Repository _repository;

        private List<String> additions, deletions, modificationsOld, modificationsNew, contents;
        public string PrevCommit { get; }
        public string CurCommit { get; }

        public SMALGit()
        {
            
        }

        public SMALGit(string pathToRepo)
        {
            _repository = new Repository(pathToRepo);
        }

        public SMALGit(string pathToRepo, string curCommit)
        {
            _repository = new Repository(pathToRepo);
            CurCommit = curCommit;

        }

        public SMALGit(string pathToRepo, string curCommit, string prevCommit)
        {
            _repository = new Repository(pathToRepo);
            CurCommit = curCommit;
            PrevCommit = prevCommit;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public List<Commit> Log()
        {
            return _repository.Commits.Take(15).ToList();
        }

        private Commit TryAndGetCurrentCommit(Branch gitBranch)
        {
            if (String.IsNullOrWhiteSpace(CurCommit))
            {
                return gitBranch.Commits.FirstOrDefault();
            }
            else
            {
                ObjectId commitId;
                if (ObjectId.TryParse(CurCommit, out commitId))
                {
                    return gitBranch.Commits.First(w => w.Id == commitId);
                }
            }
            return null;
        }

        private Commit TryAndGetPreviousCommit(Branch gitBranch)
        {
            if (String.IsNullOrWhiteSpace(PrevCommit))
            {
                return gitBranch.Commits.Skip(1).First();
            }
            else
            {
                ObjectId commitId;
                if (ObjectId.TryParse(PrevCommit, out commitId))
                {
                    return gitBranch.Commits.First(w => w.Id == commitId);
                }
            }
            return null;
        }

        public IEnumerable<Change> GetTreeChanges(string branch)
        {
            var gitBranch = _repository.Branches[branch];

            var gitCurrentCommit = TryAndGetCurrentCommit(gitBranch);
            var gitPreviousCommit = TryAndGetPreviousCommit(gitBranch);

            List<Change> changes = new List<Change>();

            if (gitCurrentCommit != null && gitPreviousCommit != null)
            {
                TreeChanges tc = _repository.Diff.Compare<TreeChanges>(gitPreviousCommit.Tree, gitCurrentCommit.Tree);
                changes.AddRange(tc.Select(c => new Change
                {
                    Path = c.Path,
                    Mode = (GitMode) c.Mode,
                    Status = (GitChangeKind) c.Status
                }));
            }
            else
            {
                throw new Exception("Either the current or previous commits specified could not be found!");
            }

            return changes;
        }

       


        public List<string> GetAdditions()
        {
            //if (additions == null){
            //        additions = new ArrayList<String>();
            //    }
            //return additions;
            throw new NotImplementedException();
        }

        public List<string> GetNewChangeSet()
        {
            //ArrayList<String> newChangeSet = new ArrayList<String>();

            //if (prevCommit == null){
            //        newChangeSet = getContents();
            //    }else{
            //        newChangeSet.addAll(additions);
            //        newChangeSet.addAll(modificationsNew);
            //    }

            //return newChangeSet;
            throw new NotImplementedException();
        }

        public List<string> GetOldChangeSet()
        {
            //ArrayList<String> oldChangeSet = new ArrayList<String>();
            //oldChangeSet.addAll(deletions);
            //oldChangeSet.addAll(modificationsOld);

            //return oldChangeSet;
            throw new NotImplementedException();
        }

        public void GetPrevCommitFiles(List<SMAMetadata> members, string destDir)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePackageXML(string workspace, string userName, string userEmail)
        {
            throw new NotImplementedException();
        }

        public List<string> GetContents()
        {
            throw new NotImplementedException();
        }

        private void DetermineChanges()
        {
            throw new NotImplementedException();
        }

        private List<DiffEntry> GetDiffs()
        {
            throw new NotImplementedException();
        }

        private CanonicalTreeParser getTree(String commit)
        {
            throw new NotImplementedException();
        }

        private static string CheckMeta(string repoItem)
        {
            throw new NotImplementedException();
        }


    }

    public enum GitChangeKind
    {
        Unmodified,
        Added,
        Deleted,
        Modified,
        Renamed,
        Copied,
        Ignored,
        Untracked,
        TypeChanged,
        Unreadable,
    }

    public enum GitMode
    {
        Nonexistent = 0,
        Directory = 16384,
        NonExecutableFile = 33188,
        NonExecutableGroupWritableFile = 33204,
        ExecutableFile = 33261,
        SymbolicLink = 40960,
        GitLink = 57344,
    }

    public class Change
    {
        public string Path { get; set; }
        public GitMode Mode { get; set; }
        public GitChangeKind Status { get; set; }
    }

    internal class CanonicalTreeParser
    {
    }

    internal class DiffEntry
    {
    }

    public class SMAMetadata
    {
    }
}
