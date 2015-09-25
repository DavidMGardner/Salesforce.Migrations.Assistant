using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    static public class SalesforceQueryExtensions
    {
        public static SalesforceQuery QueryApexClassesByOffest(DateTimeOffset dto)
        {
            return new SalesforceQuery().Select("Id, Name, CreatedDate, CreatedById, NamespacePrefix, LastModifiedDate, Body")
                .From("ApexClass")
                .Where("LastModifiedDate")
                .GreaterThanDateTime(dto.ToString("o"));
        }

        public static SalesforceQuery QueryApexTriggersByOffest(DateTimeOffset dto)
        {
            return new SalesforceQuery().Select("Id, Name, CreatedDate, CreatedById, NamespacePrefix, LastModifiedDate, Body")
                .From("ApexTrigger")
                .Where("LastModifiedDate")
                .GreaterThanDateTime(dto.ToString("o"));
        }

        public static SalesforceQuery QueryApexPagesByOffest(DateTimeOffset dto)
        {
            return new SalesforceQuery().Select("Id, Name, CreatedDate, CreatedById, LastModifiedDate")
                .From("ApexPage")
                .Where("LastModifiedDate")
                .GreaterThanDateTime(dto.ToString("o"));
        }

        public static SalesforceQuery QueryApexComponentsByOffest(DateTimeOffset dto)
        {
            return new SalesforceQuery().Select("Id, Name, LastModifiedDate")
                .From("ApexComponent")
                .Where("LastModifiedDate")
                .GreaterThanDateTime(dto.ToString("o"));
        }

        public static SalesforceQuery QueryStaticResourcesByOffest(DateTimeOffset dto)
        {
            return new SalesforceQuery().Select("Id, Name, CreatedDate, CreatedById, LastModifiedDate, Body")
                .From("StaticResource")
                .Where("LastModifiedDate")
                .GreaterThanDateTime(dto.ToString("o"));
        }
    }


    public static class EnumerableExtensions
    {
        public static void NullSafeAddRange<T>(this IEnumerable<T> self, List<T> destination)
        {
            if (self != null) destination.AddRange(self);
        }
    }

    public static class DynamicExtensions
    {
        public static bool IsPropertyExist(dynamic dynamicObj, string property)
        {
            try
            {
                //var value = dynamicObj[property].Value;
                return dynamicObj.GetType().GetProperty(property) != null; 
            }
            catch (RuntimeBinderException)
            {
                return false;
            }
        }
    }
}