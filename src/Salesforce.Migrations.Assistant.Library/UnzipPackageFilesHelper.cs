using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Salesforce.Migrations.Assistant.Library.AsyncHelpers;
using Salesforce.Migrations.Assistant.Library.Domain;

namespace Salesforce.Migrations.Assistant.Library
{
    static public class ZipPackageFileHelper
    {
        public static byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }

        static public PackageEntity BuildPackageXML(IList<IDeployableItem> items)
        {
            var response = items.GroupBy(g => g.Type)
              .Select(s => new PackageTypeEntity
              {
                  Name = s.Key.ToString(),
                  Members = s.Select(sm => sm.FileNameWithoutExtension).ToArray()
              })
              .ToArray();

            return new PackageEntity { Version = "29.0", Types = response };
        }

        static public byte[] ZipObjectsForDeploy(IList<IDeployableItem> items)
        {
            ZipFile zipFile = new ZipFile();
            foreach (IDeployableItem deployableItem in (IEnumerable<IDeployableItem>)items)
            {
                string entryName = Path.Combine("unpackaged", deployableItem.Type.GetSalesforceDirectory().ToLower(), Path.GetFileName(deployableItem.Name)).Replace("\\", "/");
                zipFile.AddEntry(entryName, deployableItem.FileBody);
            }

            string xml = BuildPackageXML(items).GetXml().OuterXml;

            zipFile.AddEntry("unpackaged/package.xml", xml);
            MemoryStream memoryStream = new MemoryStream();
            zipFile.Save((Stream)memoryStream);
            return memoryStream.ToArray();
        }
    }


    static public class UnzipPackageFilesHelper
    {
        static public List<SalesforceFileProxy> UnzipPackageFilesRecursive(byte[] zip)
        {
            List<SalesforceFileProxy> sffp = new List<SalesforceFileProxy>();
            foreach (ZipEntry zipEntry in ZipFile.Read((Stream)new MemoryStream(zip)))
            {
                byte[] numArray = new byte[zipEntry.UncompressedSize];
                zipEntry.OpenReader().Read(numArray, 0, numArray.Length);
                SalesforceFileProxy projectFileEntity = new SalesforceFileProxy()
                {
                    BinaryBody = numArray,
                    FileBody = Encoding.UTF8.GetString(numArray),
                    FileName = zipEntry.FileName
                };
                sffp.Add(projectFileEntity);

                if (IsZipFile(numArray))
                {
                    List<SalesforceFileProxy> zipList = UnzipStaticResource(numArray, zipEntry.FileName.RemoveExtension());
                    if (zipList.Count > 0)
                    {
                        projectFileEntity.Type = "StaticResourceZip";
                        zipList.AddRange(zipList);
                    }
                    sffp.AddRange(zipList);
                }
            }
            return sffp;
        }

        static public List<SalesforceFileProxy> UnzipPackageFiles(byte[] zip)
        {
            List<SalesforceFileProxy> list = new List<SalesforceFileProxy>();
            foreach (ZipEntry zipEntry in ZipFile.Read(new MemoryStream(zip)))
            {
                byte[] numArray = new byte[zipEntry.UncompressedSize];
                zipEntry.OpenReader().Read(numArray, 0, numArray.Length);
                SalesforceFileProxy fileProxy = new SalesforceFileProxy()
                {
                    BinaryBody = numArray,
                    FileBody = Encoding.UTF8.GetString(numArray),
                    FileName = zipEntry.FileName
                };
                list.Add(fileProxy);
            }
            return list;
        }

        static public byte[] ZipObjectsForDeploy(IList<IDeployableItem> items, string packageXml)
        {
            ZipFile zipFile = new ZipFile();
            foreach (IDeployableItem deployableItem in (IEnumerable<IDeployableItem>)items)
            {
                string entryName = Path.Combine("unpackaged", deployableItem.Type.GetSalesforceDirectory().ToLower(), Path.GetFileName(deployableItem.Name)).Replace("\\", "/");
                zipFile.AddEntry(entryName, deployableItem.FileBody);
            }
            zipFile.AddEntry("unpackaged/package.xml", packageXml);
            MemoryStream memoryStream = new MemoryStream();
            zipFile.Save((Stream)memoryStream);
            return memoryStream.ToArray();
        }

        static public bool IsZipFile(byte[] bytes)
        {
            if (bytes.Length == 0)
                return false;
            return BitConverter.ToInt32(bytes, 0) == 67324752;
        }

        static public List<SalesforceFileProxy> UnzipStaticResource(byte[] bytes, string fileName)
        {
            List<SalesforceFileProxy> list = new List<SalesforceFileProxy>();
            ZipFile zipFile = ZipFile.Read((Stream)new MemoryStream(bytes));
            if (zipFile.Count == 0)
                return list;
            foreach (ZipEntry zipEntry in zipFile)
            {
                byte[] buffer = new byte[zipEntry.UncompressedSize];
                if (!zipEntry.IsDirectory)
                {
                    zipEntry.OpenReader().Read(buffer, 0, buffer.Length);
                    string str = Path.Combine(fileName, zipEntry.FileName).ReplaceUnixDirectorySeparator().ReplaceWebSeparator();
                    SalesforceFileProxy projectFileEntity = new SalesforceFileProxy()
                    {
                        BinaryBody = buffer,
                        FileName = str,
                        ResourceFileName = fileName,
                        Type = "StaticResourceZipItem",
                        PathInResource = str
                    };
                    list.Add(projectFileEntity);
                }
            }
            return list;
        }
    }
}