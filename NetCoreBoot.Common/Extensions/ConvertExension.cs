using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.Common
{
    public static class ConvertExension
    {
        public static int ToInt(this object data)
        {
            if(!data.NotNull())
            {
                return 0;
            }
            int result = 0;
            var success = int.TryParse(data.ToString(), out result);
            if (success)
                return result;
            return result;
        }

        public static DateTime ToDate(this object data)
        {
            if (data == null)
                return DateTime.MinValue;
            DateTime result;
            return DateTime.TryParse(data.ToString(), out result) ? result : DateTime.MinValue;
        }

    }
}
