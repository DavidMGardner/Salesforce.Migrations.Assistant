using System;
using System.IO;
using Salesforce.Migrations.Assistant.Library.Crypto;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public abstract class SalesforceFileNode : ISalesforceItem, IDeployableItem
    {
        protected SalesforceFileNode(PackageEntity root)
        {
            
        }

        protected SalesforceFileNode()
        {

        }

        public string Url { get; set; }
        public string FullPath => Url;

        public string Id
        {
            get { throw new NotImplementedException(); }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string FileNameWithoutExtension
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public abstract MetadataType Type { get; }

        public DateTime LastModifiedDateOnRemote { get; set; }

        public virtual bool IsChangedOnRemote
        {
            get
            {
                if (this.IsDeletedOnRemote)
                    return false;
                if (this.LastModifiedDateOnRemote == new DateTime())
                    throw new InvalidOperationException("LastModifiedDateOnRemote should be requested first");
                return !this.LastModifiedDateOnRemote.ToFileTimeUtc().ToString().Equals(this.LastModefiedDateUtcTicks);
            }
        }

        public bool IsChangedOnLocal
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.LocalHash))
                    return false;
                return !this.Hash.Equals(this.LocalHash);
            }
        }

        public bool IsDeletedOnRemote { get; set; }

        public byte[] FileBody
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.FullPath) && File.Exists(this.FullPath))
                    return File.ReadAllBytes(this.FullPath);
                return (byte[])null;
            }
        }

        public string LocalHash
        {
            get
            {
                if (this.FileBody != null)
                    return Convert.ToBase64String(CryptographyProvider.ComputeMurMurHash(this.FileBody));
                return string.Empty;
            }
        }

        public string Hash
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public byte[] RemoteBinaryBody { get; set; }

        public string LastModefiedDateUtcTicks
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        
    }
}