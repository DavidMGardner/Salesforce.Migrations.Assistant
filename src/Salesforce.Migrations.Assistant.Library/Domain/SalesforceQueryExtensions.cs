using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    static public class SalesforceQueryExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
        {
            T[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new T[size];

                bucket[count++] = item;

                if (count != size)
                    continue;

                yield return bucket.Select(x => x);

                bucket = null;
                count = 0;
            }

            // Return the last bucket with all remaining elements
            if (bucket != null && count > 0)
                yield return bucket.Take(count);
        }

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