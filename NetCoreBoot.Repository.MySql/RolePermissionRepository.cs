/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：角色权限表接口实现                                                    
*│　作    者：yilezhu                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-01-05 17:54:04                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： NetCoreBoot.Repository.MySql                                  
*│　类    名： RolePermissionRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using NetCoreBoot.Core.DBConnection;
using NetCoreBoot.Entity.CommonModel;
using NetCoreBoot.Core.Repository;
using NetCoreBoot.IRepository;
using NetCoreBoot.Entity;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.Repository.MySql
{
    public partial class RolePermissionRepository:BaseRepository<RolePermission,Int32>, IRolePermissionRepository
    {
      
		public int DeleteLogical(int[] ids)
        {
            string sql = "update RolePermission set IsDelete=1 where Id in @Ids";
            return _dbConnection.Execute(sql, new
            {
                Ids = ids
            });
        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update RolePermission set IsDelete=1 where Id in @Ids";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                Ids = ids
            });
        }

        /// <summary>
        /// 通过角色主键获取菜单主键数组
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public int[] GetIdsByRoleId(int RoleId)
        {
            string sql = "select MenuId from RolePermission where RoleId=@RoleId";
            return  _dbConnection.Query<int>(sql, new
            {
                RoleId = RoleId
            }).ToArray();
        }

    }
}