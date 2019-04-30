using NetCoreBoot.Core.Repository;
using NetCoreBoot.Entity;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NetCoreBoot.IRepository
{
    public partial interface IManagerRoleRepository : IBaseRepository<ManagerRole, Int32>
    {
    }
}