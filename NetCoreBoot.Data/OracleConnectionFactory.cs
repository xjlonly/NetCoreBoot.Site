using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Chloe.Infrastructure;
using Chloe.Oracle;

namespace NetCoreBoot.Data
{
    public class OracleConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public OracleConnectionFactory(string connString)
        {
            this._connString = connString;
        }
        public IDbConnection CreateConnection()
        {
            return null;
        }
    }
}
