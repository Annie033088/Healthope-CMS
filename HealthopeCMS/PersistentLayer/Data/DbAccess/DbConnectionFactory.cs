using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Interface;
using PersistentLayer.Interface;

namespace PersistentLayer.Data.DbAccess
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IAppConfigProvider provider;

        public DbConnectionFactory(IAppConfigProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// 產生一個新的 SQL 連接
        /// </summary>
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(provider.GetConnectionString());
        }
    }
}
