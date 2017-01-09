using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NetCoreBoot.CommonApplication;
using NetCoreBoot.Common;

namespace NetCoreBoot.Admin.Controllers
{
    public class LoginController : WebController
    {

        private readonly WebHelper _webHelper;
        private readonly string cookie_key = "login_auth_code";
        public LoginController(WebHelper helper)
        {
            this._webHelper = helper;
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
            _webHelper.SetCookie(cookie_key, _webHelper.MD5(code.ToLower()), 10);
            return new FileContentResult(imgs, @"image/Gif");
        }

        [HttpPost]
        public IActionResult CheckLogin(string username, string password, string code)
        {
            if(string.IsNullOrEmpty(code))
            {
                return this.FailedMsg("请输入验证码!");
            }
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return this.FailedMsg("请输入用户名或密码!");
            }

            string cikcode = _webHelper.GetCookie(cookie_key);
            _webHelper.RemoveCookie(cookie_key);
            if(_webHelper.MD5(cikcode.ToLower()) != cikcode)
            {
                return this.FailedMsg("验证码输入错误，请重新输入！");
            }
            return View();
        }


    }
}
