/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理菜单接口实现                                                    
*│　作    者：yilezhu                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-01-05 17:54:04                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： NetCoreBoot.Repository.MySql                                  
*│　类    名： MenuRepository                                      
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
    public partial class MenuRepository : BaseRepository<Menu, Int32>, IMenuRepository
    {
       

        public async Task<Int32> ChangeDisplayStatusByIdAsync(int id, bool status)
        {
            string sql = "update Menu set IsDisplay=@IsDisplay where  Id=@Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                IsDisplay = status ? 1 : 0,
                Id = id,
            });
        }

        public int DeleteLogical(int[] ids)
        {
            string sql = "update Menu set IsDelete=1 where Id in @Ids";
            return _dbConnection.Execute(sql, new
            {
                Ids = ids
            });
        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update Menu set IsDelete=1 where Id in @Ids";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                Ids = ids
            });
        }

        public async Task<Boolean> GetDisplayStatusByIdAsync(int id)
        {
            string sql = "select IsDisplay from Menu where Id=@Id and IsDelete=0";
            return await _dbConnection.QueryFirstOrDefaultAsync<bool>(sql, new
            {
                Id = id,
            });
        }

        public async Task<Boolean> IsExistsNameAsync(string Name)
        {
            string sql = "select Id from Menu where Name=@Name and IsDelete=0";
            var result = await _dbConnection.QueryAsync<int>(sql, new
            {
                Name = Name,
            });
            if (result != null && result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Boolean> IsExistsNameAsync(string Name, Int32 Id)
        {
            string sql = "select Id from Menu where Name=@Name and Id <> @Id and IsDelete=0";
            var result = await _dbConnection.QueryAsync<int>(sql, new
            {
                Name = Name,
                Id=Id,
            });
            if (result != null && result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}