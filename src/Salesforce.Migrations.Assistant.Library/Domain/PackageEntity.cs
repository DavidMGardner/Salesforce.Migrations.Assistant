using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class PackageTypeEntity
    {
        public string Name { get; set; }
        public string[] Members { get; set; }
    }

    public class PackageEntity
    {
        private readonly List<PackageTypeEntity> _listTypes = new List<PackageTypeEntity>();

        public string Version { get; set; }

        public PackageTypeEntity[] Types
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

        public XmlDocument GetXml()
        {
            XmlOutput xo = new XmlOutput()
           .XmlDeclaration()
           .Node("package").Attribute("xmlns", "http://soap.sforce.com/2006/04/metadata").Within();

            foreach (var salesForceChange in Types)
            {
                XmlOutput xmlOutput = xo.Node("types").Within();
                foreach (var member in salesForceChange.Members)
                {
                    xmlOutput.Node("members").InnerText(member);
                }
                xmlOutput.Node("name").InnerText(salesForceChange.Name)
                       .EndWithin();
            }

            xo.EndWithin()
            .Node("version").InnerText(this.Version);

            return xo.GetXmlDocument();
        }
    }
}
