using NetCoreBoot.Core.Repository;
using NetCoreBoot.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreBoot.IRepository
{
    public partial interface ITaskInfoRepository : IBaseRepository<TaskInfo, Int32>
    {
    }
}