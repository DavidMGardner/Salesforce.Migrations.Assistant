﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using Salesforce.Migrations.Assistant.Library.AsyncHelpers;
using Salesforce.Migrations.Assistant.Library.Exceptions;
using Salesforce.Migrations.Assistant.Library.MetaDataService;
using Salesforce.Migrations.Assistant.Library.Partner.SforceService;
using Salesforce.Migrations.Assistant.Library.Services;
using Serilog;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public enum SalesforceEnvironmentType
    {
        ProductionOrDeveloper,
        Sandbox,
        CustomDomain,
    }

    public class SalesforceContext
    {
        private string _userName;
        private string _password;
        private string _token;
        private SalesforceEnvironmentType _environmentType;
        private string _url;
        private readonly ISalesforcePartnerServiceAdapter _partnerServiceAdapter;
        private readonly ISalesforceToolingServiceAdapter _toolingServiceAdapter;
        private readonly ISalesforceMetadataServiceAdapter _metadataServiceAdapter;

        private string _sessionId;
        private bool _isLoggedIn;

        public IPollingResult PollingResult { get; }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            private set { _isLoggedIn = value; }
        }

        public static string ClientName { get; } = "Sogeti Migrations Assistant";

        public ISalesforceToolingServiceAdapter ToolingServiceAdapter
        {
            get
            {
                LoggedInGuard();
                return _toolingServiceAdapter;
            }
        }

        public ISalesforcePartnerServiceAdapter PartnerServiceAdapter
        {
            get
            {
                LoggedInGuard();
                return _partnerServiceAdapter;
            }
        }

        public ISalesforceMetadataServiceAdapter MetadataServiceAdapter
        {
            get
            {
                LoggedInGuard();
                return _metadataServiceAdapter;
            }
        }

        private void LoggedInGuard()
        {
            if (!IsLoggedIn)
            {
                throw new NotLoggedIntoSalesforceContextException();
            }
        }

        public SalesforceContext()
        {
            SalesforceEnvironmentType envtype;

            _partnerServiceAdapter = new SalesforcePartnerServiceAdapter();
            _toolingServiceAdapter = new SalesforceToolingServiceAdapter();
            _metadataServiceAdapter = new SalesforceMetadataServiceAdapter();

            PollingResult = new AsyncExtensions.PollingResultHelper();

            string username = ConfigurationManager.AppSettings["salesforce:context:username"];
            string password = ConfigurationManager.AppSettings["salesforce:context:password"];
            string token = ConfigurationManager.AppSettings["salesforce:context:token"];
            bool parsedEnvironment = Enum.TryParse(ConfigurationManager.AppSettings["salesforce:context:environment"], out envtype);

            if (parsedEnvironment && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(token))
            {
                Login(username, password, token, envtype);
            }
        }

        public bool LoginSandbox(string username, string password, string securityToken = null)
        {
            return Login(username, password, securityToken, SalesforceEnvironmentType.Sandbox);
        }

        public bool Login(string username, string password, string securityToken = null,
            SalesforceEnvironmentType environment = SalesforceEnvironmentType.CustomDomain, string environmentUrl = null)
        {
            string str = environment == SalesforceEnvironmentType.CustomDomain
                ? environmentUrl
                : GetEnvironment(environment);

            SaveUserCredentials(username, password, securityToken, environment, str);

            string passwordFormatted = password + (securityToken ?? string.Empty);

            LoginResult loginResult;
            try
            {
                _partnerServiceAdapter.SetupClientId(ClientName);
                loginResult = _partnerServiceAdapter.Login(username, passwordFormatted, str);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }

            if (loginResult == null || loginResult.passwordExpired)
                return false;

            _sessionId = loginResult.sessionId;
            IsLoggedIn = true;

            SetupToolingAdapter(loginResult.serverUrl);
            SetupMetadataAdapter(loginResult.metadataServerUrl);
            SetupPartneraAdapter(loginResult.serverUrl);
            return true;
        }

        private void SaveUserCredentials(string username, string password, string securityToken, SalesforceEnvironmentType environment, string domain)
        {
            _userName = username;
            _password = password;
            _token = securityToken;
            _environmentType = environment;
            _url = domain;
        }

        private string GetEnvironment(SalesforceEnvironmentType environmentType)
        {
            switch (environmentType)
            {
                case SalesforceEnvironmentType.Sandbox:
                    return "test";
                default:
                    return "login";
            }
        }

        private void SetupToolingAdapter(string serverUrl)
        {
            _toolingServiceAdapter.Url = serverUrl.Replace("/u/", "/T/");
            _toolingServiceAdapter.SessionHeader =
                new Salesforce.Migrations.Assistant.Library.Tooling.SforceService.SessionHeader { sessionId = _sessionId };

            _toolingServiceAdapter.SetupClientId(ClientName);
        }

        private void SetupMetadataAdapter(string serverUrl)
        {
            _metadataServiceAdapter.Url = serverUrl.Replace("/u/", "/m/");
            _metadataServiceAdapter.SessionHeader =
                new Salesforce.Migrations.Assistant.Library.MetaDataService.SessionHeader { sessionId = _sessionId };
            _metadataServiceAdapter.SetupClientId(ClientName);
        }

        private void SetupPartneraAdapter(string serverUrl)
        {
            _partnerServiceAdapter.Url = serverUrl;
            _partnerServiceAdapter.SessionHeader =
                new Salesforce.Migrations.Assistant.Library.Partner.SforceService.SessionHeader { sessionId = _sessionId };
            _partnerServiceAdapter.SetupClientId(ClientName);
        }

        private bool ReLogin()
        {
            return Login(_userName, _password, _token, _environmentType, _url);
        }

        public T SessionExpirationWrapper<T>(Func<T> adapterRequest)
        {
            try
            {
                return adapterRequest();
            }
            catch (SoapException ex)
            {
                if ((ex.Message.Contains("INVALID_SESSION_ID:") ||
                     ex.Message.Contains("UNKNOWN_EXCEPTION: Destination URL not reset.")) && this.ReLogin())
                    return adapterRequest();
                Log.Information(ex.Message);
                throw;
            }
        }

        public void WaitAllTaskskWithHandlingAggregateException<T>(Task<T>[] tasks, CancellationToken cancellationToken)
        {
            try
            {
                Task.WaitAll(tasks, cancellationToken);
            }
            catch (AggregateException ex)
            {
                foreach (Exception obj in ex.InnerExceptions)
                    Log.Error("TWS - Salesforce Core", obj.ToString());
                throw;
            }
        }

        public RetrieveResult CheckRetrieveResult(string id)
        {
            RetrieveResult retrieveResult = null;
            try
            {
                retrieveResult = SessionExpirationWrapper(() => _metadataServiceAdapter.CheckRetrieveStatus(id, true));
            }
            catch (SoapException ex)
            {
                if (ex.Message != "INVALID_ID_FIELD: Deployment still in process: InProgress")
                {
                    if (ex.Message != "INVALID_ID_FIELD: Deployment still in process: Queued")
                    {
                        Log.Error(ex.Message);
                        throw;
                    }
                }
            }
            return retrieveResult;
        }
    }
}