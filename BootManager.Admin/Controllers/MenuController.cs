using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreBoot.IServices;

namespace BootManager.Admin.Controllers
{
    public class MenuController : Controller
    {
        private IMenuService menuService;
        public MenuController(IMenuService service)
        {
            menuService = service;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}