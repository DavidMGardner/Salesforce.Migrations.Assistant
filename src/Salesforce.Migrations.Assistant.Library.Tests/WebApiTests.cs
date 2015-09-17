using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Salesforce.Migrations.Assistant.Library.Tests.SforceServiceReference;

namespace Salesforce.Migrations.Assistant.Library.Tests
{
    [TestClass]
    public class WebApiTests
    {
        public double ApiVersion => 29.0;

        [TestMethod]
        public void TestMethod1()
        {
            Boolean success = false;
            string username = "dave.gardner@us.sogeti.com";
            string password = "@Tngds10!xoWNt5SPFiT0RZWo0QBQHqz8";

            // Create a service object 
            var binding = new SforceServiceReference.SoapClient();
            var result = binding.login(null, username, password);

            //MetadataService msBinding = new MetadataService
            //{
            //    SessionHeaderValue = new SessionHeader
            //    {
            //        sessionId = result.sessionId
            //    }
            //};


            //var query = msBinding.listMetadata(new ListMetadataQuery[]
            //{
            //    new ListMetadataQuery()
            //    {
            //        type = "ApexClass",
            //        folder = "Classes"
            //    }
            //},
            //ApiVersion);
            
            
            //MetaDataServiceReference.FileProperties[] fileProperties = MetaDataServiceReference.listMetadata(queries, 25);

            //var metaDataServiceUrl = result.metadataServerUrl;

            //var svc = new MetaDataServiceReference.ListMetadataQuery {type = "Workflow"};
            //queries.add(queryWorkflow);


        }
    }
}
