using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Salesforce.Migrations.Assistant.Library.Tooling.SforceService;
using Encoding = System.Text.Encoding;

namespace Salesforce.Migrations.Assistant.Library.Services
{
    public class SalesforceToolingServiceAdapter : ISalesforceToolingServiceAdapter, IDisposable
    {
        private readonly SforceServiceService _service;

        public SalesforceToolingServiceAdapter() : this(new SforceServiceService()) {}
        public SalesforceToolingServiceAdapter(SforceServiceService service)
        {
            _service = service;
        }

        public string Url { get; set; }
        public SessionHeader SessionHeader { get; set; }
        public ExecuteAnonymousResult ExecuteAnonymous(string apexToExecute)
        {
            return _service.executeAnonymous(apexToExecute);
        }

        public SaveResult[] Create(sObject[] sObjects)
        {
            return _service.create(sObjects);
        }

        public sObject[] Retrieve(string @select, string type, string[] ids)
        {
            return _service.retrieve(select, type, ids);
        }

        public DescribeGlobalResult DescribeGlobal()
        {
            return _service.describeGlobal();
        }

        public DescribeSObjectResult DescribeObject(string type)
        {
            return _service.describeSObject(type);
        }

        public QueryResult Query(string query)
        {
            return _service.query(query);
        }

        public SaveResult[] Update(sObject[] sObjects)
        {
            return _service.update(sObjects);
        }

        public DeleteResult[] Delete(string[] ids)
        {
            return _service.delete(ids);
        }

        public string GetLogBody(string id)
        {
            WebRequest webRequest = WebRequest.Create(this.Url.Replace(new Uri(this.Url).AbsolutePath, string.Empty) + "/services/data/v28.0/tooling/sobjects/ApexLog/" + id + "/Body/");
            webRequest.Headers.Add("Authorization", "Bearer " + this._service.SessionHeaderValue.sessionId);
            webRequest.Method = "GET";
            HttpWebResponse httpWebResponse = webRequest.GetResponse() as HttpWebResponse;
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                return string.Empty;
            using (Stream responseStream = httpWebResponse.GetResponseStream())
                return new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
        }

        public SaveResult[] StartLogging(string id)
        {
            return this._service.create((sObject[])new sObject[1]
            {
                new TraceFlag()
                {
                    ApexCode = "Debug",
                    ApexProfiling = "Debug",
                    Callout = "Debug",
                    Database = "Debug",
                    System = "Debug",
                    Validation = "Debug",
                    Visualforce = "Debug",
                    Workflow = "Debug",
                    ExpirationDate = new DateTime?(DateTime.Now.AddDays(1.0)),
                    TracedEntityId = id
                }
            });
        }

        public DeleteResult[] StopLogging(string id)
        {
            QueryResult queryResult = this.Query(string.Format("SELECT Id, CreatedById FROM TraceFlag WHERE TracedEntityId='{0}' AND CreatedById='{0}'", (object)id));
            if (queryResult.size > 0)
                return this.Delete(Enumerable.ToArray<string>(Enumerable.Select<TraceFlag, string>(Enumerable.OfType<TraceFlag>((IEnumerable)queryResult.records), (Func<TraceFlag, string>)(x => x.Id))));
            return new DeleteResult[0];
        }

        public void SetupClientId(string clientId)
        {
            CallOptions callOptions = this._service.CallOptionsValue ?? new CallOptions();
            callOptions.client = clientId;
            this._service.CallOptionsValue = callOptions;
        }

        public void Dispose()
        {
            _service.Dispose();
        }
    }
}
