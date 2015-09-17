using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class PackageEntity
    {
        public string Version { get; set; }

        public PackageEntity.PackageTypeEntity[] Types { get; set; }

        public class PackageTypeEntity
        {
            public string Name { get; set; }

            public string[] Members { get; set; }
        }
    }
}
