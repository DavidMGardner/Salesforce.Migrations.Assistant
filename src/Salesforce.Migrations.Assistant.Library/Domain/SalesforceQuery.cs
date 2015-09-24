using System;
using System.Collections.Generic;
using System.Text;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class SalesforceQuery
    {
        private string _query;
        private readonly List<string> _fieldList = new List<string>();
    
        private string _operator;
        private string _filter;
        private string _predicate;
        private string _source;
        private string _type;

        public string Type()
        {
            return _type;
        }

        public SalesforceQuery Select(string name)
        {
            _fieldList.Add(name);

            return this;
        }

        public SalesforceQuery Select(string[] names)
        {
            _fieldList.AddRange(names);

            return this;
        }

        public SalesforceQuery From(string type)
        {
            _type = type;
            _source = String.Format("from {0}", type);

            return this;
        }

        public SalesforceQuery Like(string value)
        {
            _operator = String.Format("like '{0}'", value);

            return this;
        }

        public SalesforceQuery Equals(string value)
        {
            _filter = String.Format("= '{0}' ", value);

            return this;
        }

        public SalesforceQuery GreaterThan(string value)
        {
            _filter = String.Format("> '{0}' ", value);

            return this;
        }

        public SalesforceQuery GreaterThanDateTime(string value)
        {
            _filter = String.Format("> {0} ", value);

            return this;
        }

        public SalesforceQuery LessThan(string value)
        {
            _filter = String.Format("< '{0}' ", value);

            return this;
        }

        public SalesforceQuery Where(string name)
        {
            _predicate = String.Format("where {0} ", name);

            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Select ")
                .Append(string.Join(",", _fieldList))
                .Append(" ")
                .Append(_source)
                .Append(" ")
                .Append(_predicate)
                .Append(_operator)
                .Append(_filter);

            return sb.ToString();
        }
    }
}