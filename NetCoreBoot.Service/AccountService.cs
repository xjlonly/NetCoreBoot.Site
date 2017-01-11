using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.IService;
using NetCoreBoot.Entity;
using Chloe;
using NetCoreBoot.Common;

namespace NetCoreBoot.Service
{
    public class AccountService : BaseService, IAccountService
    {
        bool IAccountService.CheckLogin(string userName, string passWord, out Sys_User userInfo, out string msg)
        {
            userName.NotNullOrEmpty();
            passWord.NotNullOrEmpty();

            var isSuccess = false;
            userInfo = null;
            msg = null;
            Sys_User sys_user = this.Query<Sys_User>().Where(x => x.F_Account == userName).FirstOrDefault();
            userInfo = sys_user;
            if (sys_user.NotNull())
            {
                if(sys_user.F_EnabledMark == true)
                {

                    Sys_UserLogOn userLogOnEntity =  this.Query<Sys_UserLogOn>().Where(p => p.F_Id == sys_user.F_Id).FirstOrDefault();
                    this.DbContext.TrackEntity(userLogOnEntity);
                    string desString = Aes.Encrypt(passWord.ToLower(), userLogOnEntity.F_UserSecretkey).ToLower();
                    string endbPassword = Hash.MD5(desString);
                    if(endbPassword == userLogOnEntity.F_UserPassword)
                    {
                        DateTime dateTime = DateTime.Now;
                        userLogOnEntity.F_PreviousVisitTime = userLogOnEntity.F_LastVisitTime.NotNull() ? userLogOnEntity.F_LastVisitTime.ToDate() : userLogOnEntity.F_LastVisitTime;

                        userLogOnEntity.F_LastVisitTime = DateTime.Now;
                        userLogOnEntity.F_LogOnCount = userLogOnEntity.F_LogOnCount.ToInt() +1;
                        this.DbContext.Update(userLogOnEntity);
                        isSuccess = true;
                    }
                    else
                    {
                        msg = "账户或密码错误";
                        isSuccess = false;
                    }
                            
                }
                else
                {
                    msg = "账户被系统锁定,请联系管理员!";
                    isSuccess = false;
                }
            }
            else
            {
                msg = "账户或密码错误!";
                isSuccess = false;
            }
            

            return isSuccess;
        }

        
    }
}
