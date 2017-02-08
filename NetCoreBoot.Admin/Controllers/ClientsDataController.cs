using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NetCoreBoot.CommonApplication;
using NetCoreBoot.Entity;
using NetCoreBoot.Common;
using NetCoreBoot.IService;


namespace NetCoreBoot.Admin.Controllers
{
    public class ClientsDataController : WebController
    {
        private IRoleAuthorizeService authorizeService { get; set; }
        public ClientsDataController(IRoleAuthorizeService authorService)
        {
            this.authorizeService = authorService;
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult GetClientsDataJson()
        {
            var data = new 
            {
                dataItems = this.GetDataItemList(),
                organize = this.GetOrganizeList(),
                role = this.GetRoleList(),
                duty = this.GetDutyList(),
                user = "",
                authorizeMenu = this.GetMenuList(),
                authorizeButton = this.GetMenuButtonList(),
            };
            return View();
        }

        //获取菜单列表
        private object GetMenuList()
        {
            var roleId = AdminUtils.GetCurrentCookie().RoleId;
            return ToMenuJson(authorizeService.GetMenuList(roleId), "0");
        }
        
        private string ToMenuJson(List<Sys_Module> data, string parentId)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            List<Sys_Module> entitys = data.FindAll(x => x.F_ParentId == parentId);
            entitys?.ForEach(model =>
            {
                string toJson = model.Serialize();
                toJson = toJson.Insert(toJson.Length - 1, $",\"ChildNodes:\":{ToMenuJson(data, model.F_Id).ToString()}");
                json.Append(toJson + ",");
            });
            return json.ToString().TrimEnd(',') + "]";
            
        }

        //获取角色列表
        private object GetRoleList()
        {

            var data = authorizeService.GetRoleList(1);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (Sys_Role item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_EnCode,
                    fullname = item.F_FullName
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }

        //获取基础数据分组列表
        private object GetDataItemList()
        {
            var itemdetails = authorizeService.GetItemDetailList();
            Dictionary<string, object> dictionaryItem = new Dictionary<string, object>();
            var items = authorizeService.GetList<Sys_Items>();
            foreach (var item in items)
            {
                var dataItemList = itemdetails.FindAll(t => t.F_ItemId.Equals(item.F_Id));
                Dictionary<string, string> dictionaryItemList = new Dictionary<string, string>();
                foreach (var itemList in dataItemList)
                {
                    dictionaryItemList.Add(itemList.F_ItemCode, itemList.F_ItemName);
                }
                dictionaryItem.Add(item.F_EnCode, dictionaryItemList);
            }
            return dictionaryItem;
        }

        //获取岗位列表
        private object GetDutyList()
        {
            var data = authorizeService.GetRoleList(2);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (Sys_Role item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_EnCode,
                    fullname = item.F_FullName
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }

        //获取菜单按钮列表
        private object GetMenuButtonList()
        {
            var roleId  = AdminUtils.GetCurrentCookie().RoleId;
            var data =  authorizeService.GetButtonList(roleId);
            var dataModuleId = data.Distinct(new ListExt<Sys_ModuleButton>("F_ModuleId"));
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (Sys_ModuleButton item in dataModuleId)
            {
                var buttonList = data.Where(t => t.F_ModuleId.Equals(item.F_ModuleId));
                dictionary.Add(item.F_ModuleId, buttonList);
            }
            return dictionary;
        }

        //获取组织列表
        private object GetOrganizeList()
        {
            var data = authorizeService.GetList<Sys_Organize>().OrderBy(x => x.F_CreatorTime).ToList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (Sys_Organize item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_EnCode,
                    fullname = item.F_FullName
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }
    }
}
