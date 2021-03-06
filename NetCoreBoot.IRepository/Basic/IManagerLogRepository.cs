using System;
using System.Threading.Tasks;
using NetCoreBoot.Core.Repository;
using NetCoreBoot.Entity;

namespace NetCoreBoot.IRepository
{
    public partial interface IManagerLogRepository : IBaseRepository<ManagerLog, Int32>
    {
    }
}