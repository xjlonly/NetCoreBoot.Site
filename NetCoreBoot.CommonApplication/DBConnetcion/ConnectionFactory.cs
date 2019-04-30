using System;
using System.Collections.Generic;
using System.Text;
using NetCoreBoot.Common;
using System.Data;
using NetCoreBoot.Entity.Enums;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Npgsql;

namespace NetCoreBoot.Core.DBConnection
{
    /// <summary>
    /// 数据库连接工厂类
    /// </summary>
    public class ConnectionFactory
    {
        public  static IDbConnection CreateConnection(string dbType, string connString)
        {
            if (dbType.IsEmptyOrNull())
            {
                throw new ArgumentNullException("获取数据连接时获取不到数据库类型!");
            }

            if (connString.IsEmptyOrNull())
            {
                throw new ArgumentNullException("获取数据连接时连接字符串为空!");
            }
            DatabaseType databaseType = GetDatabaseType(dbType);
            return CreateConnection(databaseType, connString);
        }

        private static IDbConnection CreateConnection(DatabaseType databaseType, string connString)
        {
            if (connString.IsEmptyOrNull())
            {
                throw new ArgumentException("数据库连接串为空！");
            }
            IDbConnection connection = null;
            switch (databaseType)
            {
                case DatabaseType.MySql:
                    connection = new MySqlConnection(connString);
                    break;
                case DatabaseType.SqlServer:
                    connection = new SqlConnection(connString);
                    break;
                case DatabaseType.PostgerSql:
                    connection = new NpgsqlConnection(connString);
                    break;
                default:
                    throw new ArgumentException($"不支持的{databaseType.ToString()}数据库类型");

            }
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }

        private static DatabaseType  GetDatabaseType(string dbType)
        {
            if (dbType.IsEmptyOrNull())
            {
                throw new ArgumentException("创建数据连接时数据库类型为空");
            }

            DatabaseType returnValue = DatabaseType.MySql;
            foreach (DatabaseType databasetype in Enum.GetValues(typeof(DatabaseType)))
            {
                if (dbType.Equals(databasetype.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    returnValue = databasetype;
                    break;
                }
            }

            return returnValue;
        }


    }

}
