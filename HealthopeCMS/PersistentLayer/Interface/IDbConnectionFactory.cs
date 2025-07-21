using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Interface
{
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// 產生一個新的 SQL 連接
        /// </summary>
        SqlConnection CreateConnection();
    }
}
