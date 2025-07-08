using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using static System.TimeZoneInfo;

namespace PersistentLayer.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 付款失敗時, 修改狀態
        /// </summary>
        public bool EditCreditCardTransactionStatusFail(int creditCardTransactionId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editCreditCardTransactionStatusFail @creditCardTransactionId";

                cmd.Parameters.Add("@creditCardTransactionId", SqlDbType.Int).Value = creditCardTransactionId;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                // 受影響筆數>1代表成功
                if (ExeCnt > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

        /// <summary>
        /// 取得付款紀錄
        /// </summary>
        public (List<PaymentTransaction> transactions, int totalPage) GetTransaction(RequestGetTransactionDto getTransactionDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<PaymentTransaction> transactions = null;
            int totalPage = 0;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getPaymentTransaction @status, @method, " +
                    "@sortOrder, @sortOption, @recordPerPage, @page, @totalPage OUTPUT";

                if (getTransactionDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = getTransactionDto.Status;

                if (getTransactionDto.Method == null)
                    cmd.Parameters.Add("@method", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@method", SqlDbType.TinyInt).Value = getTransactionDto.Method;

                if (getTransactionDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getTransactionDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getTransactionDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getTransactionDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getTransactionDto.Page;
                SqlParameter totalPageOutput = new SqlParameter("@totalPage", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(totalPageOutput);


                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);
                totalPage = (int)totalPageOutput.Value;

                cmd.Connection.Close();

                if (dt.Rows.Count > 0) transactions = new List<PaymentTransaction>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    PaymentTransaction transaction = new PaymentTransaction();
                    transaction.TransactionId = dr.IsNull("f_transactionId") ? 0 : dr.Field<int>("f_transactionId");
                    transaction.OrderId = dr.IsNull("f_orderId") ? 0 : dr.Field<int>("f_orderId");
                    transaction.Status = (byte)(dr.IsNull("f_status") ? 0 : dr.Field<byte>("f_status"));
                    transaction.Amount = dr.IsNull("f_amount") ? 0 : dr.Field<int>("f_amount");
                    transaction.Method = (byte)(dr.IsNull("f_method") ? 0 : dr.Field<byte>("f_method"));
                    transaction.Time = dr.IsNull("f_time") ? DateTime.MinValue : dr.Field<DateTime>("f_time");
                    transactions.Add(transaction);
                }

                return (transactions, totalPage);
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

        /// <summary>
        /// 取得金流資訊(Auth code 跟 外部金流Id)
        /// </summary>
        public PaymentTransaction GetCreditCardCashFlowData(int transactionId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getCreditCardCashFlowData @transactionId";

                cmd.Parameters.Add("@transactionId", SqlDbType.Int).Value = transactionId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    PaymentTransaction transaction = new PaymentTransaction()
                    {
                        AuthCode = dr.IsNull("f_authCode") ? string.Empty : dr.Field<string>("f_authCode"),
                        GatewayTransactionId = dr.IsNull("f_gatewayTransactionId") ? string.Empty : dr.Field<string>("f_gatewayTransactionId"),
                    };

                    return transaction;
                }

                return null;
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
