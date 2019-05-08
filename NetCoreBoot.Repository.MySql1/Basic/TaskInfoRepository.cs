
using NetCoreBoot.Core.DBConnection;
using NetCoreBoot.Entity.CommonModel;
using NetCoreBoot.Core.Repository;
using NetCoreBoot.IRepository;
using NetCoreBoot.Entity;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace NetCoreBoot.Repository.MySql
{
    public partial class TaskInfoRepository : BaseRepository<TaskInfo, Int32>, ITaskInfoRepository
    {
        public TaskInfoRepository(IOptionsSnapshot<DbOption> options)
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