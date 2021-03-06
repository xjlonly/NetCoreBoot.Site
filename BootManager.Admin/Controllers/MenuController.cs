﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreBoot.IServices;
using NetCoreBoot.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
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
        public string LoadData([FromQuery]MenuRequestModel requestModel)
        {
            return JsonConvert.SerializeObject(menuService.LoadData(requestModel));
        }
    }
}
