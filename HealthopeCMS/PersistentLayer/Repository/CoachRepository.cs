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
    public class CoachRepository : ICoachRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增教練
        /// </summary>
        public OperationResult addCoach(Coach coach)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addCoach @name, @phone, @email, @type, " +
                    "@contractStartTime, @contractEndTime, @account, @hash, @introduction, @specialty, " +
                    "@certification, @photoUrl, @errorCode OUTPUT";

                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = coach.Name;
                cmd.Parameters.Add("@phone", SqlDbType.Int).Value = coach.Phone;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = coach.Email;
                cmd.Parameters.Add("@type", SqlDbType.TinyInt).Value = coach.Type;
                cmd.Parameters.Add("@contractStartTime", SqlDbType.Date).Value = coach.ContractStartTime;
                cmd.Parameters.Add("@contractEndTime", SqlDbType.Date).Value = coach.ContractEndTime;
                cmd.Parameters.Add("@account", SqlDbType.VarChar).Value = coach.Account;
                cmd.Parameters.Add("@hash", SqlDbType.VarChar).Value = coach.Hash;
                cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = coach.Introduction;
                cmd.Parameters.Add("@specialty", SqlDbType.NVarChar).Value = coach.Specialty;
                cmd.Parameters.Add("@certification", SqlDbType.NVarChar).Value = coach.Certification;
                cmd.Parameters.Add("@photoUrl", SqlDbType.NVarChar).Value = coach.PhotoUrl;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
                errorCodeNumber = (int)errorCodeOutput.Value;

                OperationResult result = new OperationResult()
                {
                    ErrorCodeNumber = errorCodeNumber,
                    Exception = null
                };
                return result;
            }
            catch (Exception ex)
            {
                int serverError = 6;
                OperationResult result = new OperationResult()
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
