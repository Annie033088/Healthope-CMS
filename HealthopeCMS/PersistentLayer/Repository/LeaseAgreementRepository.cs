using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace PersistentLayer.Repository
{
    public class LeaseAgreementRepository : ILeaseAgreementRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增條款
        /// </summary>
        public bool AddLeaseAgreement(LeaseAgreement leaseAgreement)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addLeaseAgreement @startTime, @endTime, @reminderLeadTime";

                cmd.Parameters.Add("@startTime", SqlDbType.Date).Value = leaseAgreement.StartTime;
                cmd.Parameters.Add("@endTime", SqlDbType.Date).Value = leaseAgreement.EndTime;
                cmd.Parameters.Add("@reminderLeadTime", SqlDbType.Int).Value = leaseAgreement.ReminderLeadTime;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                if (ExeCnt == 1) return true;

                return false;
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
        /// 取得條款
        /// </summary>
        public (List<LeaseAgreement> leaseAgreements, int totalPage) GetLeaseAgreement(RequestGetLeaseAgreementDto getLeaseAgreementDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage = 1;
            List<LeaseAgreement> leaseAgreements = new List<LeaseAgreement>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getLeaseAgreement @status, @recordPerPage, @page, @totalPage OUTPUT";

                if (getLeaseAgreementDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = getLeaseAgreementDto.Status;

                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getLeaseAgreementDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getLeaseAgreementDto.Page;
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

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    LeaseAgreement leaseAgreement = new LeaseAgreement()
                    {
                        LeaseAgreementId = dr.IsNull("f_leaseAgreementId") ? 0 : dr.Field<int>("f_leaseAgreementId"),
                        StartTime = dr.IsNull("f_startTime") ? DateTime.MinValue : dr.Field<DateTime>("f_startTime"),
                        EndTime = dr.IsNull("f_endTime") ? DateTime.MinValue : dr.Field<DateTime>("f_endTime"),
                        ReminderLeadTime = dr.IsNull("f_reminderLeadTime") ? 0 : dr.Field<int>("f_reminderLeadTime"),
                        Status = (byte)(dr.IsNull("f_status") ? 0 : dr.Field<byte>("f_status")),
                        Remind = dr.IsNull("f_remind") ? false : dr.Field<bool>("f_remind"),
                        Remark = dr.IsNull("f_remark") ? string.Empty : dr.Field<string>("f_remark"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime"),
                    };
                    leaseAgreements.Add(leaseAgreement);
                }

                return (leaseAgreements, totalPage);
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
        /// 修改租約狀態 (僅限未啟用=>啟用, 啟用=>已完成、取消)
        /// </summary>
        public (int errorCodeNumber, bool sendEmailFlag, DateTime leaseEndTime)
            EditLeaseAgreementStatus(LeaseAgreement leaseAgreement)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int errorCodeNumber;
            bool sendEmailFlag;
            DateTime leaseEndTime;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editLeaseAgreementStatus @leaseAgreementId, @status, @remark," +
                    " @updateTime, @errorCode OUTPUT";

                if (leaseAgreement.Remark == null)
                {
                    cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = leaseAgreement.Remark;
                }

                cmd.Parameters.Add("@leaseAgreementId", SqlDbType.Int).Value = leaseAgreement.LeaseAgreementId;
                cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = leaseAgreement.Status;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = leaseAgreement.UpdateTime;

                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);
                errorCodeNumber = (int)errorCodeOutput.Value;

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    sendEmailFlag = dr.IsNull("f_sendEmailFlag") ? false : dr.Field<int>("f_sendEmailFlag") == 1;
                    leaseEndTime = dr.IsNull("f_leaseEndTime") ? DateTime.MinValue : dr.Field<DateTime>("f_leaseEndTime");
                    return (errorCodeNumber, sendEmailFlag, leaseEndTime);
                }

                return (errorCodeNumber, false, DateTime.MinValue);
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
        /// 修改是否提醒
        /// </summary>
        public int EditLeaseAgreementRemind(LeaseAgreement leaseAgreement)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editLeaseAgreementRemind @leaseAgreementId, @remind, @updateTime" +
                    ", @errorCode OUTPUT";

                cmd.Parameters.Add("@leaseAgreementId", SqlDbType.Int).Value = leaseAgreement.LeaseAgreementId;
                cmd.Parameters.Add("@remind", SqlDbType.Bit).Value = leaseAgreement.Remind;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = leaseAgreement.UpdateTime;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                errorCodeNumber = (int)errorCodeOutput.Value;

                return errorCodeNumber;
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
        /// 刪除租約(僅限未啟用租約)
        /// </summary>
        public bool DeleteLeaseAgreement(int leaseAgreementId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_delLeaseAgreement @leaseAgreementId";

                cmd.Parameters.Add("@leaseAgreementId", SqlDbType.Int).Value = leaseAgreementId;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                if (ExeCnt == 1) return true;

                return false;
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
