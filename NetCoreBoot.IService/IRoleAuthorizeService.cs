using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.Entity;

namespace NetCoreBoot.IService
{
    public interface IRoleAuthorizeService : IBaseService
    {
        
        //获取角色/岗位列表
        List<Sys_Role> GetRoleList(int Category = 0, string keyword="");

        //获取角色权限列表
        List<Sys_RoleAuthorize> GetRoleAuthorizeList(string ObjectId);

        //获取菜单列表
        List<Sys_Module> GetMenuList(string roleId);

        //获取基础信息详情列表
        List<Sys_ItemsDetail> GetItemDetailList(string itemId = "", string keyword = "");

        //获取模块按钮列表
        List<Sys_ModuleButton> GetButtonList(string roleId);

    }
}
