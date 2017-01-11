using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.Common;
using NetCoreBoot.Data;
using NetCoreBoot.Entity;

namespace NetCoreBoot.DITest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string input = "4a7d1ed414474e4033ac29ccb8653d9b";
            string encry = "57d3031d6fc4a34d";
            string encry1 = "12345678";
            string result = Des.Encrypt(input, encry1);
            Console.WriteLine(result);
            //测试AES加密算法
            string aeskey = "12345678";
            string aes_result = Aes.Encrypt(input, aeskey);
            Console.WriteLine(aes_result);
            Console.WriteLine(Aes.Decrypt(aes_result, aeskey));

            Chloe.IDbContext dbContext = DbContextProvider.CreateContext();
            List<Sys_UserLogOn> list =  dbContext.Query<Sys_UserLogOn>().ToList();
            list.ForEach(model => {
                //dbContext.TrackEntity(model);
                string password = Hash.MD5(Aes.Encrypt(Hash.MD5("000000").ToLower(), model.F_UserSecretkey));
                //dbContext.Update(model);
                dbContext.DoWithTransaction(() =>
                {
                    dbContext.Update<Sys_UserLogOn>(a => a.F_UserId == model.F_UserId, a => new Sys_UserLogOn() { F_UserPassword = password });
                    
                });
                Console.WriteLine($"{model.F_UserId}密码更新完成！");
            });
            Console.ReadLine();
        }
    }
}
