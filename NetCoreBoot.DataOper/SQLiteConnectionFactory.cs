using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Chloe.Infrastructure;

namespace NetCoreBoot.Data
{
    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public SQLiteConnectionFactory(string connString)
        {
            this._connString = connString;
        }

        IDbConnection IDbConnectionFactory.CreateConnection()
        {
            //SqliteConnection conn = new SqliteConnection(this._connString);
            //return conn;
            return null;
        }
    }
}
