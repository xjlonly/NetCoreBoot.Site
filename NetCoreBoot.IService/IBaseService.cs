using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreBoot.IService
{
    public interface IBaseService
    {
        List<T> GetList<T>(Expression<Func<T, bool>> condition = null) where T : new();

        List<T> GetList<T>(int index, int pagesize, Expression<Func<T, bool>> condition = null) where T : new();
    }
}
