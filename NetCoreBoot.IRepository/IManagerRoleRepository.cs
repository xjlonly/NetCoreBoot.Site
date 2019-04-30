using NetCoreBoot.Core.Repository;
using NetCoreBoot.Entity;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NetCoreBoot.IRepository
{
    public partial interface IManagerRoleRepository : IBaseRepository<ManagerRole, Int32>
    {
        /// <summary>
        /// 逻辑删除返回影响的行数
        /// </summary>
        /// <param name="ids">需要删除的主键数组</param>
        /// <returns>影响的行数</returns>
        int DeleteLogical(int[] ids);
        /// <summary>
        /// 逻辑删除返回影响的行数（异步操作）
        /// </summary>
        /// <param name="ids">需要删除的主键数组</param>
        /// <returns>影响的行数</returns>
        Task<int> DeleteLogicalAsync(int[] ids);

        /// <summary>
        /// 根据主键获取名称
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>名称</returns>
        string GetNameById(int id);
        /// <summary>
        /// 根据主键获取名称（异步操作）
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>名称</returns>
        Task<string> GetNameByIdAsync(int id);

        /// <summary>
        /// 事务新增,并保存关联表数据
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        int? InsertByTrans(ManagerRole model);

        /// <summary>
        /// 事务修改，并保存关联表数据
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        int UpdateByTrans(ManagerRole model);

        /// <summary>
        /// 通过角色ID获取角色分配的菜单列表
        /// </summary>
        /// <param name="roleId">角色主键</param>
        /// <returns></returns>
        List<Menu> GetMenusByRoleId(int roleId);
    }
}