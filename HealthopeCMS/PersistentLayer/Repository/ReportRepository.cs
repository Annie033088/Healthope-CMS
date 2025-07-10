using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ApiLayer.Models.Report.Request;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace PersistentLayer.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得收支資料
        /// </summary>
        public List<ResponseGetRevenueExpenseDto> GetRevenueExpenseReport(RequestGetRevenueExpenseReportDto getFinancialStatementDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ResponseGetRevenueExpenseDto> getRevenueExpenses = new List<ResponseGetRevenueExpenseDto>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getRevenueExpenseReport @year, @month";

                cmd.Parameters.Add("@year", SqlDbType.Int).Value = getFinancialStatementDto.Year;

                if (getFinancialStatementDto.Month == null)
                    cmd.Parameters.Add("@month", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@month", SqlDbType.Int).Value = getFinancialStatementDto.Month;


                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    ResponseGetRevenueExpenseDto getRevenueExpense = new ResponseGetRevenueExpenseDto()
                    {
                        MembershipRevenue = dr.IsNull("f_membershipRevenue") ? 0 : dr.Field<int>("f_membershipRevenue"),
                        PersonalTrainingRevenue = dr.IsNull("f_personalTrainingRevenue") ? 0 : dr.Field<int>("f_personalTrainingRevenue"),
                        SingleEntryRevenue = dr.IsNull("f_singleEntryRevenue") ? 0 : dr.Field<int>("f_singleEntryRevenue"),
                        RefundExpense = dr.IsNull("f_refundExpense") ? 0 : dr.Field<int>("f_refundExpense"),
                        PenaltyIncome = dr.IsNull("f_penaltyIncome") ? 0 : dr.Field<int>("f_penaltyIncome"),
                    };
                    DateTime date = dr.IsNull("f_date") ? DateTime.MinValue : dr.Field<DateTime>("f_date");
                    getRevenueExpense.Year = getFinancialStatementDto.Year;
                    getRevenueExpense.Month = date.Month;
                    getRevenueExpense.Day = date.Day;

                    getRevenueExpenses.Add(getRevenueExpense);
                }

                return getRevenueExpenses;
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
