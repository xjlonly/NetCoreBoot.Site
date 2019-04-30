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
        public ArticleCategoryRepository(IOptionsSnapshot<DbOption> options)
        {
            //可配置多个数据库
            _dbOption =options.Get(_dbConnectionAliasName);
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

    }
}