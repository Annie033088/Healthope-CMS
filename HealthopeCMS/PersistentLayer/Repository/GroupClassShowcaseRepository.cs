using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using System.Configuration;

namespace PersistentLayer.Repository
{
    public class GroupClassShowcaseRepository : IGroupClassShowcaseRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增展示用團課
        /// </summary>
        public ResultWithException AddShowcase(GroupClassShowcase groupClassShowcase)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addGroupClassShowcase @name, @summary, @detailContent" +
                    "@imageUrl, @category, @icon, @sort, @errorCode OUTPUT";

                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = groupClassShowcase.Name;
                cmd.Parameters.Add("@summary", SqlDbType.NVarChar).Value = groupClassShowcase.Summary;
                cmd.Parameters.Add("@detailContent", SqlDbType.NVarChar).Value = groupClassShowcase.DetailContent;
                cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = groupClassShowcase.ImageUrl;
                cmd.Parameters.Add("@category", SqlDbType.Int).Value = groupClassShowcase.Category;
                cmd.Parameters.Add("@icon", SqlDbType.Int).Value = groupClassShowcase.Icon;
                cmd.Parameters.Add("@sort", SqlDbType.Int).Value = groupClassShowcase.Sort;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
                errorCodeNumber = (int)errorCodeOutput.Value;

                ResultWithException result = new ResultWithException()
                {
                    ErrorCodeNumber = errorCodeNumber,
                    Exception = null
                };
                return result;
            }
            catch (Exception ex)
            {
                int serverError = 6;
                ResultWithException result = new ResultWithException()
                {
                    ErrorCodeNumber = serverError,
                    Exception = ex
                };
                return result;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }
    }
}
