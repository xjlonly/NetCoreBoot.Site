using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NetCoreBoot.IService;
using NetCoreBoot.Entity;
using NetCoreBoot.CommonApplication;
using NetCoreBoot.Common;

namespace NetCoreBoot.Service
{
    public class RoleAuthorizeService : BaseService, IRoleAuthorizeService
    {
        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <returns></returns>
        public List<Sys_RoleAuthorize> GetRoleAuthorizeList(string ObjectId)
        {
            return this.Query<Sys_RoleAuthorize>().Where(x => x.F_ObjectId == ObjectId).ToList();
        }

        public List<Sys_Module> GetMenuList(string roleId)
        {
            var data = new List<Sys_Module>();
            var moduledata = Query<Sys_Module>().OrderBy(x => x.F_SortCode).ToList();
            if (AdminUtils.GetCurrentCookie().IsAdmin)
            {
                data = moduledata;
            }
            else
            {
                //获得模块类型
                var authorizedata = Query<Sys_RoleAuthorize>().Where(x => x.F_ObjectId == roleId && x.F_ItemType == 1).ToList();
                authorizedata.ForEach(model =>
                {
                    Sys_Module item = moduledata.Find(x => x.F_Id == model.F_Id);
                    if (item != null)
                    {
                        data.Add(item);
                    }
                });
            }
            return data;
        }

        /// <summary>
        /// 获取角色/岗位列表
        /// </summary>
        /// <param name="Category">分类:1-角色2-岗位</param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Sys_Role> GetRoleList(int Category = 0, string keyword = "")
        {
            var expression = LinqExtension.True<Sys_Role>();
            if(keyword.NotNullOrEmpty())
            {
                expression = expression.And(x=>x.F_FullName.Contains(keyword.Trim()));
                expression = expression.Or(x=>x.F_EnCode.Contains(keyword.Trim()));
            }
            if(Category > 0)
            {
                expression = expression.And(x => x.F_Category == Category);
            }
            return Query<Sys_Role>().Where(expression).OrderBy(x => x.F_SortCode).ToList();
        }

        /// <summary>
        /// 基础数据列表
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public  List<Sys_ItemsDetail> GetItemDetailList(string itemId = "", string keyword = "")
        {
            var expression = LinqExtension.True<Sys_ItemsDetail>();
            if(itemId.NotNullOrEmpty())
            {
                expression = expression.And(x => x.F_ItemId == itemId); 
            }
            if(keyword.NotNullOrEmpty())
            {
                expression = expression.And(x => x.F_ItemName.Contains(keyword.Trim()));
                expression = expression.Or(x => x.F_ItemCode.Contains(keyword.Trim()));
            }
            return Query<Sys_ItemsDetail>().Where(expression).OrderBy(x => x.F_SortCode).ToList();
        }

        //获取菜单按钮列表
        public List<Sys_ModuleButton> GetButtonList(string roleId)
        {
            var data = new List<Sys_ModuleButton>();
            var modulebuttons = GetList<Sys_ModuleButton>().OrderBy(x => x.F_SortCode).ToList();
            if (AdminUtils.GetCurrentCookie().IsAdmin)
            {
                data = modulebuttons;
            }
            else
            {
                var authorizedata = Query<Sys_RoleAuthorize>().Where(x => x.F_ObjectId == roleId && x.F_ItemType == 2).ToList();
                foreach (var item in authorizedata)
                {
                    Sys_ModuleButton moduleButtonEntity = modulebuttons.Find(t => t.F_Id == item.F_ItemId);
                    if (moduleButtonEntity != null)
                    {
                        data.Add(moduleButtonEntity);
                    }
                }
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }
    }
}
