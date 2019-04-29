using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NetCoreBoot.Core;
using NetCoreBoot.Common;
using NetCoreBoot.IService;
using NetCoreBoot.Entity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ManagerBoot.Admin.Controllers
{
    public class LoginController : WebController
    {

        private readonly WebHelper _webHelper;
        private readonly string cookie_key = "login_auth_code";
        private IAccountService _accountService { get; set; }

        private IUserService _userServices { get; set; }
        public LoginController(WebHelper helper, IAccountService accountService, IUserService userService)
        {
            this._webHelper = helper;
            this._accountService = accountService;
            this._userServices = userService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAuthCode()
        {
            string code = string.Empty;
            byte[] imgs = VerifyCodeHelper.GetVerifyCode(out code);
            _webHelper.SetCookie(cookie_key, Hash.MD5(code.ToLower()), 10);
            return new FileContentResult(imgs, @"image/Gif");
        }

        [HttpPost]
        public IActionResult CheckLogin(string username, string password, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return this.FailedMsg("请输入验证码!");
            }
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return this.FailedMsg("请输入用户名或密码!");
            }

            string cikcode = _webHelper.GetCookie(cookie_key);
            _webHelper.RemoveCookie(cookie_key);
            if (Hash.MD5(code.ToLower()) != cikcode)
            {
                return this.FailedMsg("验证码输入错误，请重新输入！");
            }
            Sys_User userInfo = new Sys_User();
            string msg = string.Empty;
            if (!_accountService.CheckLogin(username, password, out userInfo, out msg))
            {
                return this.FailedMsg(msg);
            }

            this.CurrentCookie = new LoginCookie
            {
                Account = userInfo.F_Account,
                DepartmentId = userInfo.F_DepartmentId,
                DutyId = userInfo.F_DutyId,
                IsAdmin = userInfo.F_Account == "admin" ? true : false,
                LoginTime = DateTime.Now,
                RealName = userInfo.F_RealName,
                UserId = userInfo.F_Id,
                RoleId = userInfo.F_RoleId
            };

            _accountService.LogSync(userInfo.F_Id, userInfo.F_Account, userInfo.F_RealName, NetHelper.Ip, LogType.Login, "登录", true, "登录成功");

            return this.SuccessMsg(msg);
        }


    }
}
