using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using PersistentLayer.Interface;

namespace PersistentLayer.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增訂單
        /// </summary>
        public (Order order, int errorCodeNumber) AddOrder(Order addOrder, long orderNumber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addOrder @memberId, @planType, @planId, @method," +
                    " @orderNumber, @errorCode OUTPUT";

                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = addOrder.MemberId;
                cmd.Parameters.Add("@planType", SqlDbType.TinyInt).Value = addOrder.PlanType;
                cmd.Parameters.Add("@planId", SqlDbType.Int).Value = addOrder.PlanId;
                cmd.Parameters.Add("@method", SqlDbType.TinyInt).Value = addOrder.Method;
                cmd.Parameters.Add("@orderNumber", SqlDbType.BigInt).Value = orderNumber;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);
                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                errorCodeNumber = (int)errorCodeOutput.Value;
                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    Order order = new Order()
                    {
                        OrderId = dr.IsNull("f_orderId") ? 0 : dr.Field<int>("f_orderId"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };

                    return (order, errorCodeNumber);
                }

                return (null, errorCodeNumber);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }
    }
}
