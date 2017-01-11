using System.IO;
using Chloe;
using Chloe.MySql;
using Microsoft.Extensions.Configuration;

namespace NetCoreBoot.Data
{
    public class DbContextProvider
    {
        public static string ConnectionString { get; private set; }
        public static string DbType { get; private set; }
        static DbContextProvider()
        {
            var builder = new ConfigurationBuilder();
            var Config = builder.AddInMemoryCollection().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)?.Build();
            DbType = Config["DBType"];
            if(string.IsNullOrEmpty(DbType))
            {
                throw new System.Exception("数据库配置有误，请检查appsettings.json文件");
            }
            ConnectionString = Config?.GetConnectionString("DefaultConnection") ?? "User ID=root;Password=!@#$%^&*();Host=144.168.56.227;Database=CoreBoot;Pooling=true;";
        }

        public static IDbContext CreateContext()
        {
            IDbContext dbContext = CreateContext(ConnectionString);
            return dbContext;
        }

        public static IDbContext CreateContext(string connString)
        {
            IDbContext dbContext = null;
            if(DbType.ToLower() == "mysql")
            {
                dbContext = CreateMySqlContext(connString);
            }
            return dbContext;
        }

        static IDbContext CreateSqlServerContext(string connString)
        {
            //MsSqlContext dbContext = new MsSqlContext(connString);
            //return dbContext;
            return null;
        }
        static IDbContext CreateMySqlContext(string connString)
        {
            MySqlContext dbContext = new MySqlContext(new MySqlConnectionFactory(connString));
            return dbContext;
        }
        static IDbContext CreateOracleContext(string connString)
        {
            //OracleContext dbContext = new OracleContext(new OracleConnectionFactory(connString));
            //return dbContext;
            return null;
        }
        static IDbContext CreateSQLiteContext(string connString)
        {
            //SqliteContext dbContext = new SqliteContext(new SQLiteConnectionFactory(connString));
            //return dbContext;
            return null;
        }
    }
}
