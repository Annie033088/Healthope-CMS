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
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 付款失敗時, 修改狀態
        /// </summary>
        public bool EditCreditCardTransactionStatusFail(CreditCardTransaction creditCardTransaction)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editCreditCardTransactionStatusFail @creditCardTransactionId";

                cmd.Parameters.Add("@creditCardTransactionId", SqlDbType.Int).Value = creditCardTransaction.CreditCardTransactionId;

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
        /// 付款成功時, 修改狀態
        /// </summary>
        public bool EditCreditCardTransactionStatusSuccess(CreditCardTransaction creditCardTransaction)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editCreditCardTransactionStatusSuccess @creditCardTransactionId, @authCode, " +
                    "@cardLastFour, @cardType, @transactionId";

                cmd.Parameters.Add("@creditCardTransactionId", SqlDbType.Int).Value = creditCardTransaction.CreditCardTransactionId;
                cmd.Parameters.Add("@authCode", SqlDbType.Char).Value = creditCardTransaction.AuthCode;
                cmd.Parameters.Add("@cardLastFour", SqlDbType.Char).Value = creditCardTransaction.CardLastFour;
                cmd.Parameters.Add("@cardType", SqlDbType.VarChar).Value = creditCardTransaction.CardType;
                cmd.Parameters.Add("@transactionId", SqlDbType.Char).Value = creditCardTransaction.TransactionId;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                // 受影響筆數>0代表成功
                if (ExeCnt > 0)
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
    }
}
