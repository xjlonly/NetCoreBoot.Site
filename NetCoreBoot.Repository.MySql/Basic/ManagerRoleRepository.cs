/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员角色接口实现                                                    
*│　作    者：yilezhu                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2018-12-18 13:28:43                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： NetCoreBoot.Repository.MySql                                  
*│　类    名： ManagerRoleRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using NetCoreBoot.Core.DBConnection;
using NetCoreBoot.Entity.CommonModel;
using NetCoreBoot.Core.Repository;
using NetCoreBoot.IRepository;
using NetCoreBoot.Entity;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace NetCoreBoot.Repository.MySql
{
    public partial class ManagerRoleRepository:BaseRepository<ManagerRole,Int32>, IManagerRoleRepository
    {
        public ManagerRoleRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get(_dbConnectionAliasName);
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

    }
}