using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Salesforce.Migrations.Assistant.Library.DomainAttributes;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public static class MetadataTypeExtensions
    {
        private static T[] GetAttributes<T>(this MetadataType enumMember)
        {
            return typeof(MetadataType).GetMember(enumMember.ToString())[0].GetCustomAttributes(typeof(T), false).OfType<T>().ToArray<T>();
        }

        private static TResult GetAttributeValueOrDefault<T, TResult>(this MetadataType enumMember, Func<T, TResult> value)
        {
            T[] attributes = GetAttributes<T>(enumMember);
            if (attributes.Length == 0)
                return default(TResult);
            return value(attributes[0]);
        }

        private static TResult GetAttributeValueOrThrowException<T, TResult>(this MetadataType enumMember, Func<T, TResult> value)
        {
            T[] attributes = enumMember.GetAttributes<T>();
            if (attributes.Length == 0)
                throw new MetadataSubTypeParentTypeIsNotPresentException(enumMember.ToString());
            return value(attributes[0]);
        }

        private static bool HasMetadataAttribute<T>(this MetadataType metadataType) where T : Attribute
        {
            return metadataType.GetAttributes<T>().Length > 0;
        }

        public static bool AllowsWildcard(this MetadataType enumMember)
        {
            return enumMember.GetAttributeValueOrDefault(value: (Func<AllowsWildcardAttribute, bool>)(x => x.IsSupported));
        }

        public static bool IsSupported(this MetadataType enumMember)
        {
            return enumMember.GetAttributeValueOrDefault(value: (Func<SupportedAttribute, bool>)(x => x.IsSupported));
        }

        public static bool IsIncludedByDefault(this MetadataType enumMember)
        {
            return enumMember.HasMetadataAttribute<IncludedByDefaultAttribute>();
        }

        public static string GetMetadataTypeDisplayName(this MetadataType enumMember)
        {
            return enumMember.GetAttributeValueOrDefault((Func<DisplayNameAttribute, string>)(x => x.DisplayName));
        }

        public static string GetFileMemberFileExtensionByType(this MetadataType enumMember)
        {
            return enumMember.GetAttributeValueOrDefault(value: (Func<FileExtensionAttribute, string>)(x => x.Extension));
        }

        public static MetadataType GetMetadataTypeByExtension(this string fileExtension)
        {
            foreach (MetadataType enumMember in Enum.GetValues(typeof(MetadataType)).OfType<MetadataType>())
            {
                string attributeValueOrDefault = enumMember.GetAttributeValueOrDefault((Func<FileExtensionAttribute, string>)(x => x.Extension));
                if (!string.IsNullOrEmpty(attributeValueOrDefault) && attributeValueOrDefault.Equals(fileExtension, StringComparison.OrdinalIgnoreCase))
                    return enumMember;
            }
            throw new MetadataTypeIsNotFoundByExtension(fileExtension);
        }

        public static MetadataType[] GetSubTypesForMetadata(this MetadataType parent)
        {
            IEnumerable<MetadataType> enumerable = Enum.GetValues(typeof(MetadataType)).OfType<MetadataType>();
            List<MetadataType> list = new List<MetadataType>();
            foreach (MetadataType metadataType in enumerable)
            {
                if (HasMetadataAttribute<MetadataSubTypeAttribute>(metadataType) && metadataType.GetAttributeValueOrThrowException(value: (Func<MetadataSubTypeAttribute, MetadataType>)(i => i.Parent)) == parent)
                    list.Add(metadataType);
            }
            return list.ToArray();
        }

        public static string GetSalesforceDirectory(this MetadataType metadata)
        {
            return metadata.GetAttributeValueOrDefault(value: (Func<SalesforceDirectory, string>)(i => i.SfDirectory));
        }

        public class AllowsWildcardAttributeMissingException : Exception
        {
            public AllowsWildcardAttributeMissingException(string message)
                : base(message)
            {
            }
        }

        public class AttributeMissingException : Exception
        {
            public AttributeMissingException(string message)
                : base(message)
            {
            }
        }
    }
}