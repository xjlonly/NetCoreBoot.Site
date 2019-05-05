using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreBoot.IServices
{
    public interface IRolePermissionService
    {
        /// <summary>
        /// 通过角色主键获取菜单主键数组
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        int[] GetIdsByRoleId(int RoleId);
    }
}