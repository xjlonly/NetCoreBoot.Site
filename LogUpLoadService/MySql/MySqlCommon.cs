using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using ZJH.Base.Util;
using MySql.Data.MySqlClient;
using System.Data;

namespace LogUpLoadService
{
    public class GeneralParameterMapper : IParameterMapper
    {
        private Action<DbCommand, object[]> _act;

        /// <summary>
        /// GeneralParameterMapper
        /// </summary>
        /// <param name="act">
        /// 定义如何将parameterValues赋给DbCommand的委托
        /// 如果不传，将使用默认委托，该委托不校验要传递的parameterValues在DbCommand是否已定义
        /// 此时反复执行Execute将会导致异常，以保证不会因为编码问题导致反复查询</param>
        public GeneralParameterMapper(Action<DbCommand, object[]> act = null)
        {
            if (act != null) { this._act = act; }
            else
            {
                this._act = (cmd, paramters) =>
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(paramters.Where(m => m != null).ToArray());
                };
            }
        }

        #region IParameterMapper 成员

        /// <summary>        /// IParameterMapper.AssignParameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterValues"></param>
        public void AssignParameters(DbCommand command, object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                this._act(command, parameterValues);
            }
        }

        #endregion
    }

    public static class MysqlUnity
    {
        public static string GetDefaultOrder<T>(string firstColDirection)
        {
            var cols = ModelClassHelper.GetKeyColumns(typeof(T));
            if (cols != null && cols.Length > 0)
            {
                var result = string.Empty;
                foreach (var c in cols)
                {
                    if (string.IsNullOrEmpty(result))
                        result += $"{c.ColumnName} {firstColDirection}";
                    else
                        result += $", {c.ColumnName}";
                }
                return result;
            }
            else
                return null;
        }
    }

    public class TransactionScope : IDisposable
    {
        private DbTransaction _trans = null;
        private bool _isCommited = false;
        private bool _isRollBack = false;
        private string _connStr = null;

        public DbTransaction Transaction
        {
            get
            {
                return _trans;
            }
        }

        public TransactionScope(DbTransaction trans, string connStr)
        {
            this._trans = trans;
            this._connStr = connStr;
        }

        public void Commit()
        {
            if (_trans != null && !_isRollBack && !_isCommited)
            {
                _trans.Commit();
                if (_connStr != null)
                    LogicalThreadContext.FreeNamedDataSlot(_connStr);
                _isCommited = true;
            }
        }
        public DbConnection GetConnection()
        {
            if (_trans != null)
                return _trans.Connection;
            else
                return null;
        }
        public void Rollback()
        {
            if (!_isCommited && !_isRollBack && _trans != null)
            {
                _trans.Rollback();
                if (_connStr != null)
                    LogicalThreadContext.FreeNamedDataSlot(_connStr);
                _isRollBack = true;
            }
        }

        public void Dispose()
        {

            if (_trans != null)
            {
                if (_trans.Connection != null)
                {
                    _trans.Connection.Close();

                    if (_trans.Connection != null)
                        _trans.Connection.Dispose();
                }
            }
        }
    }

}
