using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chloe;
using NetCoreBoot.Data;
using Chloe.Infrastructure;
using System.Data;
using NetCoreBoot.IService;
using NetCoreBoot.Entity;

namespace NetCoreBoot.Service
{
    public abstract class BaseService : IDisposable, ISysLogService
    {
        IDbContext _dbContext = null;

        protected BaseService() 
            :this(null)
        {

        }

        public BaseService(object p)
        {
            this.p = p;
        }

        public IDbContext DbContext
        {
            get
            {
                if(this._dbContext == null)
                {
                    this._dbContext = DbContextProvider.CreateContext();
                }
                return _dbContext;
            }

            set
            {
                this._dbContext = value;
            }
        }

        public IQuery<T> Query<T>() where T : new ()
        {
            return DbContext.Query<T>();
        }

        //异步任务，主要进行日志操作
        public Task DoAsync(Action<IDbContext> act, bool? startTransaction = null)
        {
            return Task.Run(()=> {
                using (var dbContext = DbContextProvider.CreateContext())
                {
                    if(startTransaction.HasValue && startTransaction.Value)
                    {
                        dbContext.DoWithTransaction(()=> {
                            act(dbContext);
                        });
                    }
                    else
                    {
                        act(dbContext);
                    }
                }
            });
        }


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用
        private object p;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ServiceBase() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }


        void ISysLogService.Log(string userid, string account, string realName, string ip, LogType logType, string moduleName, bool? result, string description)
        {
            Sys_Log log = this.CreateLog(userid, account, realName, ip, logType, moduleName, result, description);
            this.DbContext.Insert(log);
        }


        Task ISysLogService.LogSync(string userid,string account, string realName, string ip, LogType logType, string moduleName, bool? result, string description)
        {
            Sys_Log log = this.CreateLog(userid, account, realName, ip, logType, moduleName, result, description);

            return this.DoAsync(dbContext =>
            {
                dbContext.Insert(log);
            });
        }

        private Sys_Log CreateLog(string userid, string account, string realName, string ip, LogType logType, string moduleName, bool? result, string description, string moduleId="")
        {
            Sys_Log entity = new Sys_Log
            {
                F_Account = account,
                F_ModuleId = moduleId,
                F_Result = result,
                F_CreatorTime = DateTime.Now,
                F_CreatorUserId = userid,
                F_Description = description,
                F_IPAddress = ip,
                F_Type = logType.ToString(),
                F_NickName = realName,
                F_ModuleName = moduleName,
            };

            return entity;
        }
        #endregion

    }
}
