using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class SalesforceFileProxy
    {
        private string _fileName;

        public string Id { get; set; }
        public string FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value?.Trim();
            }
        }

        public virtual string FullName { get; set; }
        public string Hash { get; set; }
        public string LocalHash { get; set; }
        public string CreatedDateUtcTicks { get; set; }
        public string LastModifiedDateUtcTicks { get; set; }
        public string LastLocalModifiedDateUtcTicks { get; set; }
        public string FileBody { get; set; }
        public string Type { get; set; }
        public FileInfo FileInfo { get; set; }
        public string CreatedByName { get; set; }
        public string NamespacePrefix { get; set; }
        public byte[] BinaryBody { get; set; }
        public string ResourceFileName { get; set; }
        public string PathInResource { get; set; }


        public override string ToString()
        {
            return this.FileName + "." + this.Type;
        }
    }
}
