using System;
using System.Linq;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreBoot.Core.Repository
{
    public interface IBaseRepository<T, TKey> : IDisposable where T:class
    {
        /// <summary>
        /// Get the specified Id.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        T Get(TKey id);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns>The list.</returns>
        IEnumerable<T> GetList();

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns>The list.</returns>
        /// <param name="whereConditions">Where conditions.</param>
        IEnumerable<T> GetList(object whereConditions);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns>The list.</returns>
        /// <param name="conditions">Conditions.</param>
        /// <param name="parameters">Parameters.</param>
        IEnumerable<T> GetList(string conditions, object parameters = null);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns>The list.</returns>
        /// <param name="pageNumber">页码.</param>
        /// <param name="rowsPerpage">每页显示数据量.</param>
        /// <param name="conditions">Conditions.</param>
        /// <param name="orderby">Orderby.</param>
        /// <param name="parameters">Parameters.</param>
        IEnumerable<T> GetListPaged(int pageNumber, int rowsPerpage, string conditions, string orderby, object parameters);

        /// <summary>
        /// 插入一条数据 返回主键 表自增类型返回主键否则返回null
        /// </summary>
        /// <returns>The insert.</returns>
        /// <param name="entity">Entity.</param>
        int? Insert(T entity);

        /// <summary>
        /// Update the specified entity.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="entity">Entity.</param>
        int Update(T entity);

        /// <summary>
        /// Delete the specified id.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="id">Identifier.</param>
        int Delete(TKey id);

        /// <summary>
        /// Delete the specified entity.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="entity">Entity.</param>
        int Delete(T entity);

        /// <summary>
        /// Deletes the list.
        /// </summary>
        /// <returns>The list.</returns>
        /// <param name="conditions">where条件子句.</param>
        /// <param name="parameters">参数.</param>
        /// <param name="transaction">事务.</param>
        /// <param name="commandTimeout">Command timeout.</param>
        int DeleteList(string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Records the count.
        /// </summary>
        /// <returns>The count.</returns>
        /// <param name="conditions">Conditions.</param>
        /// <param name="parameters">Parameters.</param>
        int RecordCount(string conditions = "", object parameters = null);


        /// <summary>
        /// 通过主键获取实体对象
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        Task<T> GetAsync(TKey id);
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetListAsync();
        /// <summary>
        /// 执行具有条件的查询，并将结果映射到强类型列表
        /// </summary>
        /// <param name="whereConditions">条件</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetListAsync(object whereConditions);
        /// <summary>
        /// 带参数的查询满足条件的数据
        /// </summary>
        /// <param name="conditions">条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetListAsync(string conditions, object parameters = null);
        /// <summary>
        /// 使用where子句执行查询，并将结果映射到具有Paging的强类型List
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="rowsPerPage">每页显示数据</param>
        /// <param name="conditions">查询条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetListPagedAsync(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null);
        /// <summary>
        /// 插入一条记录并返回主键值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int?> InsertAsync(T entity);
        /// <summary>
        /// 更新一条数据并返回影响的行数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>影响的行数</returns>
        Task<int> UpdateAsync(T entity);
        /// <summary>
        /// 根据实体主键删除一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>影响的行数</returns>
        Task<int> DeleteAsync(TKey id);
        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回影响的行数</returns>
        Task<int> DeleteAsync(T entity);
        /// <summary>
        /// 条件删除多条记录
        /// </summary>
        /// <param name="whereConditions">条件</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>影响的行数</returns>
        Task<int> DeleteListAsync(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null);
        /// <summary>
        /// Deletes the list async.
        /// </summary>
        /// <returns>The list async.</returns>
        /// <param name="conditions">Conditions.</param>
        /// <param name="parameters">Parameters.</param>
        /// <param name="transaction">Transaction.</param>
        /// <param name="commandTimeout">Command timeout.</param>
        Task<int> DeleteListAsync(string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);
      
        /// <summary>
        /// Records the count async.
        /// </summary>
        /// <returns>The count async.</returns>
        /// <param name="conditions">Conditions.</param>
        /// <param name="parameters">Parameters.</param>
        Task<int> RecordCountAsync(string conditions = "", object parameters = null);
    }
}
