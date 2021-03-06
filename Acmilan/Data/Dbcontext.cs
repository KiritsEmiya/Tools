﻿using System;
using System.Data.Common;

namespace Acmilan.Data
{
    public class DbContext : IDisposable
    {

        public static ISqlProvider provider;
        public static string defaultString = string.Empty;

        public DbContext()
        {
            Provider = provider;
            ConnString = defaultString;
        }

        public ISqlProvider Provider { get; set; }

        protected string ConnString { get; set; }

        private DbConnection _conn;
        public DbConnection Conn
        {
            get
            {
                if (_conn == null)
                {
                    _conn = Provider.Instance().CreateConnection();
                    _conn.ConnectionString = ConnString;
                    _conn.Open();
                }
                return _conn;
            }
            set
            {
                _conn = value;
            }
        }

        public void Dispose()
        {
            if (_conn != null)
            {
                _conn.Close();
                _conn.Dispose();
                _conn = null;
            }
        }

        private CompanyCollection _companyCollection;
        public CompanyCollection CompanyCollection
        {
            get
            {
                if (_companyCollection == null)
                    _companyCollection = new CompanyCollection(this);
                return _companyCollection;
            }
        }

        private ServicesCollection _servicesCollection;
        public ServicesCollection ServicesCollection
        {
            get
            {
                if (_servicesCollection == null)
                    _servicesCollection = new ServicesCollection(this);
                return _servicesCollection;
            }
        }

    }
}
