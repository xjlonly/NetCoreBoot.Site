using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreBoot.Common;

namespace NetCoreBoot.CommonApplication
{ 
    public abstract class WebController : Controller
    {

        protected ContentResult JsonContent(object obj)
        {
            var result = JsonHelper.Serialize(obj);
            return base.Content(result);
        }

        protected ContentResult SuccessData(object obj = null)
        {
            Result<object> result = Result.CreateResult<object>(ResultStatus.OK, obj);
            return this.JsonContent(result);
        }


        protected ContentResult SuccessMsg(string msg)
        {
            return this.JsonContent(new Result(ResultStatus.OK, msg));
        }

        protected ContentResult AddSuccessData(object obj, string msg = "添加成功")
        {
            Result<object> result = Result.CreateResult<object>(ResultStatus.OK, obj);
            result.Msg = msg;
            return this.JsonContent(result);
        }

        protected ContentResult AddSuccessMsg(string msg = "添加成功")
        {
            return this.SuccessMsg(msg);
        }

        protected ContentResult UpdateSuccessMsg(string msg = "更新成功")
        {
            return this.SuccessMsg(msg);
        }
        protected ContentResult DeleteSuccessMsg(string msg = "删除成功")
        {
            return this.SuccessMsg(msg);
        }
        protected ContentResult FailedMsg(string msg = null)
        {
            Result retResult = new Result(ResultStatus.Failed, msg);
            return this.JsonContent(retResult);
        }

        LoginCookie _cookie;
        public LoginCookie CurrentCookie
        {
            get
            {
                if(this._cookie != null)
                {
                    return this._cookie;
                }
                var currentCookie = AdminUtils.GetCurrentCookie();
                this._cookie = currentCookie;
                return this._cookie;
            }
            set
            {
                LoginCookie cookie = value;
                AdminUtils.SetLoginCookie(cookie);
                this._cookie = cookie;
            }
        }
    }
}
