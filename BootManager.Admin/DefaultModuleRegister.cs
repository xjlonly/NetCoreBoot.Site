using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BootManager.Admin
{
    public class DefaultModuleRegister :Autofac.Module
    {
        /// <summary>
        /// 模块化注册服务
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            var assemblis = AppDomain.CurrentDomain.GetAssemblies().ToList();
            //注册Repository.MySql结尾的程序集中以“Repository”结尾的类,暴漏类实现的所有接口，生命周期为PerLifetimeScope
            builder.RegisterAssemblyTypes(assemblis.Find(x => x.FullName.Split(",")[0].EndsWith("Repository.MySql")))
                .Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            //注册Service结尾的程序集中以“Service”结尾的类,暴漏类实现的所有接口，生命周期为PerLifetimeScope
            builder.RegisterAssemblyTypes(assemblis.Find(x => x.FullName.Split(",")[0].EndsWith("Service")))
                .Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            //

        }
    }
}
