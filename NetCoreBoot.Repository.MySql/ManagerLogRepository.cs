using NetCoreBoot.Core.DBConnection;
using NetCoreBoot.Entity.CommonModel;
using NetCoreBoot.Core.Repository;
using NetCoreBoot.IRepository;
using NetCoreBoot.Entity;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace NetCoreBoot.Repository.MySql
{
    public partial class ManagerLogRepository:BaseRepository<ManagerLog,Int32>, IManagerLogRepository
    {
     

		public int DeleteLogical(int[] ids)
        {
            string sql = "update ManagerLog set IsDelete=1 where Id in @Ids";
            return _dbConnection.Execute(sql, new
            {
                Ids = ids
            });
        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update ManagerLog set IsDelete=1 where Id in @Ids";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                Ids = ids
            });
        }

    }
}