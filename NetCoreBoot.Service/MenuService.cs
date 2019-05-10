using NetCoreBoot.IServices;
using NetCoreBoot.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using NetCoreBoot.Common;
using System.Linq;

namespace NetCoreBoot.Service
{
    public partial class MenuService : IMenuService
    {
       
        public TableDataModel LoadData(MenuRequestModel model)
        {
            string where = "  where IsDelete=0 ";
            if (!model.Key.IsEmptyOrNull())
            {
                where += " and DisplayName like '%@key%'";
            }

            return new TableDataModel
            {
                count = menuRepository.RecordCount(where),
                data = menuRepository.GetListPaged(model.PageIndex, model.PageSize, where, "", new {model.Key}).ToList()
            };
        }
    }
}
