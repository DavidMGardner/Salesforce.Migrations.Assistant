using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Salesforce.Migrations.Assistant.Library.MetaDataService;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public static class SalesforceRepositoryHelpers
    {
        public static List<SalesforceFileProxy> DownloadAllFilesSynchronously(CancellationToken cancellationToken, SalesforceContext ctx)
        {
            PackageEntity pe = new PackageEntity().BuildQueryAllCommonTypes();
            return DownloadAllFilesSynchronously(pe, ctx, cancellationToken);
        }

        public static List<SalesforceFileProxy> DownloadAllFilesSynchronously(PackageEntity package, SalesforceContext ctx,  CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!ctx.IsLoggedIn)
                throw new InvalidOperationException("Service should be logged in Salesforce");

            List<SalesforceFileProxy> listFileProxies = new List<SalesforceFileProxy>();
            RetrieveRequest[] retrieveRequestArray = ConvertPackageToRequests(package);

            List<AsyncResult> listResults = new List<AsyncResult>();

            foreach (var retrieveRequest in retrieveRequestArray)
            {
                RetrieveRequest request = retrieveRequest;
                cancellationToken.ThrowIfCancellationRequested();
                AsyncResult asyncResult = ctx.SessionExpirationWrapper(() => ctx.MetadataServiceAdapter.Retrieve(request));
                listResults.Add(asyncResult);
            }

            cancellationToken.ThrowIfCancellationRequested();
            ICollection<Task<RetrieveResult>> collection = new Collection<Task<RetrieveResult>>();
            foreach (var asyncResult in listResults)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Task<RetrieveResult> task = Task<RetrieveResult>.Factory.StartNew(() => ctx.PollingResult.PollForResultWrapper(3, 180, 3, () =>
                        ctx.CheckRetrieveResult(asyncResult.id), res => res != null, cancellationToken), cancellationToken);
                collection.Add(task);
            }

            ctx.WaitAllTaskskWithHandlingAggregateException(collection.ToArray(), cancellationToken);
            foreach (Task<RetrieveResult> task in collection)
            {
                cancellationToken.ThrowIfCancellationRequested();
                RetrieveResult result = task.Result;

                string fileName = String.Format("{0}\\{1}.zip", ctx.OutputLocation, Guid.NewGuid().ToString());

                bool rawZip = Boolean.Parse(ConfigurationManager.AppSettings["salesforcemigrations:dumprawzip"]);

                if(rawZip)
                {
                    SalesforceFileProcessing.EnsureFolder(fileName);
                    SalesforceFileProcessing.SaveData(fileName, result.zipFile);
                }
               
                List<SalesforceFileProxy> files = UnzipPackageFilesHelper.UnzipPackageFilesRecursive(result.zipFile);

                UpdateFileProperties(files, result.fileProperties);
                listFileProxies.AddRange(files);
            }
            return listFileProxies;
        }


        public static RetrieveRequest[] ConvertPackageToRequests(PackageEntity package)
        {
            return package.Types.Select(t =>
            {
                RetrieveRequest retrieveRequest = new RetrieveRequest
                {
                    apiVersion = Double.Parse(package.Version, CultureInfo.InvariantCulture),
                    unpackaged = new Package()
                    {
                        types = new[]
                        {
                            new PackageTypeMembers()
                            {
                                name = t.Name,
                                members = t.Members
                            }
                        }
                    }
                };
                return retrieveRequest;
            }).ToArray();
        }

        public static void UpdateFileProperties(List<SalesforceFileProxy> files, FileProperties[] properties)
        {
            foreach (FileProperties fileProperties in properties)
            {
                foreach (SalesforceFileProxy projectFileEntity in files)
                {
                    if (projectFileEntity.FileName == fileProperties.fileName)
                    {
                        projectFileEntity.FullName = fileProperties.fullName;
                        projectFileEntity.Id = fileProperties.id;
                        projectFileEntity.CreatedByName = fileProperties.createdByName;
                        projectFileEntity.ModifiedByName = fileProperties.lastModifiedByName;
                        projectFileEntity.CreatedDateUtcTicks = fileProperties.createdDate.ToFileTimeUtc().ToString();
                        projectFileEntity.Id = fileProperties.id;
                        projectFileEntity.NamespacePrefix = fileProperties.namespacePrefix;
                        projectFileEntity.Type = projectFileEntity.Type ?? fileProperties.type;
                        projectFileEntity.LastModifiedDateUtcTicks = projectFileEntity.Type != "CustomObject" ? fileProperties.lastModifiedDate.ToFileTimeUtc().ToString() : DateTimeProvider.UtcNow.ToFileTimeUtc().ToString();
                    }
                    else if (projectFileEntity.FileName.Contains("-meta.xml"))
                        projectFileEntity.Type = "Metadata";
                    else if (projectFileEntity.FileName.Contains("/package.xml"))
                        projectFileEntity.Type = "Package";
                }
            }
        }
    }
}