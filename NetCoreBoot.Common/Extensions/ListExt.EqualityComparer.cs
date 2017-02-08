using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NetCoreBoot.Common
{
    public class ListExt<T> : IEqualityComparer<T> where T : new()
    {
     
        //属性名称列表
        private string[] comparintNames = { };

        public ListExt() { }
        public ListExt(params string[] comparintNames)
        {
            this.comparintNames = comparintNames;
        }

        public bool Equals(T x, T y)
        {
            if(x == null && y == null)
            {
                return false;
            }

            if(comparintNames.Length == 0)
            {
                return x.Equals(y);
            }

            bool result = true;
            var typeX = x.GetType();
            var typeY = y.GetType();
            foreach(var item in comparintNames)
            {
                var xPropertyInfo = (from p in typeX.GetProperties() where p.Name.Equals(item) select p).FirstOrDefault();
                var yPropertyInfo = (from p in typeY.GetProperties() where p.Name.Equals(item) select p).FirstOrDefault();

                result = result && xPropertyInfo != null && yPropertyInfo != null && xPropertyInfo.GetValue(x, null).ToString().Equals(yPropertyInfo.GetValue(y, null).ToString());
            }

            return result;

        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
