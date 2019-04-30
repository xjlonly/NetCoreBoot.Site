using NetCoreBoot.Core.Repository;
using NetCoreBoot.Entity;
using System;
using System.Threading.Tasks;

namespace NetCoreBoot.IRepository
{
    public partial interface IRolePermissionRepository : IBaseRepository<RolePermission, Int32>
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
        /// 通过角色主键获取菜单主键数组
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        int[] GetIdsByRoleId(int RoleId);
    }
}