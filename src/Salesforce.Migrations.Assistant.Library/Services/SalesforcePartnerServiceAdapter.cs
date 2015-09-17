using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Salesforce.Migrations.Assistant.Library.Partner.SforceService;

namespace Salesforce.Migrations.Assistant.Library.Services
{
    public class SalesforcePartnerServiceAdapter : ISalesforcePartnerServiceAdapter, IDisposable
    {
        private readonly SforceService _service;

        public SalesforcePartnerServiceAdapter(SforceService service)
        {
            _service = service;
        }

        public SalesforcePartnerServiceAdapter() : this(new SforceService()) {}

        public void Dispose()
        {
            _service.Dispose();
        }

        public string Url
        {
            get { return _service.Url; }
            set { _service.Url = value;}
        }

        public SessionHeader SessionHeader
        {
            get { return _service.SessionHeaderValue; }
            set { _service.SessionHeaderValue = value;}
        }

        public DescribeGlobalResult DescribeGlobal()
        {
            throw new NotImplementedException();
        }

        public GetUserInfoResult GetUserInfo()
        {
            throw new NotImplementedException();
        }

        public QueryResult Query(string query)
        {
            throw new NotImplementedException();
        }

        public QueryResult QueryMore(string queryLocator)
        {
            throw new NotImplementedException();
        }

        public LoginResult Login(string username, string password, string environmentUrl)
        {
            LoginResult loginResult = null;
            loginResult = _service.LoginAtEnvironment(username, password, environmentUrl);
            return loginResult;
        }

        public void SetupClientId(string clientId)
        {
            CallOptions callOptions = _service.CallOptionsValue ?? new CallOptions();
            callOptions.client = clientId;
            _service.CallOptionsValue = callOptions;
        }
    }

    static public class PartnerServiceExtensions
    {
        static public LoginResult LoginAtEnvironment(this SforceService svc, string username, string password, string environment)
        {
            string url = svc.Url;
            svc.Url = "https://" + environment + ".salesforce.com/services/Soap/u/29.0";
            LoginResult loginResult = svc.login(username, password);
            svc.Url = url;
            return loginResult;
        }
    }
}
