using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetCoreBoot.Core.DBConnection;
using NetCoreBoot.Entity.CommonModel;
using NetCoreBoot.Core.Repository;
using NetCoreBoot.IRepository;
using NetCoreBoot.Entity;
using Dapper;

namespace NetCoreBoot.Repository.MySql
{
    public partial class ArticleCategoryRepository:BaseRepository<ArticleCategory,Int32>, IArticleCategoryRepository
    {

		public int DeleteLogical(int[] ids)
        {
            string sql = "update ArticleCategory set IsDelete=1 where Id in @Ids";
            return _dbConnection.Execute(sql, new
            {
                Ids = ids
            });
        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update ArticleCategory set IsDelete=1 where Id in @Ids";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                Ids = ids
            });
        }

    }
}