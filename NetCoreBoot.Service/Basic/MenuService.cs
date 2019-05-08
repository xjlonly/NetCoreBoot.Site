
using System;
using System.Collections.Generic;
using System.Text;
using NetCoreBoot.IServices;
using NetCoreBoot.ViewModel;
using NetCoreBoot.IRepository;

namespace NetCoreBoot.Service
{
   
    public partial class MenuService : IMenuService
    {
        private IMenuRepository menuRepository;
        public MenuService(IMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }
    }
}
