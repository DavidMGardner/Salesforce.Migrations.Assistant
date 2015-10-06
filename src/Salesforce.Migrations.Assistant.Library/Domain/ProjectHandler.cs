using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Salesforce.Migrations.Assistant.Library.Configuration;
using Salesforce.Migrations.Assistant.Library.MetaDataService;
using Serilog;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class ProjectHandler
    {
        private readonly string _projectName = "SalesforceMigrations.json";
        private SalesforceMigrationsProject _salesforceMigrationsProject = new SalesforceMigrationsProject();

        public string GetProjectLocation { get; } = ConfigurationManager.AppSettings["SalesforceMigrations:ProjectLocation"];

        public DateTime LastRun { get; set; } = DateTime.Now;

        public ProjectHandler()
        {

        }

        public ProjectHandler Initialize()
        {
            string fileName = string.Format("{0}\\{1}", GetProjectLocation, _projectName);

            _salesforceMigrationsProject =
                JsonConvert.DeserializeObject<SalesforceMigrationsProject>(File.ReadAllText(fileName));
            return this;
        }

        public ProjectHandler Initialize(SalesforceMigrationsProject salesforceMigrationsProject)
        {
            string fileName = string.Format("{0}\\{1}", GetProjectLocation, _projectName);
            _salesforceMigrationsProject = salesforceMigrationsProject;

            return this;
        }

        public bool UpdateLastRun()
        {
            return UpdateLastRun(LastRun);
        }

        public bool UpdateLastRun(DateTime dt)
        {
            _salesforceMigrationsProject.LastRun = dt;

            if (SaveProject())
                return true;

            return false;
        }

        public bool SaveProject()
        {
            string fileName = String.Format("{0}\\{1}", GetProjectLocation, _projectName);

            SalesforceFileProcessing.EnsureFolder(fileName);
            try
            {
                using (FileStream fs = File.Open(fileName, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, _salesforceMigrationsProject);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public IEnumerable<SalesForceEnvionment> GetBy(Func<SalesForceEnvionment, bool> predicate)
        {
            return _salesforceMigrationsProject.Environments.Where(predicate);
        }

        public IEnumerable<SalesForceEnvionment> GetEnviroments()
        {
            return _salesforceMigrationsProject.Environments;
        }

        public SalesForceEnvionment GetEnviromentsByName(string name)
        {
            return
                _salesforceMigrationsProject.Environments.FirstOrDefault(w => String.Equals(w.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        public SalesforceContext GetContext(string name)
        {
            var env = GetEnviromentsByName(name);

            if (env != null)
            {
                return new SalesforceContext(env);
            }

            return null;
        }

        public IEnumerable<string> GetPullEnviroments()
        {
            return _salesforceMigrationsProject.PullEnvironments;
        }
    }
}
