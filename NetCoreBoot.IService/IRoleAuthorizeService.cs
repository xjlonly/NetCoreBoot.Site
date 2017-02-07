using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.IService
{
    public interface IRoleAuthorizeService
    {
        List<T> GetList<T>(string ObjectId);
    }
}
