using System;
using System.Collections.Generic;
using System.Text;
using NetCoreBoot.Common;
using System.Data;
using NetCoreBoot.Entity.Enums;

namespace NetCoreBoot.Core
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

            return null;
        }

        private DatabaseType GetDatabaseType(string dbType)
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
