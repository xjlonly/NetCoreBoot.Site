using NetCoreBoot.Core.Repository;
using NetCoreBoot.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreBoot.IRepository
{
    public partial interface ITaskInfoRepository : IBaseRepository<TaskInfo, Int32>
    {
	     /// <summary>
        /// 逻辑删除返回影响的行数
        /// </summary>
        /// <param name="ids">需要删除的主键数组</param>
        /// <returns>影响的行数</returns>
        Int32 DeleteLogical(Int32[] ids);
        /// <summary>
        /// 逻辑删除返回影响的行数（异步操作）
        /// </summary>
        /// <param name="ids">需要删除的主键数组</param>
        /// <returns>影响的行数</returns>
        Task<Int32> DeleteLogicalAsync(Int32[] ids);

        /// <summary>
        /// 应用程序停止时暂停所有任务
        /// </summary>
        /// <returns></returns>
        Task<bool> SystemStoppedAsync();

        /// <summary>
        /// 应用程序启动时启动所有任务
        /// </summary>
        /// <returns></returns>
        Task<bool> ResumeSystemStoppedAsync();

        /// <summary>
        /// 根据ids更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> UpdateStatusByIdsAsync(Int32[] ids,int Status);

        /// <summary>
        /// 根据状态获取任务列表
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        Task<List<TaskInfo>> GetListByJobStatuAsync(int Status);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="Name">别名</param>
        /// <returns></returns>
        Task<Boolean> IsExistsNameAsync(string Name);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="Name">别名</param>
        /// <param name="Id">主键</param>
        /// <returns></returns>
        Task<Boolean> IsExistsNameAsync(string Name, Int32 Id);
    }
}