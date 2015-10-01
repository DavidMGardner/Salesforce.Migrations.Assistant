using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moo.Extenders;
using Salesforce.Migrations.Assistant.Library.Tooling.SforceService;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public static class ObjectExtensions
    {
        public static sObject Cast<T>(this object typeHolder)
        {
            return (sObject)typeHolder;
        }
    }

    public class ApexAdapter
    {
        private static IEnumerable<sObject> GetApexMembers<T>(IEnumerable<ICompilableItem> files, string metadataContainerId) where T : new()
        {
            return files.Where(s => s.Type == MetadataType.ApexTrigger).Select(s => new
            {
                ContentEntityId = s.Id,
                MetadataContainerId = metadataContainerId,
                Body = s.FileBody
            }.MapTo<T>().Cast<sObject>()).ToArray();
        }

        private static IEnumerable<sObject> GetApexClassMembers(IEnumerable<ICompilableItem> files, string metadataContainerId)
        {
            return files.Where(s => s.Type == MetadataType.ApexClass).Select(s => new ApexClassMember()
            {
                ContentEntityId = s.Id,
                MetadataContainerId = metadataContainerId,
                Body = s.FileBody
            }).ToArray();
        }

        private static IEnumerable<sObject> GetApexTriggerMembers(IEnumerable<ICompilableItem> files, string metadataContainerId)
        {
            return files.Where(s => s.Type == MetadataType.ApexTrigger).Select(s => new ApexTriggerMember()
            {
                ContentEntityId = s.Id,
                MetadataContainerId = metadataContainerId,
                Body = s.FileBody
            }).ToArray();
        }


       //private static 
    }
}
