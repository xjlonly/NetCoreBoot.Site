using System.Data;
using Chloe.Infrastructure;
using MySql.Data.MySqlClient;

namespace NetCoreBoot.Data
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public MySqlConnectionFactory(string connString)
        {
            this._connString = connString;
        }

        IDbConnection IDbConnectionFactory.CreateConnection()
        {
            MySqlConnection conn = new MySqlConnection(this._connString);
            return conn;
        }
    }
}
