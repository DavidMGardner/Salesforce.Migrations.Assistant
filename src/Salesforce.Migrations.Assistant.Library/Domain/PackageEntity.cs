using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class PackageEntity
    {
        private readonly List<PackageTypeEntity> _listTypes = new List<PackageTypeEntity>();

        public string Version { get; set; }

        public PackageEntity.PackageTypeEntity[] Types
        {
            get { return _listTypes.ToArray(); }
            set { _listTypes.AddRange(value);  }
        }

        public PackageEntity SetVersion(string ver)
        {
            Version = ver;
            return this;
        }

        public PackageEntity AddGlobal(string type)
        {
            _listTypes.Add(new PackageTypeEntity
            {
                Name = type,
                Members = new[] {"*"}
            });

            return this;
        }

        public class PackageTypeEntity
        {
            public string Name { get; set; }
            public string[] Members { get; set; }
        }
    }
}
