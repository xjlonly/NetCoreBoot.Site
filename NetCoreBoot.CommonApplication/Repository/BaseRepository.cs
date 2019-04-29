using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Dapper;

namespace NetCoreBoot.Core.Repository
{
    public class BaseRepository<T, Tkey> : IBaseRepository<T, Tkey> where T : class
    {
        protected IDbConnection _dbconnection;
        public int Delete(Tkey id) => _dbconnection.Delete(id);

        public int Delete(T entity) => _dbconnection.Delete(entity);

        public Task<int> DeleteAsync(Tkey id) => _dbconnection.DeleteAsync(id);

        public Task<int> DeleteAsync(T entity) => _dbconnection.DeleteAsync(entity);

        public int DeleteList(string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _dbconnection.DeleteList<T>(conditions, parameters, transaction, commandTimeout);
        }

        public async Task<int> DeleteListAsync(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await _dbconnection.DeleteListAsync<T>(whereConditions, transaction, commandTimeout);
        }

        public async Task<int> DeleteListAsync(string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await _dbconnection.DeleteListAsync<T>(conditions, parameters, transaction, commandTimeout);
        }


        public T Get(Tkey id) => _dbconnection.Get<T>(id);

        public async Task<T> GetAsync(Tkey id)
        {
            return await _dbconnection.GetAsync<T>(id);
        }

        public IEnumerable<T> GetList() => _dbconnection.GetList();

        public IEnumerable<T> GetList(object whereConditions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetList(string conditions, object parameters = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetListPaged(int pageNumber, int rowsPerpage, string conditions, string orderby, object parameters)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetListAsync(object whereConditions)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetListAsync(string conditions, object parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetListPagedAsync(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null)
        {
            throw new NotImplementedException();
        }

        public int? Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int?> InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public int RecordCount(string conditions = "", object parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> RecordCountAsync(string conditions = "", object parameters = null)
        {
            throw new NotImplementedException();
        }

        public int Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~BaseRepository() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
