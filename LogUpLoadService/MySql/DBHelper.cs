using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using ZJH.Base.Util;
using Common;
using Common.DBUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace LogUpLoadService
{
    public class DBHelper
    {
        #region
        /// <summary>
        /// 日志访问器
        /// </summary>
        protected static Log5 Logger = new Log5();
        public static string connectionString
        {
            get
            {
                //return "server=192.168.1.207;Port=3306;database=ymdata1;uid=ym;pwd=123qwe ";
                //return "server=10.1.130.11;Port=3308;database=ymzjh;uid=zjh_r;pwd=123456;Allow User Variables=True";
                return PubConstant.MysqlConnectionString;
            }
        }

        public static int CommandTimeout = PubConstant.CommandTimeout;
        #endregion

        public bool Add<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
                return false;
            string sql = string.Empty;
            Type typeinfo = typeof(T);
            string tableName = ModelClassHelper.GetMappingTableName(typeinfo);
            var columnInfos = ModelClassHelper.GetColumns(typeinfo);
            List<string> tmp = new List<string>();
            var columns = String.Join(",", columnInfos.Where(n => n.IsAutoGen == false).Select(m => "`" + m.ColumnName + "`"));
            var values = string.Join(",", columnInfos.Where(n => n.IsAutoGen == false).Select(p => "@" + p.ColumnName));
            sql = "Insert Into " + tableName + "(" + columns + ") Values(" + values + ");";
            var autoGenCol = columnInfos.Where(m => m.IsAutoGen).FirstOrDefault();

            /* 有标识列，取得标识列值，赋值到Entity */
            if (autoGenCol != null)
            {
                sql += "SELECT LAST_INSERT_ID() as KeyId;";
            }
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var ret = connection.ExecuteScalar(sql, entity, transaction, commandTimeout);//todo jiaxl测试没有自增的字段
                    if (ret == null)
                        return false;
                    object KeyId = (object)ret;
                    autoGenCol.Property.SetValue(entity, Common.CommonTools.ConvertObject(KeyId, autoGenCol.Property.PropertyType), null);
                    connection.Close();
                    return true;
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                    string strEnty = entity == null ? string.Empty : JsonHelper.GetJson(entity);
                    Logger.Error($"Message:{ex.Message}\r\nsql:{sql}  \r\nparam:{strEnty}");
                    return false;
                }
            }
        }

        public virtual bool Update<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
                return false;
            string sql = string.Empty;
            Type typeinfo = typeof(T);
            var key = ModelClassHelper.GetKeyColumns(typeinfo);
            string tableName = ModelClassHelper.GetMappingTableName(typeinfo);
            var columnInfos = ModelClassHelper.GetColumns(typeinfo);
            if (key != null && key.Length > 0)
            {
                var updateFields = string.Join(",", columnInfos.Where(m => m.IsKey == false).Select(p => "`" + p.ColumnName + "`" + " = @" + p.ColumnName));

                var wheresql = string.Format(" WHERE {0} = @{0} ", key.FirstOrDefault().ColumnName);
                sql = string.Format("UPDATE {0} SET {1}{2} ", tableName, updateFields, wheresql);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        var ret = connection.Execute(sql, entity, transaction, commandTimeout);
                        return ret >= 1;
                    }
                    catch (MySqlException ex)
                    {
                        string strEnty = entity == null ? string.Empty : JsonHelper.GetJson(entity);
                        Logger.Error($"Message:{ex.Message}\r\nsql:{sql} \r\nparam:{strEnty}");
                        return false;
                    }
                }
            }

            else
            {
                Logger.Error(string.Format("表{0}没有主键，修改失败", tableName));
                return false;
            }
        }

        public virtual bool Delete<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
                return false;
            string sql = string.Empty;
            Type typeinfo = typeof(T);
            var key = ModelClassHelper.GetKeyColumns(typeinfo);
            string tableName = ModelClassHelper.GetMappingTableName(typeinfo);
            var columnInfos = ModelClassHelper.GetColumns(typeinfo);
            if (key != null && key.Length > 0)
            {
                var updateFields = string.Join(",", columnInfos.Where(m => m.IsKey == false).Select(p => p + " = @" + p));
                var wheresql = string.Format(" WHERE {0} = @{0} ", key.FirstOrDefault().ColumnName);
                sql = string.Format("DELETE FROM {0} {1} ", tableName, wheresql);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        var ret = connection.Execute(sql, entity, transaction, commandTimeout);
                        return ret >= 1;
                    }
                    catch (MySqlException ex)
                    {
                        connection.Close();
                        string strEnty = entity == null ? string.Empty : JsonHelper.GetJson(entity);
                        Logger.Error($"Message:{ex.Message}\r\nsql:{sql} \r\nparam:{strEnty}");
                        return false;
                    }
                }
            }
            else
            {
                Logger.Error(string.Format("表{0}没有主键，修改失败", tableName));
                return false;
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>
        public virtual int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        MySqlTransaction tx = conn.BeginTransaction();
                        cmd.Transaction = tx;
                        try
                        {
                            int count = 0;
                            for (int n = 0; n < SQLStringList.Count; n++)
                            {
                                string strsql = SQLStringList[n];
                                if (strsql.Trim().Length > 1)
                                {
                                    cmd.CommandText = strsql;
                                    count += cmd.ExecuteNonQuery();
                                }
                            }
                            tx.Commit();
                            return count;
                        }
                        catch
                        {
                            tx.Rollback();
                            return 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行带一个参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public virtual int ExecuteSql(string sql, object param = null, IDbTransaction trans = null)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    int rows = connection.Execute(sql, param, trans);
                    connection.Close();
                    return rows;
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                    string strEnty = param == null ? string.Empty : JsonHelper.GetJson(param);
                    Logger.Error($"Message:{ex.Message}\r\nsql:{sql} \r\nparam:{strEnty}");
                    return 0;
                }
            }
        }

        /// <summary>
        /// 执行SQL语句,返回第一个结果的Int值。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>第一个结果的Int值</returns>
        public virtual int ExecuteSqlGetFirstInt(string SQLString, object param = null, IDbTransaction trans = null)
        {
            int count = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var result = connection.ExecuteScalar(SQLString, param, trans);
                    connection.Close();
                    if (result != null && result != DBNull.Value)
                    {
                        int.TryParse(result.ToString(), out count);
                    }
                    return count;
                }
                catch (MySqlException e)
                {
                    connection.Close();
                    Logger.Error(e.Message + "\r\nsql:" + SQLString);
                    return count;
                }
            }
        }

        /// <summary>
        /// 查询操作返回默认第一条数据(如返回null则创建默认类型)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SQLString"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual T ExecuteSqlGetFirst<T>(string sql, object param = null, IDbTransaction trans = null)
        {

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var result = connection.QueryFirstOrDefault<T>(sql, param, trans);
                    connection.Close();
                    return result;
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                    string strEnty = param == null ? string.Empty : JsonHelper.GetJson(param);
                    Logger.Error($"Message:{ex.Message}\r\nsql:{sql} \r\nparam:{strEnty}");
                    return default(T);
                }
            }
        }

        /// <summary>
		/// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
		/// </summary>
		/// <param name="strSQL">SQL语句</param>
		/// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
		/// <returns>影响的记录数</returns>
		public virtual int ExecuteSqlInsertImg(string sql, byte[] fs)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                MySqlParameter myParameter = new MySqlParameter("@content", SqlDbType.Image)
                {
                    Value = fs
                };
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (MySqlException ex)
                {
                    Logger.Error($"Message:{ex.Message}\r\nsql:{sql}");
                    return 0;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        public virtual object GetSingle(string sql, object param = null)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        object result = conn.ExecuteScalar(sql, param);
                        return result;
                    }
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                    string strEnty = param == null ? string.Empty : JsonHelper.GetJson(param);
                    Logger.Error($"Message:{ex.Message}\r\nsql:{sql} \r\nparam:{strEnty}");
                    return default(object);
                }
            }
        }

        /// <summary>
		/// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
		/// </summary>
		/// <param name="strSQL">查询语句</param>
		/// <returns>SqlDataReader</returns>
		public virtual MySqlDataReader ExecuteReader(string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandText = sql;
                        MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        return myReader;
                    }
                    catch (MySqlException ex)
                    {
                        connection.Close();
                        Logger.Error($"Message:{ex.Message}\r\nsql:{sql}");
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
		/// 执行查询语句，返回DataSet
		/// </summary>
		/// <param name="SQLString">查询语句</param>
		/// <returns>DataSet</returns>
		public virtual DataSet Query(string sql, string ConnectString = "", int CommandTimeout = 0)
        {
            var connString = string.IsNullOrWhiteSpace(ConnectString) ? connectionString : ConnectString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                DataSet ds = new DataSet();
                try
                {
                    conn.Open();
                    using (MySqlDataAdapter command = new MySqlDataAdapter(sql, conn))
                    {
                        command.SelectCommand.CommandTimeout = CommandTimeout > 0 ? CommandTimeout : CommandTimeout;
                        command.Fill(ds, "ds");
                    }
                }
                catch (MySqlException ex)
                {
                    conn.Close();
                    Logger.Error($"Message:{ex.Message}\r\nsql:{sql}");
                    return null;
                }
                return ds;
            }
        }

        public virtual List<T> Query<T>(string sql, object param = null, string ConnectString = "", int CommandTimeout = 0)
        {

            var connString = string.IsNullOrWhiteSpace(ConnectString) ? connectionString : ConnectString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    var result = conn.Query<T>(sql, param,null,true, CommandTimeout);
                    return result.ToList<T>();
                }
                catch (MySqlException ex)
                {
                    string strEnty = param == null ? string.Empty : JsonHelper.GetJson(param);
                    Logger.Error($"Message:{ex.Message}\r\nsql:{sql} \r\nparam:{strEnty}");
                    return null;
                }
            }
        }
        /// <summary>
        /// 返回第一行第一列结果 (使用临时变量时使用)
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="ConnectString"></param>
        /// <param name="CommandTimeout"></param>
        /// <returns></returns>
        public virtual object QueryWithOutDapper(string sql, string ConnectString = "", int CommandTimeout = 0)
        {
            var connString = string.IsNullOrWhiteSpace(ConnectString) ? connectionString : ConnectString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    if (CommandTimeout > 0)
                    {
                        cmd.CommandTimeout = CommandTimeout;
                    }
                    return cmd.ExecuteScalar();
                }
                catch (MySqlException ex)
                {
                    Logger.Error($"Message:{ex.Message}\r\nsql:{sql}");
                    return null;
                }
            }
        }



        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public virtual DataSet Query(string sql, object param = null, string ConnectString = "", int CommandTimeout = 0)
        {
            var connString = string.IsNullOrWhiteSpace(ConnectString) ? connectionString : ConnectString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                DataSet ds = new DataSet();
                try
                {
                    conn.Open();
                    using (MySqlDataAdapter command = new MySqlDataAdapter(sql, conn))
                    {
                        command.SelectCommand.CommandTimeout = CommandTimeout > 0 ? CommandTimeout : CommandTimeout;
                        command.Fill(ds, "ds");
                    }
                }
                catch (MySqlException ex)
                {
                    conn.Close();
                    string strEnty = param == null ? string.Empty : JsonHelper.GetJson(param);
                    Logger.Error($"Message:{ex.Message}\r\nsql:{sql} \r\nparam:{strEnty}");
                    return null;
                }
                return ds;
            }
        }
        public virtual IEnumerable<T> Query<T>(IDictionary<string, object> whereKeys, string orderBy = null, bool slave = true, int startIndex = 0, int pageSize = 0, int count = int.MaxValue)
        {
            string sql = string.Empty;
            Type typeinfo = typeof(T);
            string tableName = ModelClassHelper.GetMappingTableName(typeinfo);
            var columns = ModelClassHelper.GetColumns(typeinfo);

            var colNames = string.Empty;
            columns.ForEach((col) =>
                {
                    if (col.IsNotMaped)
                        return;
                    if (!string.IsNullOrEmpty(colNames))
                        colNames += ",";
                    colNames += "`" + col.ColumnName + "`";
                });

            var paramList = new DynamicParameters();
            var where = string.Empty;

            IParameterMapper paramMapper = new GeneralParameterMapper();
            if (whereKeys != null)
            {
                //paramList = new MySqlParameter[whereKeys.Count];
                int index = 0;
                foreach (var v in whereKeys)
                {
                    var column = columns.Where(m => m.Property.Name.Equals(v.Key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (column == null)
                    {
#if DEBUG
                        throw new Exception($"表{tableName}中不存在{v.Key}");
#else
						continue;
#endif
                    }
                    var colname = column.ColumnName;

                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";
                    if (v.Value == null)
                    {
                        where += $"{colname} IS NULL ";
                    }
                    else
                    {
                        var dbParam = "@" + colname;
                        where += $"{colname}=@{colname}"; //username=@username
                        paramList.Add(colname, v.Value);
                        //paramList[index] = (new MySqlParameter() { DbType = column.DbType, ParameterName = dbParam, Value = v.Value });
                        index++;
                    }
                }
            }

            sql = $"SELECT {colNames} from {tableName} ";
            if (string.IsNullOrEmpty(orderBy))
                orderBy = MysqlUnity.GetDefaultOrder<T>("desc");
            if (!string.IsNullOrEmpty(where)) sql += " where " + where;
            if (!string.IsNullOrEmpty(orderBy) && count == int.MaxValue) sql += " Order By " + orderBy;
            if (startIndex > 0 && pageSize > 0)
            {
                sql += $" LIMIT  {(startIndex - 1) * pageSize},{pageSize} ";
            }

            else if (count != int.MaxValue && count > 0)
            {
                sql += $" LIMIT  {count}";
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var result = conn.Query<T>(sql, paramList);
                    return result;
                }
            }
            catch (MySqlException ex)
            {
                Logger.Error($"Message:{ex.Message}\r\nsql:{sql} \r\ntableName:{tableName}");
                return null;
            }
        }
        /// <summary>
        /// 获取第一条记录
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual T GetEntity<T>(IDictionary<string, object> filter)
        {
            IEnumerable<T> enumerable = Query<T>(filter);
            var array = enumerable as T[] ?? enumerable.ToArray();
            if (array != null && array.ToList().Count > 0)
            {
                return array.FirstOrDefault();
            }
            return default(T);
        }

        public virtual int GetMaxID(string FieldName, string TableName)
        {
            string strsql = String.Format("select max({0})+1 from {1}", FieldName, TableName);
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 根据Id查找实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T GetById<T>(object Id)
        {
            Type typeinfo = typeof(T);
            var keyCols = ModelClassHelper.GetKeyColumns(typeinfo);
            if (keyCols.Length > 1)
            {
                throw new Exception("表中包含多个主键");
            }
            else if (keyCols.Length == 0)
            {
                throw new Exception("没有定义主键");
            }
            var col = keyCols.First();
            var where = new Dictionary<string, object>();
            where.Add(col.ColumnName, Id);
            var result = Query<T>(where);
            if (result != null)
                return result.FirstOrDefault();
            else
                return default(T);

        }
        public virtual int RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                int result;
                connection.Open();
                MySqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }
        /// <summary>
        /// 执行存储过程，返回影响的行数
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public virtual int ExcuteProcedure(string storedProcName, DynamicParameters parameters = null)
        {
            int result = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var ts = connection.Query(storedProcName, parameters, null, true, null, CommandType.StoredProcedure);

                }
                catch (MySqlException ex)
                {
                    Logger.Error(ex.Message);
                    return 0;
                }
                //Connection.Close();
                return result;
            }
        }
        public static DataSet RunProcedure(string proc, IDataParameter[] param, string tn, int page, int pagesize, out int pagecount)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                using (MySqlDataAdapter sqlDA = new MySqlDataAdapter()
                {
                    SelectCommand = BuildQueryCommand(connection, proc, param)
                })
                {
                    sqlDA.Fill(dataSet, page * pagesize, pagesize, tn);
                    connection.Close();
                    using (DataSet countds = new DataSet())
                    {
                        int fillcount = sqlDA.Fill(countds, "tempcountrecordstable");
                        if (fillcount % pagesize == 0)
                            pagecount = fillcount / pagesize;
                        else
                            pagecount = fillcount / pagesize + 1;
                    }
                }
                return dataSet;
            }
        }


        #region 私有方法
        private static MySqlCommand BuildIntCommand(MySqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            MySqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);

            command.Parameters.Add(new MySqlParameter("ReturnValue",
                MySqlDbType.Int32, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }

        private static void Connection_InfoMessage(object sender, MySqlInfoMessageEventArgs e)
        {
            var msg = new System.Text.StringBuilder();
            msg.AppendLine(e.errors.FirstOrDefault().Message);
            msg.AppendLine("Detail:");
            if (e.errors != null && e.errors.Length > 0)
            {
                foreach (MySqlError error in e.errors)
                {
                    msg.Append($"{error.Message} AT {error.Message} {error.Code}: {error.Level}\r\n");
                }
            }
            Logger.Info(msg.ToString());
        }

        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static MySqlCommand BuildQueryCommand(MySqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            MySqlCommand command = new MySqlCommand(storedProcName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parameters != null)
            {
                foreach (MySqlParameter parameter in parameters)
                {
                    if (parameter != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                }
            }

            return command;
        }


        public TransactionScope BeginTrans(System.Data.IsolationLevel level = System.Data.IsolationLevel.Unspecified)
        {
            var tmp = LogicalThreadContext.GetData(connectionString);
            if (tmp != null)
            {
                throw new Exception("当前线程已经启动了一个事物。事物不允许嵌套。");
            }
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            var tran = conn.BeginTransaction();
            var scope = new TransactionScope(tran, connectionString);
            LogicalThreadContext.SetData(connectionString, scope);
            return scope;
        }


        public virtual bool IsExist<T>(IDictionary<string, object> whereDict, bool slave = true)
        {
            string sql = string.Empty;
            Type typeinfo = typeof(T);
            string tableName = ModelClassHelper.GetMappingTableName(typeinfo);
            var columns = ModelClassHelper.GetColumns(typeinfo);
            var keyCols = ModelClassHelper.GetKeyColumns(typeinfo);
            if (keyCols.Length > 1)
            {
                throw new Exception("表中包含多个主键");
            }
            var where = string.Empty;
            var paramList = new DynamicParameters();
            sql = "select " + keyCols.FirstOrDefault().ColumnName + " > 0 from " + tableName + "{0}   limit 1 ";
            if (whereDict != null)
            {
                int index = 0;
                foreach (var v in whereDict)
                {
                    var column = columns.Where(m => m.Property.Name.Equals(v.Key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (column == null)
                    {
#if DEBUG
                        throw new Exception($"表{tableName}中不存在{v.Key}");
#else
							continue;
#endif
                    }
                    var colname = column.ColumnName;

                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";
                    if (v.Value == null)
                    {
                        where += $"{colname} IS NULL ";
                    }
                    else
                    {
                        var dbParam = "@" + colname;
                        where += $"{colname}=@{colname}";
                        paramList.Add(colname, v.Value);
                        index++;
                    }
                }
            }
            sql = string.Format(sql, " where " + where);
            var ret = ExecuteSqlGetFirstInt(sql, paramList);
            return ret > 0;

        }
        #endregion


        public virtual int GetCount<T>(IDictionary<string, object> whereKeys, bool slave = true)
        {
            string sql = string.Empty;
            Type typeinfo = typeof(T);
            string tableName = ModelClassHelper.GetMappingTableName(typeinfo);
            var columns = ModelClassHelper.GetColumns(typeinfo);

            var paramList = new DynamicParameters();
            var where = string.Empty;

            IParameterMapper paramMapper = new GeneralParameterMapper();
            if (whereKeys != null)
            {
                //paramList = new MySqlParameter[whereKeys.Count];
                int index = 0;
                foreach (var v in whereKeys)
                {
                    var column = columns.Where(m => m.Property.Name.Equals(v.Key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (column == null)
                    {
#if DEBUG
                        throw new Exception($"表{tableName}中不存在{v.Key}");
#else
						continue;
#endif
                    }
                    var colname = column.ColumnName;

                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";
                    if (v.Value == null)
                    {
                        where += $"{colname} IS NULL ";
                    }
                    else
                    {
                        var dbParam = "@" + colname;
                        where += $"{colname}=@{colname}"; //username=@username
                        paramList.Add(colname, v.Value);
                        //paramList[index] = (new MySqlParameter() { DbType = column.DbType, ParameterName = dbParam, Value = v.Value });
                        index++;
                    }
                }
            }

            sql = $"SELECT COUNT(0) from {tableName} ";
            if (!string.IsNullOrEmpty(where)) sql += " where " + where;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var result = conn.ExecuteScalar<int>(sql, paramList);
                    return result;
                }
            }
            catch (MySqlException ex)
            {
                Logger.Error($"Message:{ex.Message}\r\nsql:{sql} \r\ntableName:{tableName}");
                return -1;
            }
        }

    }
}
