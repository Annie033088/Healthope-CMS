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

                if (ExeCnt > 0) return true;

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
