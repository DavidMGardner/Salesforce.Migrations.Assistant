using System;
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
            SalesforceDomainServices svc = new SalesforceDomainServices();

            string username = "dave.gardner@us.sogeti.com";
            string password = "@Tngds10!";
            string token = "xoWNt5SPFiT0RZWo0QBQHqz8";

            svc.LoginSandbox(username, password, token);
            var @class = svc.QueryApexFileByName("CP_COM_ContractdetailsSvcController", "ApexClass");

            @class.ShouldBeOfType<SalesforceFileProxy>();
        }


        [TestMethod]
        public void DownloadTests()
        {
            SalesforceDomainServices svc = new SalesforceDomainServices();
            string username = "dave.gardner@us.sogeti.com";
            string password = "@Tngds10!";
            string token = "xoWNt5SPFiT0RZWo0QBQHqz8";

            svc.LoginSandbox(username, password, token);
            var response = svc.DownloadAllFilesSynchronously(new PackageEntity
            {   
                Version = "29.0",
                Types = new[] { new PackageEntity.PackageTypeEntity
                {
                    Name = "ApexClass",
                    Members = new[] {"*"}
                }, }
            }, new CancellationToken());

            Assert.IsTrue(response.Count > 0);
        }


        [TestMethod]
        public void QueryByFileName()
        {
            SalesforceDomainServices svc = new SalesforceDomainServices();
            string username = "dave.gardner@us.sogeti.com";
            string password = "@Tngds10!";
            string token = "xoWNt5SPFiT0RZWo0QBQHqz8";

            svc.LoginSandbox(username, password, token);

            var file = svc.QueryApexFileByName("*", "ApexClass");
        }


        [TestMethod]
        public void QueryByOperator()
        {
            SalesforceDomainServices svc = new SalesforceDomainServices();
            string username = "dave.gardner@us.sogeti.com";
            string password = "@Tngds10!";
            string token = "xoWNt5SPFiT0RZWo0QBQHqz8";

            svc.LoginSandbox(username, password, token);

            var file = svc.QueryItemsByName<ApexClass>("CP%", "ApexClass", "LIKE");

            Console.WriteLine(file.Count());
        }

        [TestMethod]
        public void QueryBuilder()
        {
            //string.Format("select Id, Name, CreatedDate, CreatedById, NamespacePrefix, LastModifiedDate, Body from {0} where Name {1} '{2}'", type, @operator, name))

            string qs = new SalesforceQuerySession()
                .Select("Id, Name, CreatedDate, CreatedById, NamespacePrefix, LastModifiedDate, Body")
                .From("ApexClass")
                .Where("Name")
                .Like("CP%")
                .ToString();

            Console.WriteLine(qs);
        }

        
    }
}
