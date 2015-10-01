using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Salesforce.Migrations.Assistant.Library.Domain;
using Shouldly;
using ApexClass = Salesforce.Migrations.Assistant.Library.Tooling.SforceService.ApexClass1;

namespace Salesforce.Migrations.Assistant.Library.Tests
{
    [TestClass]
    public class SalesforceDomainServicesTests
    {
        [TestMethod]
        public void Test1()
        {
            SalesforceRepository resp = new SalesforceRepository();

            var list = resp.List;
        }

        [TestMethod]
        public void FilteredTest()
        {
            SalesforceRepository resp = new SalesforceRepository();
            var list = resp.FilteredList;

            var salesforceFileProxies = list as SalesforceFileProxy[] ?? list.ToArray();
                salesforceFileProxies.ProcessFiles();

            Assert.IsTrue(salesforceFileProxies.Any());
        }

        [TestMethod]
        public void DownloadTests()
        {
            //SalesforceDomainServices svc = new SalesforceDomainServices();
            //string username = "dave.gardner@us.sogeti.com";
            //string password = "@Tngds10!";
            //string token = "xoWNt5SPFiT0RZWo0QBQHqz8";

            //svc.LoginSandbox(username, password, token);
            //var response = svc.DownloadAllFilesSynchronously(new PackageEntity
            //{   
            //    Version = "29.0",
            //    Types = new[] { new PackageEntity.PackageTypeEntity
            //    {
            //        Name = "ApexClass",
            //        Members = new[] {"*"}
            //    }, }
            //}, new CancellationToken());

            //Assert.IsTrue(response.Count > 0);
        }


        [TestMethod]
        public void QueryByFileName()
        {
            //SalesforceDomainServices svc = new SalesforceDomainServices();
            //string username = "dave.gardner@us.sogeti.com";
            //string password = "@Tngds10!";
            //string token = "xoWNt5SPFiT0RZWo0QBQHqz8";

            //svc.LoginSandbox(username, password, token);

            //var file = svc.QueryApexFileByName("CP_COM_ContractdetailsSvcController", "ApexClass");
        }


        [TestMethod]
        public void QueryByOperator()
        {
            //SalesforceDomainServices svc = new SalesforceDomainServices();
            //string username = "dave.gardner@us.sogeti.com";
            //string password = "@Tngds10!";
            //string token = "xoWNt5SPFiT0RZWo0QBQHqz8";

            //svc.LoginSandbox(username, password, token);

            //var file = svc.QueryItemsByName(new SalesforceQuery()
            //                                    .Select("Id, Name, CreatedDate, CreatedById, NamespacePrefix, LastModifiedDate, Body")
            //                                    .From("ApexClass")
            //                                    .Where("LastModifiedDate")
            //                                    .GreaterThanDateTime(DateTimeOffset.Parse("09/21/2015").ToString("o")));
         }

        [TestMethod]
        public void QueryBuilder()
        {
            //string.Format("select Id, Name, CreatedDate, CreatedById, NamespacePrefix, LastModifiedDate, Body from {0} where Name {1} '{2}'", type, @operator, name))

            string qs = new SalesforceQuery()
                .Select("Id, Name, CreatedDate, CreatedById, NamespacePrefix, LastModifiedDate, Body")
                .From("ApexClass")
                .Where("Name")
                .Like("CP%")
                .ToString();

            Console.WriteLine(qs);
        }

        [TestMethod]
        public void DatetimeFormats()
        {
            string dt = DateTimeOffset.Parse("09/21/2015").ToString("o");
            Console.WriteLine(dt);
        }

        [TestMethod]
        public void GetChangedFiles()
        {
            //SalesforceDomainServices svc = new SalesforceDomainServices();
            //string username = "dave.gardner@us.sogeti.com";
            //string password = "@Tngds10!";
            //string token = "xoWNt5SPFiT0RZWo0QBQHqz8";

            //svc.LoginSandbox(username, password, token);

            //var result = svc.GetFilesChangedSince(DateTimeOffset.Parse("09/21/2015"));

            //Console.WriteLine(result.Count());

            //Assert.IsTrue(result.Any());
        }
    }
}
