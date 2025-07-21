using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace PersistentLayer.Repository
{
    public class RefundRepository : IRefundRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得退款紀錄
        /// </summary>
        public (List<ResponseGetRefundDto> refunds, int totalPage) GetRefund(RequestGetRefundDto getRefundDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ResponseGetRefundDto> refunds = null;
            int totalPage = 0;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getRefund @status, @refundType, " +
                    "@sortOrder, @sortOption, @recordPerPage, @page, @totalPage OUTPUT";

                if (getRefundDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = getRefundDto.Status;

                if (getRefundDto.RefundType == null)
                    cmd.Parameters.Add("@refundType", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@refundType", SqlDbType.TinyInt).Value = getRefundDto.RefundType;

                if (getRefundDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getRefundDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getRefundDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getRefundDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getRefundDto.Page;
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

                if (dt.Rows.Count > 0) refunds = new List<ResponseGetRefundDto>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    ResponseGetRefundDto refund = new ResponseGetRefundDto();
                    refund.RefundId = dr.IsNull("f_refundId") ? 0 : dr.Field<int>("f_refundId");
                    refund.OrderId = dr.IsNull("f_orderId") ? 0 : dr.Field<int>("f_orderId");
                    refund.MemberId = dr.IsNull("f_memberId") ? 0 : dr.Field<int>("f_memberId");
                    refund.MemberName = dr.IsNull("f_memberName") ? string.Empty : dr.Field<string>("f_memberName");
                    refund.MemberPhone = dr.IsNull("f_memberPhone") ? 0 : dr.Field<int>("f_memberPhone");
                    refund.RefundType = (byte)(dr.IsNull("f_refundType") ? 0 : dr.Field<byte>("f_refundType"));
                    refund.Status = (byte)(dr.IsNull("f_status") ? 0 : dr.Field<byte>("f_status"));
                    refund.RefundAmount = dr.IsNull("f_refundAmount") ? 0 : dr.Field<int>("f_refundAmount");
                    refund.PenaltyAmount = dr.IsNull("f_penaltyAmount") ? 0 : dr.Field<int>("f_penaltyAmount");
                    refund.CreateTime = dr.IsNull("f_createTime") ? DateTime.MinValue : dr.Field<DateTime>("f_createTime");
                    refunds.Add(refund);
                }

                return (refunds, totalPage);
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
