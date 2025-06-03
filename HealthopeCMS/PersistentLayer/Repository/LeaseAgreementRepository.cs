using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using PersistentLayer.Interface;
using System.Configuration;
using PersistentLayer.Models;

namespace PersistentLayer.Repository
{
    public class LeaseAgreementRepository : ILeaseAgreementRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
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
        public (int errorCodeNumber, bool sendEmailFlag) EditLeaseAgreementStatus(LeaseAgreement leaseAgreement)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editLeaseAgreementStatus @leaseAgreementId, @status," +
                    " @updateTime, @errorCode OUTPUT";

                cmd.Parameters.Add("@leaseAgreementId", SqlDbType.Int).Value = leaseAgreement.LeaseAgreementId;
                cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = leaseAgreement.Status;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = leaseAgreement.UpdateTime;

                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();
                int result = (int)cmd.ExecuteScalar();
                bool sendEmailFlag = result == 1;
                errorCodeNumber = (int)errorCodeOutput.Value;

                return (errorCodeNumber, sendEmailFlag);
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
