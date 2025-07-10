using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DomainLayer.Models;
using PersistentLayer.Interface;

namespace PersistentLayer.Repository
{
    public class MemberPlanRepository : IMemberPlanRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 修改會籍狀態
        /// </summary>
        public int EditMemberMembershipPlanStatus(MemberMembershipPlan membershipPlan)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editMemberMembershipPlanStatus @memberMembershipPlanId, @status" +
                    ", @updateTime, @errorCode OUTPUT";

                cmd.Parameters.Add("@memberMembershipPlanId", SqlDbType.Int).Value = membershipPlan.MemberMembershipPlanId;
                cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = membershipPlan.Status;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = membershipPlan.UpdateTime;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
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

        public int EditMemberPersonalTrainingPackageCoach(MemberPersonalTrainingPackage memberPersonalTrainingPackage)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editMemberPersonalTrainingPackageCoach @memberPersonalTrainingPackageId, @coachId" +
                    ", @updateTime, @errorCode OUTPUT";

                cmd.Parameters.Add("@memberPersonalTrainingPackageId", SqlDbType.Int).Value = memberPersonalTrainingPackage.MemberPersonalTrainingPackageId;
                cmd.Parameters.Add("@coachId", SqlDbType.Int).Value = memberPersonalTrainingPackage.CoachId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = memberPersonalTrainingPackage.UpdateTime;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
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
    }
}
