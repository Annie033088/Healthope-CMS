using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Interface
{
    public interface IDbExecutor
    {
        /// <summary>
        /// 泛用執行 SQL 指令
        /// </summary>
        T Execute<T>(SqlCommand command, Func<SqlCommand, T> executeFunc);

        /// <summary>
        /// 執行 SQL 指令並回傳受影響的筆數
        /// </summary>
        int ExecuteNonQuery(SqlCommand command);

        /// <summary>
        /// 執行 SQL 指令並回傳資料表資料
        /// </summary>
        DataTable ExecuteDataTable(SqlCommand command);

        /// <summary>
        /// 執行 SQL 指令並回傳資料集資料
        /// </summary>
        DataSet ExecuteDataSet(SqlCommand command);

        /// <summary>
        /// 執行 SQL 指令並回傳資料表資料以及輸出參數
        /// </summary>
        (DataTable dataTable, Dictionary<string, object> outputParams) ExecuteDataTableWithOutput(
            SqlCommand command, params string[] outputParamNames);
    }
}
