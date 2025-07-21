using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistentLayer.Interface;

namespace PersistentLayer.Data.DbAccess
{
    public class DbExecutor : IDbExecutor
    {
        private readonly IDbConnectionFactory connectionFactory;

        public DbExecutor(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        /// <summary>
        /// 泛用執行 SQL 指令
        /// </summary>
        public T Execute<T>(SqlCommand command, Func<SqlCommand, T> executeFunc)
        {
            SqlConnection connection = connectionFactory.CreateConnection();

            try
            {
                command.Connection = connection;
                connection.Open();
                return executeFunc(command);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Parameters.Clear();
                connection.Close();
            }
        }

        /// <summary>
        /// 執行 SQL 指令並回傳受影響的筆數
        /// </summary>
        public int ExecuteNonQuery(SqlCommand command)
        {
            try
            {
                return Execute(command, cmd =>
                {
                    int execnt = cmd.ExecuteNonQuery();
                    return execnt;
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 執行 SQL 指令並回傳資料表資料
        /// </summary>
        public DataTable ExecuteDataTable(SqlCommand command)
        {
            try
            {
                return Execute(command, cmd =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 執行 SQL 指令並回傳資料集資料
        /// </summary>
        public DataSet ExecuteDataSet(SqlCommand command)
        {
            try
            {
                return Execute(command, cmd =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 執行 SQL 指令並回傳資料表資料以及輸出參數
        /// </summary>
        public (DataTable dataTable, Dictionary<string, object> outputParams) ExecuteDataTableWithOutput(
            SqlCommand command, params string[] outputParamNames)
        {
            try
            {
                return Execute(command, cmd =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        Dictionary<string, object> output = new Dictionary<string, object>();

                        foreach (string paramName in outputParamNames)
                        {
                            output[paramName] = cmd.Parameters[paramName].Value;
                        }

                        return (dt, output);
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
